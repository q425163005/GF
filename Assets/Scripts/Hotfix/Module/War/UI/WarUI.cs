using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Fuse.Tasks;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;

namespace Fuse.Hotfix.War
{
    public partial class WarUI : BaseUI
    {
        class RemoveData
        {
            public Vector2 mianPos;

            public int power;

            public List<Vector2> AllList = new List<Vector2>();

            public bool canRemove => AllList.Count > 4;
        }


        public ProcedureHotfix_War procedure;

        private bool enableClick = true;

        private WarData                        warData;
        private Vector2                        CellSpace;
        private Dictionary<Vector2, FrameItem> frameDic      = new Dictionary<Vector2, FrameItem>();
        private Queue<FrameItem>               FreeFrameList = new Queue<FrameItem>();

        private Dictionary<Vector2, SquareItem> squareDic      = new Dictionary<Vector2, SquareItem>();
        private Queue<SquareItem>               FreeSquareList = new Queue<SquareItem>();
        private Queue<SquareItem>               NewCreateList  = new Queue<SquareItem>();


        private SquareItem    SelSquare  = null;
        private List<Vector2> allPosList = new List<Vector2>();

        private List<RemoveData> RemoveList = new List<RemoveData>();
        private int              dropNum    = 0;

        public WarUI()
        {
            UIGroup = EUIGroup.Window;
        }

