using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Fuse.Tasks;
using UnityEngine;

namespace Fuse.Hotfix.War
{
    public partial class WarUI : BaseUI
    {
        public ProcedureHotfix_War procedure;

        private WarData                        warData;
        private Vector2                        CellSpace;
        private Dictionary<Vector2, FrameItem> frameDic      = new Dictionary<Vector2, FrameItem>();
        private Queue<FrameItem>               FreeFrameList = new Queue<FrameItem>();

        private Dictionary<Vector2, SquareItem> squareDic      = new Dictionary<Vector2, SquareItem>();
        private Queue<SquareItem>               FreeSquareList = new Queue<SquareItem>();
        private Queue<SquareItem>               NewCreateList  = new Queue<SquareItem>();

        private int moveNum = 1;

        private SquareItem       SelSquare  = null;
        private List<Vector2Int> allPosList = new List<Vector2Int>();

        private List<Vector2Int> MatchList  = new List<Vector2Int>();
        private int              dropNum    = 0;
        private bool             isdrop     = false;
        private bool             isFreeStep = false;

        public WarUI()
        {
            UIGroup = EUIGroup.Window;
        }

        protected override void Init(object userdata)
        {
            Btn_Close.AddClick(CreateDropList);
            Btn_Drop.AddClick(DropToMap);
        }

        protected override void Disposed()
        {
            base.Disposed();

            for (int i = NewCreateList.Count - 1; i >= 0; i--)
            {
                NewCreateList.Dequeue().SetActive(false);
            }

            foreach (var variable in squareDic)
            {
                variable.Value.SetActive(false);
                variable.Value.gameObject.name = "free square";
            }

            foreach (var variable in frameDic)
            {
                variable.Value.SetActive(false);
                variable.Value.gameObject.name = "free frame";
            }

            squareDic.Clear();
            FreeSquareList.Clear();
            frameDic.Clear();
            FreeFrameList.Clear();

            allPosList.Clear();
        }

        protected override void Refresh(object userData = null)
        {
            foreach (Transform variable in FrameContent)
            {
                FrameItem item = new FrameItem();
                item.Instantiate(variable.gameObject);
                FreeFrameList.Enqueue(item);
            }

            foreach (Transform variable in SquareContent)
            {
                SquareItem item = new SquareItem();
                item.Instantiate(variable.gameObject);
                FreeSquareList.Enqueue(item);
            }

            warData = (WarData) userData;
            int     num_x    = warData.half_Width;
            int     num_y    = warData.half_Height;
            Vector2 Spacing  = warData.Spacing;
            Vector2 CellSize = FrameItem.GetComponent<RectTransform>().sizeDelta;
            CellSpace = CellSize + Spacing;

            for (int j = -num_y + 1; j < num_y; j++)
            {
                for (int i = -num_x + 1; i < num_x; i++)
                {
                    if (GetPos(i, j, out Vector2 retVec2))
                    {
                        FrameItem  item = GetOneFrame();
                        Vector2Int pos  = new Vector2Int(i, j);
                        allPosList.Add(pos);

                        item.rectTransform.anchoredPosition = retVec2;
                        item.SetData(pos);
                        frameDic.Add(pos, item);
                    }
                }
            }

//            CreateDropList();
//            DropToMap();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

//            if (MatchList.Count > 0)
//            {
//                if (MatchList.Count == dropNum)
//                {
//                    dropNum = 0;
//                    StartMatch(MatchList).Run();
//                }
//            }
        }

        /// <summary>退出</summary>
        private void Btn_Close_Click()
        {
            CreateDropList();
            return;
            procedure.ExitWar();
        }

        /// <summary>选择区域</summary>
        private void FrameClick(FrameItem item)
        {
            if (moveNum   <= 0) return;
            if (SelSquare == null) return;


            Stopwatch s = Stopwatch.StartNew();
            s.Start();
            if (!FindPath(SelSquare.Data.Pos, item.Pos, out var path))
            {
                Log.Error("can not move");
                return;
            }

            s.Stop();
            Log.Info($"time:{s.Elapsed}");

            //TODO : find path to move
            SelSquare.SetSel(false);
            Vector2 po = SelSquare.Data.Pos;
            SelSquare.Data.Pos = item.Pos;
            squareDic.Add(item.Pos, SelSquare);
            squareDic.Remove(po);

            float aniTime = 0.1f * path.Count;
            aniTime = Mathf.Clamp(aniTime, 0.1f, 0.5f);
            SelSquare.rectTransform.DOLocalPath(path.ToArray(), aniTime).SetEase(Ease.Linear);
            return;

            if (squareDic.ContainsKey(item.Pos)) return;

            isdrop = false;
            MatchList.Clear();
            MatchList.Add(item.Pos);
            SquareMove(SelSquare, item.rectTransform.anchoredPosition);
            SelSquare = null;
            moveNum   = 0;
        }

        /// <summary>方块区域</summary>
        private void SquareClick(SquareItem item)
        {
            if (moveNum <= 0) return;
            Log.Info(item.Data.Pos);
            SelSquare?.SetSel(false);

            SelSquare = item;
            SelSquare.SetSel(true);
        }

