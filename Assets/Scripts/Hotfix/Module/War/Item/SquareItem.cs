using System;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Fuse.Hotfix.War
{
    public partial class SquareItem : BaseItem
    {
        public SquareData Data;
        public Action<SquareItem> OnClick;

        protected override void Awake()
        {
            base.Awake();
            transform.GetComponent<Button>().AddClick(ClickEvent);
        }

        public void SetData(SquareData data)
        {
            Data = data;
            gameObject.name = $"{Data.Pos.x}_{Data.Pos.y}";
            Refresh();
        }

        public void Refresh()
        {
            if (Data==null)return;
            
            Txt_Num.text = Data.Number.ToString();
            Img_This.color = Data.GetColor;
        }

        public override void Disposed()
        {
            base.Disposed();
        }

        private void ClickEvent()
        {
            OnClick?.Invoke(this);
        }

        public void SetSel(bool sel)
        {
            imgSel.SetActive(sel);
        }
    }
}