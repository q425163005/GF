

namespace Fuse.Hotfix
{
    /// <summary>
    /// 热更新层游戏入口
    /// </summary>
    public static class HotfixGameEntry
    {
        public static void Start()
        {
            Log.Info("热更新层启动!");

            Mgr.Initialize();
            //开始热更新层入口流程
            Mgr.Procedure.StartProcedure<ProcedureHotfixPreload>();
        }
        
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            Mgr.Fsm.Update(elapseSeconds, realElapseSeconds);
            Mgr.Event.Update(elapseSeconds, realElapseSeconds);
        }

        public static void Shutdown()
        {
            Mgr.Dispose();
        }
    }
}

