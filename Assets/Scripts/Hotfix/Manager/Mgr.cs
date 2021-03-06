﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fuse.Hotfix
{
    public class Mgr
    {
        /// <summary>
        /// 有限状态机
        /// </summary>
        public static FsmManager Fsm { get; private set; }

        /// <summary>
        /// 流程
        /// </summary>
        public static ProcedureManager Procedure { get; private set; }

        /// <summary>
        /// 事件
        /// </summary>
        public static EventManager Event { get; private set; }

        /// <summary>
        /// ET网络
        /// </summary>
        public static ETNetworkManager ETNetwork { get; private set; }
        
        /// <summary>
        /// 定时器管理器
        /// </summary>
        public static TimerMgr Timer { get; private set; }


        /// <summary>
        /// 界面
        /// </summary>
        public static UIMgr UI { get; private set; }

        /// <summary>
        /// 资源
        /// </summary>
        public static ResourceMgr Res { get; private set; }

        /// <summary>
        /// Config
        /// </summary>
        public static ConfigMgr Config { get; private set; }

        /// <summary>
        /// 获取本地化组件。
        /// </summary>
        public static LangMgr Lang { get; private set; }

        /// <summary>
        /// 获取实体组件。
        /// </summary>
        public static EntityMgr Entity { get; private set; }


        //
        //        /// <summary>
        //        /// 获取游戏基础组件。
        //        /// </summary>
        //        public static BaseComponent Base {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取配置组件。
        //        /// </summary>
        //        public static ConfigComponent Config {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取数据结点组件。
        //        /// </summary>
        //        public static DataNodeComponent DataNode {
        //            get;
        //            private set;
        //        }
        //

        //
        //        /// <summary>
        //        /// 获取调试组件。
        //        /// </summary>
        //        public static DebuggerComponent Debugger {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取下载组件。
        //        /// </summary>
        //        public static DownloadComponent Download {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取实体组件。
        //        /// </summary>
        //        public static EntityComponent Entity {
        //            get;
        //            private set;
        //        }
        //        
        //        /// <summary>
        //        /// 获取文件系统组件。
        //        /// </summary>
        //        public static FileSystemComponent FileSystem {
        //            get;
        //            private set;
        //        }
        //        

        //
        //        /// <summary>
        //        /// 获取网络组件。
        //        /// </summary>
        //        public static NetworkComponent Network {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取对象池组件。
        //        /// </summary>
        //        public static ObjectPoolComponent ObjectPool {
        //            get;
        //            private set;
        //        }
        //        
        //        /// <summary>
        //        /// 获取资源组件。
        //        /// </summary>
        //        public static ResourceComponent Resource {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取场景组件。
        //        /// </summary>
        //        public static SceneComponent Scene {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取配置组件。
        //        /// </summary>
        //        public static SettingComponent Setting {
        //            get;
        //            private set;
        //        }
        //
        //        /// <summary>
        //        /// 获取声音组件。
        //        /// </summary>
        //        public static SoundComponent Sound {
        //            get;
        //            private set;
        //        }

        //
        //        /// <summary>
        //        /// 获取网络组件。
        //        /// </summary>
        //        public static WebRequestComponent WebRequest {
        //            get;
        //            private set;
        //        }

        /// <summary>缓存的数据管理器,用来统一清空缓存数据用</summary>
        public static List<IDisposable> __dataMgrList = new List<IDisposable>();

        public static void Initialize()
        {
            Fsm       = new FsmManager();
            Procedure = new ProcedureManager();
            Event     = new EventManager();
            ETNetwork = new ETNetworkManager();
            Timer     = new TimerMgr();

            UI     = new UIMgr(GameEntry.UI);
            Res    = new ResourceMgr(GameEntry.Resource);
            Config = new ConfigMgr();
            Lang   = new LangMgr(GameEntry.Localization);

            Entity=new EntityMgr(GameEntry.Entity);


            //初始化ET网络
            ETNetwork.Init();

            //初始化流程管理器
            ProcedureBase[] procedureArry = GetAllProcedure(out var nameList);
            Procedure.SetAllProcedureName(nameList);
            Procedure.Initialize(Fsm, procedureArry);
        }

        //反射获取到所有流程
        private static ProcedureBase[] GetAllProcedure(out List<string> nameList)
        {
            nameList = new List<string>();
            Assembly assembly = Assembly.Load("Fuse.Hotfix");
            var      types    = assembly.GetTypes();
            foreach (var type in types)
            {
                var baseType = type.BaseType; //获取基类
                while (baseType != null)      //获取所有基类
                {
                    if (baseType.Name == "ProcedureBase")
                    {
                        nameList.Add(type.Name);
                        break;
                    }
                    else
                    {
                        baseType = baseType.BaseType;
                    }
                }
            }

            List<ProcedureBase> allProcedure = new List<ProcedureBase>();
            foreach (var variable in nameList)
            {
                Type type = Type.GetType("Fuse.Hotfix." + variable);
                try
                {
                    allProcedure.Add(Activator.CreateInstance(type) as ProcedureBase);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message + ex.StackTrace);
                }
            }

            return allProcedure.ToArray();
        }

        public static void Dispose()
        {
            Fsm?.Shutdown();
            Procedure?.Shutdown();
            Event?.Shutdown();
            ETNetwork?.Shutdown();
            Timer?.StopAll();

            UI?.Shutdown();
            Config?.Shutdown();

            for (int i = 0; i < __dataMgrList.Count; i++) __dataMgrList[i].Dispose();
        }
    }
}