using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public class ProcedureHotfixEntry : ProcedureBase
    {

        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了热更新入口流程");

            PreloadResources();


            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            //GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            Mgr.Event.Subscribe();

//            UpdateResourceForm.SetProgress(1f, GameEntry.Localization.GetString("ForceUpdate.Message"));
//            
//            Mgr.UI.Show<LoginUI>();
         

            //TODO:在这里切换到游戏的正式开始场景
            //            procedureOwner.SetData<VarInt>(Fuse.Constant.ProcedureData.NextSceneId, (int)SceneId.TestScene);
            //            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        //加载配置文件
        private void PreloadResources()
        {
            LoadLangConfig();//语言表
        }

        /// <summary>加载语言表</summary>
        private void LoadLangConfig()
        {
            
        }
    }
}