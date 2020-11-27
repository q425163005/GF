using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fuse.Hotfix.War
{
    public partial class WarUI : BaseUI
    {
        public ProcedureHotfix_War procedure;

        private WarData          warData;
        private Vector2          CellSpace;
        private List<FrameItem>  frameList  = new List<FrameItem>();
        private List<SquareItem> squareList = new List<SquareItem>();

        public WarUI()
        {
            UIGroup = EUIGroup.Window;
        }

        protected override void Init(object userdata)
        {
        }

        protected override void Refresh(object userData = null)
        {
            warData = (WarData) userData;
            int     createNum = 0;
            int     num_x     = warData.half_Width;
            int     num_y     = warData.half_Height;
            Vector2 Spacing   = warData.Spacing;
            Vector2 CellSize  = FrameItem.GetComponent<RectTransform>().sizeDelta;
            CellSpace = CellSize + Spacing;

            foreach (var variable in frameList)
            {
                variable.SetActive(false);
            }

            for (int j = -num_y + 1; j < num_y; j++)
            {
                for (int i = -num_x + 1; i < num_x; i++)
                {
                    if (GetPos(i, j, out Vector2 retVec2))
                    {
                        CreateOneItem(ref createNum, retVec2, new Vector2(i, -j));
                    }
                }
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        /// <summary>退出</summary>
        private void Btn_Exit_Click()
        {
            procedure.ExitWar();
        }

        private void CreateOneItem(ref int index, Vector2 pos, Vector2 ind)
        {
            FrameItem item;
            if (index < frameList.Count)
            {
                item = frameList[index];
            }
            else
            {
                item = new FrameItem();
                item.Instantiate(FrameItem, FrameContent);
                frameList.Add(item);
            }

            item.gameObject.name                = $"{ind.x}_{ind.y}";
            item.rectTransform.anchoredPosition = pos;
            item.gameObject.SetActive(true);
            item.SetData();
            index++;
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
    }
}