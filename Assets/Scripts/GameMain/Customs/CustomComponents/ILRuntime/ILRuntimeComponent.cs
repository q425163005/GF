﻿using GameFramework.Resource;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GameFramework.FileSystem;
using UnityEngine;
using UnityEngine.Networking;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace Fuse
{
    /// <summary>
    /// ILRuntime组件
    /// </summary>
    public class ILRuntimeComponent : GameFrameworkComponent
    {
        [SerializeField]
        private bool m_ILRuntimeMode;

        private IMethod m_Update;

        private IMethod m_Shutdown;

        private const string HotfixDLLPath = "Assets/Res/BundleRes/Data/HotFix/Hotfix.dll.bytes";
        private const string HotfixPDBPath = "Assets/Res/BundleRes/Data/HotFix/Hotfix.pdb.bytes";

        /// <summary>
        /// 是否开启ILRuntime模式
        /// </summary>
        public bool ILRuntimeMode
        {
            get
            {
                return m_ILRuntimeMode;
            }
        }

        /// <summary>
        /// ILRuntime入口对象
        /// </summary>
        public AppDomain AppDomain
        {
            get;
            private set;
        }

        private void Update()
        {
            if (m_Update == null)
            {
                return;
            }

            using (var ctx = AppDomain.BeginInvoke(m_Update))
            {
                ctx.PushFloat(Time.deltaTime);
                ctx.PushFloat(Time.unscaledDeltaTime);
                ctx.Invoke();
            }
        }

        private void OnDestroy()
        {
            if (m_Shutdown == null)
            {
                return;
            }

            AppDomain.Invoke(m_Shutdown, null, null);
        }

        /// <summary>
        /// 获取热更新层类的Type对象
        /// </summary>
        public Type GetHotfixType(string hotfixTypeFullName)
        {
            return AppDomain.LoadedTypes[hotfixTypeFullName].ReflectionType;
        }

        /// <summary>
        /// 获取所有热更新层类的Type对象
        /// </summary>
        public List<Type> GetHotfixTypes()
        {
            return AppDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
        }

        /// <summary>
        /// 加载热更新DLL
        /// </summary>
        public async void LoadHotfixDLL()
        {
            AppDomain = new AppDomain();
            ILRuntimeHelper.InitILRuntime(AppDomain);
            
            TextAsset dllAsset = await GameEntry.Resource.AwaitLoadAsset<TextAsset>(HotfixDLLPath);
            byte[]    dll      = dllAsset.bytes;

            if (Application.isEditor)
            {
                dll = await GetDll("Library/ScriptAssemblies/Fuse.Hotfix.dll");
            }
            Log.Info("hotfix dll加载完毕");

#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
            
            TextAsset pdbAsset = await GameEntry.Resource.AwaitLoadAsset<TextAsset>(HotfixPDBPath);
            byte[]    pdb      = pdbAsset.bytes;
            if (Application.isEditor)
            {
                pdb = await GetDll("Library/ScriptAssemblies/Fuse.Hotfix.pdb");
            }
            Log.Info("hotfix pdb加载完毕");

            AppDomain.LoadAssembly(new MemoryStream(dll), new MemoryStream(pdb), new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

            //启动调试服务器
            AppDomain.DebugService.StartDebugService(56000);
#else
            AppDomain.LoadAssembly(new MemoryStream(dll));
#endif
#if UNITY_EDITOR
            //设置Unity主线程ID 这样就可以用Profiler看性能消耗了
            AppDomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            StartCoroutine(HotfixStart());
        }

        private async Task<byte[]> GetDll(string dllname)
        {
            string path = Application.dataPath.Replace("Assets","") + dllname;
            return await GameEntry.WebRequest.AwaitAddWebRequest(path);
        }

        /// <summary>
        /// 开始执行热更新层代码
        /// </summary>
        private IEnumerator HotfixStart()
        {
            yield return null;
            string typeFullName = "Fuse.Hotfix.HotfixGameEntry";
            IType type = AppDomain.LoadedTypes[typeFullName];
            AppDomain.Invoke(typeFullName, "Start", null, null);

            m_Update = type.GetMethod("Update", 2);
            m_Shutdown = type.GetMethod("Shutdown", 0);
        }
        
    }

}
