using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.War
{
    public partial class FrameItem : BaseItem
    {
        //public bool    isNull = true;
        public Action<FrameItem>  OnClick;
        public Vector2Int Pos;

        protected override void Awake()
        {
            base.Awake();
            gameObject.GetComponent<Button>().AddClick(Click);
        }

        public override void Disposed()
        {
            base.Disposed();
        }

        public void SetData(Vector2Int pos)
        {
            Pos             = pos;
            gameObject.name = $"{Pos.x}_{Pos.y}";
            Refresh();
        }

        private void Click()
        {
            OnClick?.Invoke(this);
        }

        private void Refresh()
        {
        }
    }
}