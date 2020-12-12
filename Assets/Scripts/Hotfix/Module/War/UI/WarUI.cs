using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Fuse.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.War
{
    public partial class WarUI : BaseUI
    {
        public ProcedureHotfix_War procedure;

        #region 参数

        private const float squareDropTime = 0.3f;
        private const float squareZoomTime = 0.2f;
        private       float squareMoveTime(int num) => Mathf.Clamp(0.6f * num, 0.6f, 0.3f);

        #endregion

        private WarData                           warData;
        private Vector2                           CellSpace;
        private Dictionary<Vector3Int, FrameItem> frameDic      = new Dictionary<Vector3Int, FrameItem>();
        private Queue<FrameItem>                  FreeFrameList = new Queue<FrameItem>();

        private Dictionary<Vector3Int, SquareItem> squareDic      = new Dictionary<Vector3Int, SquareItem>();
        private Queue<SquareItem>                  FreeSquareList = new Queue<SquareItem>();
        private Queue<SquareItem>                  NewCreateList  = new Queue<SquareItem>();

        private bool canMove = false;

        private SquareItem SelSquare = null;
        private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

        private bool isdrop     = false;
        private bool isFreeStep = false;

        private int              dropNum   = 0;
        private List<Vector3Int> MatchList = new List<Vector3Int>();


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

            allNodes.Clear();
            //allPosList.Clear();
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
                        Vector3Int pos  = new Vector3Int(i, (j + i) / 2, (j - i) / 2);
                        Node       node = new Node();
                        node.pos = pos;
                        allNodes.Add(pos, node);

                        FrameItem item = GetOneFrame();
                        item.rectTransform.anchoredPosition = retVec2;
                        item.SetData(pos);
                        frameDic.Add(pos, item);

                        item.gameObject.GetComponentInChildren<Text>().text = $"{pos.x},{pos.y},{pos.z}";
                    }
                }
            }

            foreach (var variable in allNodes)
            {
                variable.Value.around = AroundList(variable.Key);
            }

            CreateDropList();
            DropToMap();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