        /// <summary>随机生成下次方块</summary>
        private void CreateDropList()
        {
            int createNum                      = 4;
            int lastNum                        = allPosList.Count - squareDic.Count;
            if (lastNum < createNum) createNum = lastNum;
            if (createNum == 0)
            {
                //TODO : game end
                Log.Error("Game Over");
                return;
            }

            for (int i = 0; i < createNum; i++)
            {
                SquareItem item = GetOneSquare();
                NewCreateList.Enqueue(item);
                float a = 0.5f * (CellSpace.x - warData.Spacing.x);
                if (createNum % 2 == 0)
                    a = a * (2 * i - 3);
                else
                    a = a * (2 * i - 1.5f);

                item.rectTransform.anchoredPosition = new Vector2(a, 0) + CreatePoint.anchoredPosition;

                int _maxPower                = warData.nowMaxPower - 2;
                if (_maxPower < 2) _maxPower = 2;

                int _power = RandomHelper.Random(0, _maxPower);
                item.SetData(new SquareData(_power));
                item.transform.DOScale(0, 0.5f).From().SetEase(Ease.OutBack);
            }
        }

        private void DropToMap()
        {
            List<Vector2Int> nullList = allPosList.FindAll(s => !squareDic.ContainsKey(s));
            MatchList = RandomHelper.RandomGetNum(nullList, NewCreateList.Count);
            isdrop    = true;
            for (int i = 0; i < MatchList.Count; i++)
            {
                Vector2Int pos  = MatchList[i];
                SquareItem item = NewCreateList.Dequeue();
                squareDic.Add(pos, item);
                item.Data.Pos = pos;
                item.OnClick  = SquareClick;
                SquareMove(item, frameDic[pos].rectTransform.anchoredPosition);
            }
        }

        #region FrameItem

        private FrameItem GetOneFrame()
        {
            if (FreeFrameList.Count > 0)
            {
                return FreeFrameList.Dequeue();
            }

            FrameItem item = new FrameItem();
            item.Instantiate(FrameItem, FrameContent);
            item.OnClick = FrameClick;
            item.SetActive(true);
            return item;
        }

        private bool GetPos(int i, int j, out Vector2 retVec2)
        {
            retVec2   = Vector2.zero;
            retVec2.x = i * CellSpace.x;

            if (Mathf.Abs(i % 2) == 0)
            {
                if (Mathf.Abs(j % 2) == 1)
                    return false;
                retVec2.y = j / 2 * CellSpace.y;
            }
            else
            {
                if (Mathf.Abs(j % 2) == 0)
                    return false;
                retVec2.y = (j / 2 + 0.5f) * CellSpace.y;
                retVec2.y = 0.5f * j       * CellSpace.y;
            }

            return true;
        }

