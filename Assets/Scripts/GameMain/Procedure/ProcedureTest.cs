//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Fuse
{
    public class ProcedureTest : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get { return true; }
        }


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.UI.OpenUIForm(UpdateResourceForm.AssetFullPath, "UpdateRes");

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs uievEventArgs = (OpenUIFormSuccessEventArgs) e;

            Log.Info("open UpdateRes success");
            Log.Info(uievEventArgs.UIForm.UIFormAssetName);
        }

        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
        }
    }
}