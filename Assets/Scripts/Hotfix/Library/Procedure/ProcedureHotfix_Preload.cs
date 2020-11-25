using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public class ProcedureHotfix_Preload : ProcedureBase
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
            
            //Log.Info($"{Mgr.Config.loadCount}---{Mgr.Config.loadedCount}");
            if (!Mgr.Config.isAllLoaded) return;
            if (!Mgr.Lang.isLoaded) return;
            procedureOwner.ChangeState<ProcedureHotfix_Login>();
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}