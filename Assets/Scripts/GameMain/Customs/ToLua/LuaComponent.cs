using GameFramework;
using GameFramework.Resource;
using LuaInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace Fuse
{
    public class AsseListConfig
    {
        public List<string> list = new List<string>();
    }

    /// <summary>
    /// Lua组件
    /// </summary>
    public class LuaComponent : GameFrameworkComponent
    {
        [SerializeField, Tooltip("Lua script search paths relative to 'Assets/' for editor use.")]
        private string[] m_EditorSearchPaths = null;

        public const string LuaAssetExtInBundle = ".bytes";
        public const string LuaConfigExtInBundle = ".txt";

        public delegate void OnLoadScriptSuccess(string fileName, string fileText);

        public delegate void OnLoadScriptFailure(string fileName, LoadResourceStatus status, string errorMessage);

        private LuaLooper m_LuaLooper = null;
        private CustomLuaLoader m_LuaLoader = null;
        private Dictionary<string, byte[]> m_CachedLuaScripts = new Dictionary<string, byte[]>();
        
        public LuaState LuaState
        {
            get { return m_LuaState; }
        }

        private LuaState m_LuaState = null;

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Deinit();
        }

        /// <summary>
        /// 启动 Lua 虚拟机。
        /// </summary>
        public void StartLuaVM()
        {
            AddSearchPaths();
            m_LuaState.Start();
            StartLooper();
        }

        /// <summary>
        /// 清理 Lua 虚拟机。
        /// </summary>
        /// <remarks>重启游戏时调用。</remarks>
        public void ClearLuaVM()
        {
            Debugger.Log("GameEntry.Lua.CallMethod");
            CallMethod("GameMain", "OnDestroy");
            Deinit();
        }

        /// <summary>
        /// 加载 Lua 脚本文件。
        /// </summary>
        /// <param name="assetPath">Lua 脚本的资源路径。</param>
        /// <param name="fileName">Lua 脚本文件名。</param>
        /// <param name="onSuccess">加载成功回调。</param>
        /// <param name="onFailure">加载失败回调。</param>
        public void LoadFile(string assetPath, string fileName, OnLoadScriptSuccess onSuccess, OnLoadScriptFailure onFailure,
            bool ext = true)
        {
            if (m_CachedLuaScripts.ContainsKey(fileName))
            {
                if (onSuccess != null)
                {
                    onSuccess(fileName, string.Empty);
                }

                return;
            }

            // Load lua script from AssetBundle.
            var innerCallbacks = new LoadAssetCallbacks(loadAssetSuccessCallback: OnLoadAssetSuccess,
                loadAssetFailureCallback: OnLoadAssetFailure);
            var userData = new LoadLuaScriptUserData { FileName = fileName, OnSuccess = onSuccess, OnFailure = onFailure };

            if (ext) assetPath += LuaAssetExtInBundle;
            GameEntry.Resource.LoadAsset(assetPath, innerCallbacks, userData);
        }

        private void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            var myUserData = userData as LoadLuaScriptUserData;
            TextAsset textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                throw new GameFramework.GameFrameworkException("The loaded asset should be a text asset.");
            }

            if (!m_CachedLuaScripts.ContainsKey(myUserData.FileName))
            {
                m_CachedLuaScripts.Add(myUserData.FileName, textAsset.bytes);
            }

            if (myUserData.OnSuccess != null)
            {
                myUserData.OnSuccess(myUserData.FileName, textAsset.text);
            }
            GameEntry.Resource.UnloadAsset(asset);
            //ResExtension.UnloadAsset(asset);
        }

        private void OnLoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            var myUserData = userData as LoadLuaScriptUserData;
            if (myUserData == null) return;

            if (myUserData.OnFailure != null)
            {
                myUserData.OnFailure(myUserData.FileName, status, errorMessage);
            }
        }

        /// <summary>
        /// 卸载 Lua 脚本文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        public void UnloadFile(string fileName)
        {
            if (Application.isEditor && GameEntry.Base.EditorResourceMode)
            {
                m_CachedLuaScripts.Remove(fileName);
            }
        }

        /// <summary>
        /// 执行 Lua 脚本字符串。
        /// </summary>
        /// <param name="chunk">代码块。</param>
        /// <param name="chunkName">代码块名称。</param>
        /// <returns>返回值列表。</returns>
        public void DoString(string chunk, string chunkName = "")
        {
            if (string.IsNullOrEmpty(chunkName))
            {
                m_LuaState.DoString(chunk);
            }

            m_LuaState.DoString(chunk, chunkName);
        }


        /// <summary>
        /// 执行 Lua 脚本文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns>返回值列表。</returns>
        public void DoFile(string fileName)
        {
            m_LuaState.DoFile(fileName);
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public object[] CallMethod(string module, string func, params object[] args)
        {
            return CallFunction(module + "." + func, args);
        }

        public object[] CallFunction(string funcName, params object[] args)
        {
            if (m_LuaState == null)
            {
                return null;
            }

            LuaFunction func = m_LuaState.GetFunction(funcName);
            if (func != null)
            {
                return func.LazyCall(args);
            }

            return null;
        }


        public void SafeDoString(string scriptContent)
        {
            if (m_LuaState != null)
            {
                try
                {
                    m_LuaState.DoString(scriptContent);
                }
                catch (System.Exception ex)
                {
                    string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                    Log.Error(msg);
                }
            }
        }


        public T SafeDoString<T>(string chunk, string chunkName = "")
        {
            try
            {
                if (string.IsNullOrEmpty(chunkName))
                {
                    return m_LuaState.DoString<T>(chunk);
                }
                return m_LuaState.DoString<T>(chunk);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Log.Error(msg);
                return default(T);
            }
        }

        public void SafeCall<T1>(LuaTable table, string name, T1 arg1)
        {
            try
            {
                table.Call(name, arg1);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Log.Error(msg);
            }
        }

        public void SafeCall<T1, T2>(LuaTable table, string name, T1 arg1, T2 arg2)
        {
            try
            {
                table.Call(name, arg1, arg2);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Log.Error(msg);
            }
        }

        public void SafeCall<T1, T2, T3>(LuaTable table, string name, T1 arg1, T2 arg2, T3 arg3)
        {
            try
            {
                table.Call(name, arg1, arg2, arg3);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Log.Error(msg);
            }
        }

        public void SafeCall<T1, T2, T3, T4>(LuaTable table, string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            try
            {
                table.Call(name, arg1, arg2, arg3, arg4);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Log.Error(msg);
            }
        }

        public void SafeCall<T1, T2, T3, T4, T5>(LuaTable table, string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            try
            {
                table.Call(name, arg1, arg2, arg3, arg4, arg5);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Log.Error(msg);
            }
        }


        public void SafeCall<T1, T2, T3, T4, T5, T6>(LuaTable table, string name, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            try
            {
                table.Call(name, arg1, arg2, arg3, arg4, arg5, arg6);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Lua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Log.Error(msg);
            }
        }

        public void PrintLuaTraceback()
        {
            LuaState.DoString("print(debug.traceback())");
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////// 
        //////////////////////////////////////////////////////////////////////////////////////////////
        private void Init()
        {
            m_LuaLoader = new CustomLuaLoader(GetScriptContent);
            m_LuaState = new LuaState();
            OpenLibs();
            m_LuaState.LuaSetTop(0);

            LuaBinder.Bind(m_LuaState);
            DelegateFactory.Init();
            LuaCoroutine.Register(m_LuaState, this);
        }

        private void Deinit()
        {
            m_CachedLuaScripts.Clear();
            if (m_LuaLooper != null)
            {
                m_LuaLooper.Destroy();
                m_LuaLooper = null;
            }

            if (m_LuaState != null)
            {
                m_LuaState.Dispose();
                m_LuaState = null;
            }

            m_LuaLoader = null;
        }

        private void OpenLibs()
        {

            m_LuaState.OpenLibs(LuaDLL.luaopen_pb);
            m_LuaState.LuaSetField(-2, "pb");


            // m_LuaState.OpenLibs(LuaDLL.luaopen_pb_io);
            // m_LuaState.LuaSetField(-2, "pb.io");
            // m_LuaState.OpenLibs(LuaDLL.luaopen_pb_conv);
            // m_LuaState.LuaSetField(-2, "pb.conv");
            // m_LuaState.OpenLibs(LuaDLL.luaopen_pb_buffer);
            // m_LuaState.LuaSetField(-2, "pb.buffer");
            // m_LuaState.OpenLibs(LuaDLL.luaopen_pb_slice);
            // m_LuaState.LuaSetField(-2, "pb.slice");

            m_LuaState.OpenLibs(LuaDLL.luaopen_bit);
            //m_LuaState.OpenLibs(LuaDLL.luaopen_protobuf_c);

            // close
            //m_LuaState.OpenLibs(LuaDLL.luaopen_pb);   
            //m_LuaState.OpenLibs(LuaDLL.luaopen_sproto_core);
            //m_LuaState.OpenLibs(LuaDLL.luaopen_lpeg);

            //m_LuaState.OpenLibs(LuaDLL.luaopen_socket_core);
            this.OpenCJson();
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            m_LuaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            m_LuaState.OpenLibs(LuaDLL.luaopen_cjson);
            m_LuaState.LuaSetField(-2, "cjson");

            m_LuaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
            m_LuaState.LuaSetField(-2, "cjson.safe");
        }


        private void StartLooper()
        {
            m_LuaLooper = gameObject.AddComponent<LuaLooper>();
            m_LuaLooper.luaState = m_LuaState;
        }

        private void AddSearchPaths()
        {
            if (Application.isEditor && GameEntry.Base.EditorResourceMode)
            {
                for (int i = 0; i < m_EditorSearchPaths.Length; ++i)
                {
                    m_LuaState.AddSearchPath(Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, m_EditorSearchPaths[i])));
                }
            }
        }

        private bool GetScriptContent(string fileName, out byte[] buffer)
        {
            return m_CachedLuaScripts.TryGetValue(fileName, out buffer);
        }

        private void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;

            m_LuaState.BeginPreLoad();
            m_LuaState.RegFunction("socket.core", LuaOpen_Socket_Core);
            m_LuaState.RegFunction("mime.core", LuaOpen_Mime_Core);
            m_LuaState.EndPreLoad();
        }

        [MonoPInvokeCallback(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallback(typeof(LuaCSFunction))]
        static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }


        private class LoadLuaScriptUserData
        {
            public string FileName;
            public OnLoadScriptSuccess OnSuccess;
            public OnLoadScriptFailure OnFailure;
        }

        private void LuaGC()
        {
            m_LuaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public void ClearMemory()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
            LuaGC();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            CallMethod("GameMain", "OnApplicationFocus", hasFocus);
        }

        private void OnApplicationPause(bool pause)
        {
            CallMethod("GameMain", "OnApplicationPause", pause);
        }

        private void OnApplicationQuit()
        {
            CallMethod("GameMain", "OnApplicationQuit");
        }

        private Dictionary<Object, Dictionary<string, LuaFunction>> objectToLuaFunctionDictionary =
            new Dictionary<Object, Dictionary<string, LuaFunction>>();

        List<LuaFunction> needDisposeLuaFunctionList = new List<LuaFunction>();

        private void Update()
        {
            if (needDisposeLuaFunctionList.Count > 0)
            {
                for (int i = 0; i < needDisposeLuaFunctionList.Count; i++)
                {
                    needDisposeLuaFunctionList[i].Dispose();
                }

                needDisposeLuaFunctionList.Clear();
            }

            if (needGC)
            {
                GameEntry.Lua.ClearMemory();
                GC.Collect();
                needGC = false;
            }
        }

        private bool needGC;

        public void TryFormGC()
        {
            needGC = true;
        }
