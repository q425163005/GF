using System;
using UnityEditor.Graphs;
using UnityEngine.UI;

namespace Fuse.Hotfix.Store
{
    public partial class StoreUI : BaseUI
    {
        enum EState
        {
            Pay   = 0,
            Gift  = 1,
            Equip = 2,
            Prop  = 3
        }

        private EState state = EState.Pay;


        public StoreUI()
        {
            UIGroup = EUIGroup.Window;
        }

        protected override void Init(object userData)
        {
            Btn_Back.AddClick(CloseSelf);

            Tog_Pay.AddClick(Toggle_Click, EState.Pay);
            Tog_Gift.AddClick(Toggle_Click, EState.Gift);
            Tog_Equip.AddClick(Toggle_Click, EState.Equip);
            Tog_Prop.AddClick(Toggle_Click, EState.Prop);

            Init_Pay();
            Init_Gift();
            Init_Equip();
            Init_Prop();
        }

        protected override void Refresh(object userData = null)
        {
            switch (state)
            {
                case EState.Pay:
                    Refresh_Pay();
                    break;
                case EState.Gift:
                    Refresh_Gift();
                    break;
                case EState.Equip:
                    Refresh_Equip();
                    break;
                case EState.Prop:
                    Refresh_Prop();
                    break;
            }
        }

        private void Toggle_Click(EState targetState)
        {
            if (state != targetState)
            {
                state = targetState;
            }

            PayContent.SetActive(state   == EState.Pay);
            GiftContent.SetActive(state  == EState.Gift);
            EquipContent.SetActive(state == EState.Equip);
            PropContent.SetActive(state  == EState.Prop);
            Refresh();
        }

        protected override void Disposed()
        {
            Disposed_Pay();
            Disposed_Gift();
            Disposed_Equip();
            Disposed_Prop();
        }
    }
}