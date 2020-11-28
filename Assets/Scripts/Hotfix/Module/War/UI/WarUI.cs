using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fuse.Hotfix.War
{
    public partial class WarUI : BaseUI
    {
        public ProcedureHotfix_War procedure;

        private bool enableClick = true;

        private WarData                        warData;
        private Vector2                        CellSpace;
        private Dictionary<Vector2, FrameItem> frameDic      = new Dictionary<Vector2, FrameItem>();
        private Queue<FrameItem>               FreeFrameList = new Queue<FrameItem>();

        private List<SquareItem>  squareList = new List<SquareItem>();
        private Queue<SquareItem> NewList    = new Queue<SquareItem>();
        private Queue<SquareItem> FreeList   = new Queue<SquareItem>();

        private SquareItem SelSquare = null;

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
            for (int i = 0; i < squareList.Count; i++)
            {
                squareList[i].SetActive(false);
                squareList[i].gameObject.name = "free square";
                squareList.Remove(squareList[i]);
                FreeList.Enqueue(squareList[i]);
            }

            List<FrameItem> frameItems = frameDic.Values.ToList();
            for (int i = 0; i < frameItems.Count; i++)
            {
                FrameItem item = frameItems[i];

                item.SetActive(false);
                item.gameObject.name = "free frame";
                frameDic.Remove(item.Pos);
                item.isNull = true;
                FreeFrameList.Enqueue(item);
            }
        }

        protected override void Refresh(object userData = null)
        {
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

                        item.rectTransform.anchoredPosition = retVec2;
                        item.SetData(pos);
                        frameDic.Add(pos, item);
                    }
                }
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
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
            SelSquare.Data.Pos                  = item.Pos;
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
        }

        /// <summary>随机生成下次方块</summary>
        private void CreateDropList()
        {
            int createNum              = 4;
            int lastNum                = warData.allCount() - squareList.Count;
            if (lastNum < 4) createNum = lastNum;
            if (createNum == 0)
            {
                //TODO : game end
                Log.Error("Game Over");
                return;
            }

            for (int i = 0; i < createNum; i++)
            {
                SquareItem item = GetOneSquare();
                NewList.Enqueue(item);
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
            List<FrameItem> nullFrameList =
                RandomHelper.RandomGetNum(frameDic.Values.ToList().FindAll(s => s.isNull), NewList.Count);
            for (int i = 0; i < nullFrameList.Count; i++)
            {
                SquareItem item = NewList.Dequeue();
                squareList.Add(item);
                item.Data.Pos                  = nullFrameList[i].Pos;
                item.OnClick                   = SquareClick;
                frameDic[item.Data.Pos].isNull = false;

                SquareMove(item, nullFrameList[i].rectTransform.anchoredPosition);
            }
        }

        #region Create FrameItem

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
            if (FreeList.Count > 0)
            {
                return FreeList.Dequeue();
            }

            SquareItem item = new SquareItem();
            item.Instantiate(SquareItem, SquareContent);
            item.SetActive(true);
            return item;
        }

        private void SquareMove(SquareItem item, Vector2 endPos)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(item.rectTransform.DOAnchorPos(endPos, 1f));
            sequence.Append(item.rectTransform.DOScale(0.8f, 0.1f).SetLoops(2, LoopType.Yoyo));
            sequence.Play();
        }

        #endregion
    }
}