        private List<Vector2Int> GetAroundFreePoint(Vector2Int pos)
        {
            List<Vector2Int> retList = new List<Vector2Int>();
            Vector2Int       temp    = pos + new Vector2Int(1, 1);
            if (allPosList.Contains(temp) && !squareDic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(1, -1);
            if (allPosList.Contains(temp) && !squareDic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(0, 2);
            if (allPosList.Contains(temp) && !squareDic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(0, -2);
            if (allPosList.Contains(temp) && !squareDic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(-1, 1);
            if (allPosList.Contains(temp) && !squareDic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(-1, -1);
            if (allPosList.Contains(temp) && !squareDic.ContainsKey(temp))
                retList.Add(temp);
            return retList;
        }

        #endregion

        #region SquareItem

        private SquareItem GetOneSquare()
        {
            SquareItem item;
            if (FreeSquareList.Count > 0)
            {
                item = FreeSquareList.Dequeue();
            }
            else
            {
                item = new SquareItem();
                item.Instantiate(SquareItem, SquareContent);
            }

            item.SetActive(true);
            return item;
        }

        private void RecycleSquare(Vector2 pos)
        {
            if (!squareDic.ContainsKey(pos)) return;
            SquareItem item = squareDic[pos];
            item.SetActive(false);
            FreeSquareList.Enqueue(item);
            squareDic.Remove(pos);
        }

        private void SquareMove(SquareItem item, Vector2 endPos)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(item.rectTransform.DOAnchorPos(endPos, 0.3f));
            sequence.Append(item.rectTransform.DOScale(0.8f, 0.1f).SetLoops(2, LoopType.Yoyo));
            sequence.OnComplete(() => { dropNum++; });
            sequence.Play();
        }

        private List<Vector2Int> GetSquareAround(Vector2Int pos, int targetPower)
        {
            List<Vector2Int> retList = new List<Vector2Int>();
            Vector2Int       temp    = pos + new Vector2Int(1, 1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2Int(1, -1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2Int(0, 2);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2Int(0, -2);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2Int(-1, 1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2Int(-1, -1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            return retList;
        }

        #endregion

        private List<Vector2Int> GetLinkSquare(Vector2Int pos)
        {
            List<Vector2Int>  linkList  = new List<Vector2Int>();
            Queue<Vector2Int> linkQueue = new Queue<Vector2Int>();
            linkQueue.Enqueue(pos);
            int power = squareDic[pos].Data.Power;

            while (linkQueue.Count > 0)
            {
                Vector2Int vec2 = linkQueue.Dequeue();
                if (!linkList.Contains(vec2))
                {
                    linkList.Add(vec2);
                    List<Vector2Int> around = GetSquareAround(vec2, power);
                    foreach (var variable in around)
                    {
                        if (!linkList.Contains(variable))
                        {
                            linkQueue.Enqueue(variable);
                        }
                    }
                }
            }

            return linkList;
        }

        private async CTask<int> MatchSquare(List<Vector2Int> m_list)
        {
            int              Num       = 0;
            List<Vector2Int> checkList = new List<Vector2Int>();

            for (int i = 0; i < m_list.Count; i++)
            {
                if (squareDic.TryGetValue(m_list[i], out var item))
                {
                    List<Vector2Int> linkList = GetLinkSquare(m_list[i]);
                    if (linkList.Count >= 4)
                    {
                        foreach (var variable in linkList)
                        {
                            if (!variable.Equals(m_list[i]))
                            {
                                Log.Info($"回收：{variable}");
                                squareDic[variable]
                                    .rectTransform
                                    .DOAnchorPos(squareDic[m_list[i]].rectTransform.anchoredPosition, 0.5f);
                                RecycleSquare(variable);
                            }
                        }

                        Num++;
                        item.Data.AddPower();
                        checkList.Add(m_list[i]);
                    }
                }
            }

            if (Num > 0)
            {
                await CTask.WaitForSeconds(0.5f);
                List<Vector2Int> list = new List<Vector2Int>();
                foreach (var variable in checkList)
                {
                    if (squareDic.TryGetValue(variable, out var item))
                    {
                        if (item.Data.Number > 2048)
                        {
                            //TODO boom
                            RecycleSquare(variable);
                        }
                        else
                        {
                            item.Refresh();
                            list.Add(variable);
                        }
                    }
                }

                int matchNum = await MatchSquare(list);
                return Num + matchNum;
            }

            return 0;
        }

        private async CTask StartMatch(List<Vector2Int> m_list)
        {
            int matchNum = await MatchSquare(m_list);
            Txt_FreeMove.SetActive(isFreeStep);
            if (!isFreeStep)
            {
                if (isdrop)
                {
                    CreateDropList();
                    await CTask.WaitForSeconds(0.6f);
                }
                else
                {
                    if (matchNum == 0)
                    {
                        DropToMap();
                    }
                }
            }

            moveNum = 1;
        }


        #region A*

        class Node
        {
            public Vector2Int pos;
            public List<Node> parent = new List<Node>();
            public int        weight = 0;

            public List<Vector2Int> around = new List<Vector2Int>();
        }

        private bool FindPath(Vector2Int startPos, Vector2Int endPos, out List<Vector3> path)
        {
            Dictionary<Vector2Int, Node> openList  = new Dictionary<Vector2Int, Node>();
            List<Vector2Int>             closeList = new List<Vector2Int>();

            Node startNode = new Node();
            startNode.pos = startPos;
            openList.Add(startPos, startNode);

            Node endNode = null;

            while (openList.Count > 0 || endNode == null)
            {
                Dictionary<Vector2Int, Node> allAround = new Dictionary<Vector2Int, Node>();
                foreach (var up in openList)
                {
                    List<Vector2Int> around = AroundList(up.Key);
                    foreach (var variable in around)
                    {
                        if (!openList.ContainsKey(variable)
                         && !closeList.Contains(variable)
                         && !allAround.ContainsKey(variable))
                        {
                            if (squareDic.ContainsKey(variable)) continue;
                            Node node = new Node
                            {
                                pos    = variable,
                                weight = up.Value.weight + 1
                            };
                            node.parent.Add(up.Value);
                            allAround.Add(variable, node);
                            if (endPos.Equals(variable)) endNode = node;
                        }
                    }

                    closeList.Add(up.Key);
                }

                openList = allAround;
            }

            path = new List<Vector3>();
            if (endNode != null)
            {
                Vector3 vec3;
                while (endNode.parent.Count > 0)
                {
                    vec3 = frameDic[endNode.pos].transform.localPosition;
                    path.Add(vec3);
                    endNode = endNode.parent.First();
                }

                vec3 = frameDic[startPos].transform.localPosition;
                path.Add(vec3);
                path.Reverse();
            }

            return endNode != null;
        }

        private List<Vector2Int> AroundList(Vector2Int pos)
        {
            List<Vector2Int> retList = new List<Vector2Int>();
            Vector2Int       temp    = pos + new Vector2Int(1, 1);
            if (allPosList.Contains(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(1, -1);
            if (allPosList.Contains(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(0, 2);
            if (allPosList.Contains(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(0, -2);
            if (allPosList.Contains(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(-1, 1);
            if (allPosList.Contains(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(-1, -1);
            if (allPosList.Contains(temp))
                retList.Add(temp);
            return retList;
        }

        #endregion
    }
}