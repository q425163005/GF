using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public class ProcedureHotfixPreload : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了热更资源加载流程");
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
                                                  float          realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds); 
            
            if (!Mgr.Config.isAllLoaded) return;
            if (!Mgr.Lang.isLoaded) return;
            ChangeState<ProcedureHotfixTest>(procedureOwner);
            //ChangeScene<ProcedureHotfixTest>(procedureOwner, "Home");
           // ChangeScene(procedureOwner, "Home", "ProcedureHotfixTest");
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}