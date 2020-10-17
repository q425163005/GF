using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public class ProcedureHotfixEntry : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了热更新入口流程");

            UpdateResourceForm.SetProgress(1f, GameEntry.Localization.GetString("ForceUpdate.Message"));
            
            Mgr.UI.Show<LoginUI>();
         

            //TODO:在这里切换到游戏的正式开始场景
            //            procedureOwner.SetData<VarInt>(Fuse.Constant.ProcedureData.NextSceneId, (int)SceneId.TestScene);
            //            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }
}