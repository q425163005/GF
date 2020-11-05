using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

namespace Fuse
{
    public class ProcedureTest : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            GameEntry.UI.AddUIGroup("Common");
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            
            //GameEntry.UI.OpenUIForm($"Assets/Res/BundleRes/UI/Common/TipsUI.prefab", "Common");
        }
    }
}
