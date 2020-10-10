namespace Fuse.Hotfix
{
    public class Mgr
    {
        /// <summary>
        /// 有限状态机
        /// </summary>
        public static FsmManager Fsm {
            get;
            private set;
        }

        /// <summary>
        /// 流程
        /// </summary>
        public static ProcedureManager Procedure {
            get;
            private set;
        }

        /// <summary>
        /// 事件
        /// </summary>
        public static EventManager Event {
            get;
            private set;
        }

        /// <summary>
        /// ET网络
        /// </summary>
        public static ETNetworkManager ETNetwork {
            get;
            private set;
        }
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
//        /// <summary>
//        /// 获取数据表组件。
//        /// </summary>
//        public static DataTableComponent DataTable {
//            get;
//            private set;
//        }
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
//        /// <summary>
//        /// 获取本地化组件。
//        /// </summary>
//        public static LocalizationComponent Localization {
//            get;
//            private set;
//        }
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
//        /// 获取界面组件。
//        /// </summary>
//        public static UIComponent UI {
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


        public static void Initialize()
        {
            Fsm       = new FsmManager();
            Procedure = new ProcedureManager();
            Event     = new EventManager();
            ETNetwork = new ETNetworkManager();

            //初始化ET网络
            Mgr.ETNetwork.Init();
            //初始化流程管理器
            //TODO:可修改为使用反射获取到所有流程然后注册
            Mgr.Procedure.Initialize(Mgr.Fsm
                                     , new ProcedureHotfixEntry()
                                     , new ProcedureChangeScene()
                                     , new ProcedureHotfixTest()
            );
        }

        public static void Dispose()
        {
            Fsm?.Shutdown();
            Procedure?.Shutdown();
            Event?.Shutdown();
            ETNetwork?.Shutdown();
        }
    }
}
