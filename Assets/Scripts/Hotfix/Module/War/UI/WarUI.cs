using UnityEngine;
using UnityEngine.EventSystems;

namespace Fuse.Hotfix.War
{
    public partial class WarUI : BaseUI
    {
        public ProcedureHotfix_War procedure;
        
        public WarUI()
        {
            UIGroup = EUIGroup.Window;
        }

        protected override void Init(object userdata)
        {
           
            
        }

        protected override void Refresh(object userData = null)
        {
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
    }
}