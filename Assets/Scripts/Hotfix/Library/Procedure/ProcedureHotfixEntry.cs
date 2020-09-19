using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public class ProcedureHotfixEntry : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了热更新入口流程");

            UpdateResourceForm m_UpdateResourceForm = (UpdateResourceForm)GameEntry.UI.GetUIForm(UpdateResourceForm.AssetFullPath).Logic;
            m_UpdateResourceForm.SetProgress(1f, GameEntry.Localization.GetString("ForceUpdate.Message"));

            //GameEntry.UI.CloseUIForm(m_UpdateResourceForm);

            //TODO:在这里切换到游戏的正式开始场景
            //            procedureOwner.SetData<VarInt>(Fuse.Constant.ProcedureData.NextSceneId, (int)SceneId.TestScene);
            //            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }
}