/*
        public void AssociateObjectToLuaFunction(Object o, string subTypeName, LuaFunction luaFunction)
        {
            Dictionary<string, LuaFunction> subTypeLuaFunctionDic;
            if (objectToLuaFunctionDictionary.TryGetValue(o, out subTypeLuaFunctionDic))
            {
                if (subTypeLuaFunctionDic.ContainsKey(subTypeName))
                {
                    string hintString = "";
                    if (o is GameObject)
                    {
                        hintString = ((GameObject)o).transform.GetTransformHierarchyPath();
                    }
                    else if (o is Component)
                    {
                        hintString = ((Component)o).transform.GetTransformHierarchyPath();
                    }
                    else
                    {
                        hintString = o.name;
                    }

                    Log.Error(
                        "AssociateObjectToLuaFunction 对于一个object的一种sub type name,只能关联一个luafunction,可能是上一次注册的事件还没有释放导致了重复注册." +
                        hintString);

                    return;
                }

                subTypeLuaFunctionDic.Add(subTypeName, luaFunction);
            }
            else
            {
                subTypeLuaFunctionDic = new Dictionary<string, LuaFunction> { { subTypeName, luaFunction } };
                objectToLuaFunctionDictionary.Add(o, subTypeLuaFunctionDic);
            }
        }
        */

        public void DisposeObjectAssociatedLuaFunctions(Object o, string subTypeName)
        {
            Dictionary<string, LuaFunction> subTypeLuaFunctionDic;
            if (objectToLuaFunctionDictionary.TryGetValue(o, out subTypeLuaFunctionDic))
            {
                if (subTypeLuaFunctionDic.ContainsKey(subTypeName))
                {
                    needDisposeLuaFunctionList.Add(subTypeLuaFunctionDic[subTypeName]);
                    subTypeLuaFunctionDic.Remove(subTypeName);
                }
            }
        }

        public void DisposeObjectAssociatedLuaFunctions(Object o)
        {
            Dictionary<string, LuaFunction> subTypeLuaFunctionDic;
            if (objectToLuaFunctionDictionary.TryGetValue(o, out subTypeLuaFunctionDic))
            {
                foreach (var kv in subTypeLuaFunctionDic)
                {
                    needDisposeLuaFunctionList.Add(kv.Value);
                }

                objectToLuaFunctionDictionary.Remove(o);
            }
        }
    }
}