//            if (MatchList.Count > 0)
//            {
//                if (MatchList.Count == dropNum)
//                {
//                    dropNum = 0;
//                    StartMatchSquare();
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
            if (!canMove) return;
            if (SelSquare == null) return;

            if (!FindPath(SelSquare.Data.Pos, item.Pos, out var path) || squareDic.ContainsKey(item.Pos))
            {
                Log.Error("can not move");
                return;
            }

            canMove = false;
            isdrop  = false;

            //TODO : find path to move
            SelSquare.SetSel(false);
            Vector3Int po = SelSquare.Data.Pos;
            SelSquare.Data.Pos = item.Pos;
            squareDic.Add(item.Pos, SelSquare);
            squareDic.Remove(po);

            MatchList = new List<Vector3Int> {SelSquare.Data.Pos};
            Transform transform = SelSquare.transform;
            transform.DOLocalPath(path.ToArray(), squareMoveTime(path.Count))
                     .SetEase(Ease.Linear)
                     .OnStart(() => { transform.localScale = Vector3.one * 0.5f; })
                     .OnComplete(() =>
                     {
                         transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
                         StartMatchSquare().Run();
                     });

            SelSquare = null;
        }

        /// <summary>方块区域</summary>
        private void SquareClick(SquareItem item)
        {
            if (!canMove) return;
            SelSquare?.SetSel(false);

            SelSquare = item;
            SelSquare.SetSel(true);
        }

        /// <summary>随机生成下次方块</summary>
        private void CreateDropList()
        {
            int createNum                      = 4;
            int lastNum                        = allNodes.Count - squareDic.Count;
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
            canMove = false;

            List<Vector3Int> nullList = allNodes.Keys.ToList();
            nullList  = nullList.FindAll(s => !squareDic.ContainsKey(s));
            MatchList = RandomHelper.RandomGetNum(nullList, NewCreateList.Count);
            isdrop    = true;

            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < MatchList.Count; i++)
            {
                Vector3Int pos  = MatchList[i];
                SquareItem item = NewCreateList.Dequeue();
                squareDic.Add(pos, item);
                item.Data.Pos = pos;
                item.OnClick  = SquareClick;
                Sequence sequence1 = SquareMove(item, frameDic[pos].rectTransform.anchoredPosition);
                sequence.Insert(0, sequence1);
            }

            sequence.OnComplete(() =>
            {
                CreateDropList();
                StartMatchSquare().Run();
            });
            sequence.Play();
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

        private void RecycleSquare(Vector3Int pos)
        {
            if (!squareDic.ContainsKey(pos)) return;
            SquareItem item = squareDic[pos];
            item.SetActive(false);
            FreeSquareList.Enqueue(item);
            squareDic.Remove(pos);
        }

        private Sequence SquareMove(SquareItem item, Vector2 endPos)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, item.rectTransform.DOScale(0.8f, 0.1f));
            sequence.Append(item.rectTransform.DOAnchorPos(endPos, squareDropTime));
            sequence.Append(item.rectTransform.DOScale(1f, squareZoomTime).SetEase(Ease.OutBack));
            sequence.Play();
            return sequence;
        }

        private List<Node> GetSquareAround(Node node, int targetPower)
        {
            List<Node> retNodes = new List<Node>();
            foreach (var variable in node.around)
            {
                if (squareDic.TryGetValue(variable.pos, out var value))
                {
                    if (value.Data.Power == targetPower)
                    {
                        retNodes.Add(variable);
                    }
                }
            }

            return retNodes;
        }

        #endregion

        #region Match

        private async CTask StartMatchSquare()
        {
            int matchNum = await MatchSquareList();

            canMove = true;

            if (!isdrop)
            {
                if (matchNum == 0)
                {
                    DropToMap();
                }
            }
        }

        private async CTask<int> MatchSquareList()
        {
            Sequence         sequence     = DOTween.Sequence();
            List<Vector3Int> newCheckList = new List<Vector3Int>();
            for (var index = 0; index < MatchList.Count; index++)
            {
                var variable = MatchList[index];
                if (MatchSquare(variable, out Sequence value))
                {
                    sequence.Insert(0, value);
                    newCheckList.Add(variable);
                }
            }

            sequence.Play();
            MatchList = newCheckList;
            int matchNum = MatchList.Count;
            if (matchNum > 0)
            {
                await CTask.WaitUntil(() => sequence.IsComplete());
                matchNum += await MatchSquareList();
            }

            return matchNum;
        }

        private bool MatchSquare(Vector3Int pos, out Sequence sequence)
        {
            sequence = DOTween.Sequence();
            if (!squareDic.ContainsKey(pos)) return false;
            if (!GetMatchList(pos, out var _list)) return false;

            SquareItem mainItem = squareDic[pos];
            foreach (var variable in _list)
            {
                if (variable.matchPath.Count == 0) continue;
                if (!squareDic.ContainsKey(variable.pos)) continue;

                SquareItem moveItem = squareDic[variable.pos];
                squareDic.Remove(variable.pos);
                Tweener tween = moveItem
                                .transform.DOLocalPath(variable.matchPath.ToArray(),
                                                       squareMoveTime(variable.matchPath.Count))
                                .SetEase(Ease.Linear)
                                .OnStart(() => { moveItem.transform.localScale = Vector3.one * 0.5f; })
                                .OnComplete(() =>
                                {
                                    moveItem.SetActive(false);
                                    moveItem.transform.localScale = Vector3.one;
                                    FreeSquareList.Enqueue(moveItem);
                                });
                sequence.Insert(0, tween);
            }

            sequence.OnComplete(() =>
            {
                mainItem.Data.AddPower();
                mainItem.Refresh();
                //dropNum++;
            });
            sequence.Play();
            return true;
        }

        private bool GetMatchList(Vector3Int startPos, out List<Node> matchList)
        {
            List<Node> chekList = new List<Node>();
            matchList = new List<Node>();

            int  targetPower = squareDic[startPos].Data.Power;
            Node startNode   = allNodes[startPos];
            startNode.parent = null;
            startNode.matchPath.Clear();
            chekList.Add(startNode);

            while (chekList.Count > 0)
            {
                List<Node> tempList = new List<Node>();
                foreach (var up in chekList)
                {
                    List<Node> around = GetSquareAround(up, targetPower);
                    if (around.Count == 0) continue;
                    foreach (var variable in around)
                    {
                        if (matchList.Contains(variable) || chekList.Contains(variable)) continue;

                        variable.parent    = up;
                        variable.matchPath = up.matchPath.ToList();
                        variable.matchPath.Add(frameDic[up.pos].transform.localPosition);
                        variable.matchPath.Reverse();
                        tempList.Add(variable);
                    }

                    matchList.Add(up);
                }

                chekList = tempList;
            }

            Log.Info($"match : {startPos}\ncount : {matchList.Count}");
            return matchList.Count >= 4;
        }

        #endregion

        #region A*

        class Node
        {
            public Vector3Int pos;
            public Node       parent;

            // 与起点的长度
            public int gCost;

            // 与目标点的长度
            public int hCost;

            // 总的路径长度
            public int fCost
            {
                get { return gCost + hCost; }
            }

            public List<Node> around = new List<Node>();

            public List<Vector3> matchPath = new List<Vector3>();

            public void SetParent(Node node, int g)
            {
                parent = node;
                gCost  = g;
            }

            public void SetHCost(Vector3Int target)
            {
                int len = Mathf.Abs(target.x - pos.x) + Mathf.Abs(target.y - pos.y) + Mathf.Abs(target.z - pos.z);
                hCost = len / 2;
            }
        }

        private bool FindPath(Vector3Int startPos, Vector3Int endPos, out List<Vector3> path)
        {
            List<Node> openList  = new List<Node>();
            List<Node> closeList = new List<Node>();

            Node nowNode = allNodes[startPos];
            nowNode.parent = null;
            nowNode.gCost  = 0;
            openList.Add(nowNode);

            bool finded = false;
            while (!finded)
            {
                nowNode = GetMinOfList(openList);

                openList.Remove(nowNode);
                closeList.Add(nowNode);

                for (int i = 0; i < nowNode.around.Count; i++)
                {
                    Node neighbor = nowNode.around[i];

                    if (neighbor.pos == endPos)
                    {
                        //TODO finded
                        finded = true;
                        neighbor.SetParent(nowNode, nowNode.gCost + 1);
                    }

                    if (closeList.Contains(neighbor) || squareDic.ContainsKey(neighbor.pos))
                    {
                        continue;
                    }

                    if (!openList.Contains(neighbor))
                    {
                        neighbor.SetParent(nowNode, nowNode.gCost + 1);
                        neighbor.SetHCost(endPos);
                        openList.Add(neighbor);
                    }
                    else
                    {
                        if (nowNode.gCost + 1 < neighbor.gCost)
                        {
                            neighbor.SetParent(nowNode, nowNode.gCost + 1);
                        }
                    }
                }

                if (openList.Count <= 0)
                {
                    Log.Error("无法到达该目标");
                    break;
                }
            }

            path = new List<Vector3>();
            if (finded)
            {
                Node endNode = allNodes[endPos];
                while (endNode.parent != null)
                {
                    path.Add(frameDic[endNode.pos].transform.localPosition);
                    endNode = endNode.parent;
                }

                path.Reverse();
            }

            return finded;
        }

        private List<Node> AroundList(Vector3Int pos)
        {
            List<Node> retList = new List<Node>();
            Vector3Int temp    = pos + new Vector3Int(0, 1, 1);
            if (allNodes.ContainsKey(temp))
                retList.Add(allNodes[temp]);
            temp = pos + new Vector3Int(1, 1, 0);
            if (allNodes.ContainsKey(temp))
                retList.Add(allNodes[temp]);
            temp = pos + new Vector3Int(1, 0, -1);
            if (allNodes.ContainsKey(temp))
                retList.Add(allNodes[temp]);
            temp = pos + new Vector3Int(0, -1, -1);
            if (allNodes.ContainsKey(temp))
                retList.Add(allNodes[temp]);
            temp = pos + new Vector3Int(-1, -1, 0);
            if (allNodes.ContainsKey(temp))
                retList.Add(allNodes[temp]);
            temp = pos + new Vector3Int(-1, 0, 1);
            if (allNodes.ContainsKey(temp))
                retList.Add(allNodes[temp]);
            return retList;
        }

        private Node GetMinOfList(List<Node> list)
        {
            int  min  = int.MaxValue;
            Node node = null;
            foreach (Node p in list)
            {
                if (p.fCost < min)
                {
                    min  = p.fCost;
                    node = p;
                }
            }

            return node;
        }

        #endregion
    }
}