        protected override void Init(object userdata)
        {
            Btn_Close.AddClick(Btn_Close_Click);
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
                        FrameItem item = GetOneFrame();
                        Vector2   pos  = new Vector2(i, -j);
                        allPosList.Add(pos);

                        item.rectTransform.anchoredPosition = retVec2;
                        item.SetData(pos);
                        frameDic.Add(pos, item);
                    }
                }
            }

            CreateDropList();
            DropToMap();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (RemoveList.Count > 0)
            {
                if (RemoveList.Count == dropNum)
                {
                    dropNum = 0;
                    MoveEnd();
                }
            }
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
            if (!enableClick) return;
            if (SelSquare == null) return;
            if (!item.isNull) return;

            //TODO : find path to move
            frameDic[SelSquare.Data.Pos].isNull = true;
            item.isNull                         = false;
            SelSquare.SetSel(false);
            Vector2 po = SelSquare.Data.Pos;
            SelSquare.Data.Pos = item.Pos;
            squareDic.Add(item.Pos, SelSquare);

            squareDic.Remove(po);
            RemoveList.Add(new RemoveData
            {
                mianPos = item.Pos,
                power   = SelSquare.Data.Power,
                AllList = GetAround(item.Pos),
            });
            SquareMove(SelSquare, item.rectTransform.anchoredPosition);
            SelSquare = null;
        }

        /// <summary>方块区域</summary>
        private void SquareClick(SquareItem item)
        {
            if (!enableClick) return;

            SelSquare?.SetSel(false);

            SelSquare = item;
            SelSquare.SetSel(true);
        }

        /// <summary>移动结束</summary>
        private void MoveEnd()
        {
            List<Vector2> removeIndexList = new List<Vector2>();
            for (int i = 0; i < RemoveList.Count; i++)
            {
                RemoveData data1 = RemoveList[i];
                if (removeIndexList.Contains(data1.mianPos)) continue;
                for (int j = 0; j < RemoveList.Count; j++)
                {
                    if (i != j)
                    {
                        RemoveData data2 = RemoveList[j];
                        if (data2.AllList.Contains(data1.mianPos))
                        {
                            removeIndexList.Add(data2.mianPos);
                        }
                    }
                }
            }

            for (int i = 0; i < removeIndexList.Count; i++)
            {
                RemoveList.RemoveAll(s => removeIndexList.Contains(s.mianPos));
            }

            for (int i = 0; i < RemoveList.Count; i++)
            {
                RemoveData data = RemoveList[i];
                if (data.canRemove)
                {
                    for (int j = 0; j < data.AllList.Count; j++)
                    {
                        Vector2 vec2 = data.AllList[j];

                        if (data.mianPos != vec2)
                        {
                            SquareItem item = squareDic[vec2];
                            frameDic[vec2].isNull = true;
                            item.SetActive(false);
                            FreeSquareList.Enqueue(item);
                            squareDic.Remove(vec2);
                        }
                        else
                        {
                            SquareItem item = squareDic[vec2];
                            item.Data.Power += 2;
                            item.Refresh();
                        }
                    }
                }
            }

            RemoveList.Clear();
            Debug.Log(123);
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
            List<FrameItem> nullFrameList = frameDic.Values.ToList().FindAll(s => s.isNull);
            nullFrameList = RandomHelper.RandomGetNum(nullFrameList, NewCreateList.Count);

            for (int i = 0; i < nullFrameList.Count; i++)
            {
                FrameItem  frame = nullFrameList[i];
                SquareItem item  = NewCreateList.Dequeue();
                squareDic.Add(frame.Pos, item);
                item.Data.Pos = frame.Pos;
                item.OnClick  = SquareClick;
                frame.isNull  = false;
                SquareMove(item, frame.rectTransform.anchoredPosition);

                RemoveList.Add(new RemoveData
                {
                    mianPos = frame.Pos,
                    power   = item.Data.Power,
                    AllList = GetAround(frame.Pos),
                });
            }
        }

        private List<Vector2> GetAround(Vector2 pos)
        {
            int targetPower = squareDic[pos].Data.Power;

            List<Vector2> linkList = new List<Vector2>();
            linkList.Add(pos);

            Queue<Vector2> temp = new Queue<Vector2>();

            foreach (var variable in GetSquareAround(pos, targetPower))
            {
                temp.Enqueue(variable);
            }

            while (temp.Count > 0)
            {
                Vector2 _traget = temp.Dequeue();
                linkList.Add(_traget);

                List<Vector2> _around = GetSquareAround(_traget, targetPower);
                foreach (var variable in _around)
                {
                    if (!linkList.Contains(variable))
                    {
                        temp.Enqueue(variable);
                    }
                }
            }

            foreach (var upper in linkList)
            {
                Log.Info(upper);
            }
            Log.Info($"{pos}__{linkList.Count}");
            return linkList;
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

        private bool FrameAroundNull(Vector2 pos)
        {
            Vector2 temp = pos + new Vector2(1, 1);
            if (allPosList.Contains(temp) && !frameDic[temp].isNull) return false;
            temp = pos + new Vector2(1, -1);
            if (allPosList.Contains(temp) && !frameDic[temp].isNull) return false;
            temp = pos + new Vector2(0, 2);
            if (allPosList.Contains(temp) && !frameDic[temp].isNull) return false;
            temp = pos + new Vector2(0, -2);
            if (allPosList.Contains(temp) && !frameDic[temp].isNull) return false;
            temp = pos + new Vector2(-1, 1);
            if (allPosList.Contains(temp) && !frameDic[temp].isNull) return false;
            temp = pos + new Vector2(-1, -1);
            if (allPosList.Contains(temp) && !frameDic[temp].isNull) return false;
            return true;
        }

        #endregion

        #region SquareItem

        private SquareItem GetOneSquare()
        {
            if (FreeSquareList.Count > 0)
            {
                return FreeSquareList.Dequeue();
            }

            SquareItem item = new SquareItem();
            item.Instantiate(SquareItem, SquareContent);
            item.SetActive(true);
            return item;
        }

        private void SquareMove(SquareItem item, Vector2 endPos)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(item.rectTransform.DOAnchorPos(endPos, 0.5f).SetEase(Ease.InQuad));
            sequence.Append(item.rectTransform.DOScale(0.8f, 0.1f).SetLoops(2, LoopType.Yoyo));
            sequence.OnComplete(() => { dropNum++; });
            sequence.Play();
        }

        private List<Vector2> GetSquareAround(Vector2 pos, int targetPower)
        {
            List<Vector2> retList = new List<Vector2>();
            Vector2       temp    = pos + new Vector2(1, 1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2(1, -1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2(0, 2);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2(0, -2);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2(-1, 1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            temp = pos + new Vector2(-1, -1);
            if (allPosList.Contains(temp) && squareDic.ContainsKey(temp))
                if (squareDic[temp].Data.Power == targetPower)
                    retList.Add(temp);
            return retList;
        }

        #endregion
    }
}