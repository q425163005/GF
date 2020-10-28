using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Procedure;
using System;
using System.Collections.Generic;
using Fuse.Hotfix.Common;
using UnityGameFramework.Runtime;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public partial class ProcedureChangeScene : ProcedureBase
    {
        /// <summary>
        /// 是否切换场景完毕
        /// </summary>
        private bool m_IsChangeSceneComplete = false;

        /// <summary>
        /// 背景音乐ID
        /// </summary>
        private int m_BackgroundMusicId = 0;

        /// <summary>
        /// 要切换目标场景ID
        /// </summary>
        private int m_TargetSceneId = 0;

        private string SceneName;
        private string TargetProcedure;


        public static LoadingUI loadingUI;

        protected internal override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            //TODO:在这里配置场景ID与切换到对应流程的方法
            //m_TargetProcedureChange.Add((int)SceneId.TestScene, () => ChangeState<ProcedureHotfixTest>(procedureOwner));
        }

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Mgr.UI.Show<LoadingUI>();

            SceneName       = procedureOwner.GetData<VarString>("SceneName");
            TargetProcedure = procedureOwner.GetData<VarString>("TargetProcedure");

            m_IsChangeSceneComplete = false;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();
            //根据行数据里的场景资源名加载场景
            GameEntry.Scene.LoadScene(Constant.AssetPath.Scene(SceneName), Constant.AssetPriority.SceneAsset, this);
            //m_BackgroundMusicId = drScene.BackgroundMusicId;
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            base.OnLeave(procedureOwner, isShutdown);

            loadingUI?.SetProgress(1);
            Mgr.UI.Close<LoadingUI>();
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
                                                  float          realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            loadingUI = Mgr.UI.GetUI<LoadingUI>();
            if (!m_IsChangeSceneComplete || loadingUI==null)
            {
                return;
            }

            Type type = Type.GetType("Fuse.Hotfix." + TargetProcedure);
            ChangeState(procedureOwner, type);

            //根据切换到的目标场景ID进行对应的流程切换
            //            if (m_TargetProcedureChange.ContainsKey(m_TargetSceneId))
            //            {
            //                m_TargetProcedureChange[m_TargetSceneId]?.Invoke();
            //            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info($"Load scene '{ne.SceneAssetName}' OK.");

//            if (m_BackgroundMusicId > 0)
//            {
//                GameEntry.Sound.PlaySound() .PlayMusic(m_BackgroundMusicId);
//            }

            m_IsChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info($"Load scene '{ne.SceneAssetName}' update, progress '{ne.Progress.ToString("P2")}'.");
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info(
                $"Load scene '{ne.SceneAssetName}' dependency asset '{ne.DependencyAssetName}', count '{ne.LoadedCount}/{ne.TotalCount}'.");
        }
    }
}