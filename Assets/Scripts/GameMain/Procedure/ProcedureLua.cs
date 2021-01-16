using System;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;


namespace Fuse
{
    /// <summary>
    /// lua流程
    /// </summary>
    public class ProcedureLua : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        class luaList
        {
            public List<string> toLua = new List<string>();
            public List<string> Lua   = new List<string>();
        }

        private static string HotFixPath  = "Assets/Res/BundleRes/Data/HotFix/";
        private static string ToLuaPath   = HotFixPath           + "ToLua/";
        private static string LuaPath     = HotFixPath           + "Lua/";
        private static string LuaListName = "LuaList.txt";

        private HashSet<string> m_LoadFlags       = new HashSet<string>();
        private bool            m_LoadConfigFlags = false;

        // private static string LuaPath = Constant.ToLua.AssetPath + "/";
        // private static string ToLuaPath = Constant.ToLua.AssetPath + "/ToLua/Lua/";
        private ProcedureOwner owner             = null;
        private bool           m_ReadyEnterLogin = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner;

            // GameEntry.Event.Subscribe(LoadSceneEventArgs.EventId, OnLoadScene);
            // GameEntry.Event.Subscribe(LoadSingleSceneEventArgs.EventId, OnLoadSingleScene);

            // if (Application.isEditor && GameEntry.Base.EditorResourceMode)
            // {
            //     m_LoadConfigFlags = true;
            //     return;
            // }

            GameEntry.Lua.LoadFile(HotFixPath + LuaListName, LuaListName,
                                   OnLoadConfigScriptSuccess, OnLoadConfigScriptFailure, false);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_LoadConfigFlags && m_LoadFlags.Count <= 0)
            {
                if (!m_ReadyEnterLogin)
                {
                    GameEntry.Lua.StartLuaVM();
                    GameEntry.Lua.DoFile("GameMain");

                    //ToDo 临时登录无奈之举
                    // if (!GameEntry.BuiltinData.BuildInfo.IsAutoLogion)
                    // {
                    //     GameEntry.Lua.CallMethod("GameMain", "SetNoAutoLogin");
                    // }

                    GameEntry.Lua.CallMethod("GameMain", "Main");
                    //GameEntry.Lua.SafeDoString("GameMain.OnInitOK()");
                    //GameEntry.Web.OnEnter();

                    // if (!GameEntry.Base.EditorResourceMode) GameEntry.CheckVersion.SetVersion(GameEntry.Resource.ApplicableGameVersion, GameEntry.Resource.InternalResourceVersion);
                    // GameEntry.CheckVersion.HideVersion();
                    // GameEntry.CheckVersion.HideBg();

                    m_ReadyEnterLogin = true;

                    GameEntry.Fsm.DestroyFsm<IProcedureManager>();
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_LoadFlags.Clear();
            base.OnLeave(procedureOwner, isShutdown);
            // GameEntry.Event.Unsubscribe(LoadSceneEventArgs.EventId, OnLoadScene);
            // GameEntry.Event.Unsubscribe(LoadSingleSceneEventArgs.EventId, OnLoadSingleScene);
        }

        private void OnLoadLuaScriptSuccess(string fileName, string fileText)
        {
            m_LoadFlags.Remove(fileName);
        }

        private void OnLoadLuaScriptFailure(string fileName, LoadResourceStatus status, string errorMessage)
        {
            GameFramework.GameFrameworkLog.Error(
                "Load lua script '{0}' failure. Status is '{1}'. Error message is '{2}'.", fileName,
                status, errorMessage);
        }

        private void OnLoadConfigScriptSuccess(string fileName, string fileText)
        {
            luaList json = JsonUtility.FromJson<luaList>(fileText);
            string  file = String.Empty;
            for (int i = 0; i < json.toLua.Count; i++)
            {
                file = json.toLua[i];
                m_LoadFlags.Add(file);
                GameEntry.Lua.LoadFile(ToLuaPath+file, file, OnLoadLuaScriptSuccess, OnLoadLuaScriptFailure);
            }

            for (int i = 0; i < json.Lua.Count; i++)
            {
                file = json.Lua[i];
                m_LoadFlags.Add(file);
                GameEntry.Lua.LoadFile(LuaPath+file, file, OnLoadLuaScriptSuccess, OnLoadLuaScriptFailure);
            }

            m_LoadConfigFlags = true;
        }

        private void OnLoadConfigScriptFailure(string fileName, LoadResourceStatus status, string errorMessage)
        {
            GameFramework.GameFrameworkLog.Error(
                "Load AssetLuaConfig '{0}' failure. Status is '{1}'. Error message is '{2}'.", fileName,
                status, errorMessage);
        }

        private void OnLoadScene(object sender, GameEventArgs e)
        {
            // LoadSceneEventArgs ne = (LoadSceneEventArgs)e;
            // owner.SetData<VarInt>(Constant.ProcedureData.NextSceneId1, ne.gotoSceneId);
            // ChangeState<ProcedureChangeSceneLoading>(owner);
        }

        private void OnLoadSingleScene(object sender, GameEventArgs e)
        {
            // LoadSingleSceneEventArgs ne = (LoadSingleSceneEventArgs)e;
            // owner.SetData<VarInt>(Constant.ProcedureData.NextSceneId1, ne.gotoSceneId);
            // ChangeState<ProcedureChangeSceneSingle>(owner);
        }
    }
}