using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
    /// <summary>
    /// 定时器管理器
    /// </summary>
    public class TimerMgr
    {
        private Queue<Timer>           cacheList  = new Queue<Timer>();
        private Dictionary<int, Timer> runList    = new Dictionary<int, Timer>();
        private List<int>              runListIds = new List<int>();

        /// <summary>
        /// 循环定时器
        /// </summary>
        /// <param name="interval">间隔时间</param>
        /// <param name="action"></param>
        /// <param name="isStartExecute">是否启动时就运行一次</param>
        /// <param name="num">运行次数 -1无次数限制</param>
        /// <returns></returns>
        public int Loop(float interval, Action action, bool isStartExecute = true, int num = -1)
        {
            Timer timer = getTimer();
            timer.Start(interval, action, num);
            if (isStartExecute)
                action();
            runList.Add(timer.UID, timer);
            runListIds.Add(timer.UID);
            return timer.UID;
        }

        /// <summary>
        /// 运行一次
        /// </summary>
        /// <param name="interval">延时时间</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int Once(float interval, Action action)
        {
            Timer timer = getTimer();
            timer.Start(interval, action, 1);
            runList.Add(timer.UID, timer);
            runListIds.Add(timer.UID);
            return timer.UID;
        }

        /// <summary>
        /// 每帧运行
        /// </summary>
        /// <param name="action">返回Time.deltaTime</param>
        /// <returns></returns>
        public int Update(Action<float> action)
        {
            Timer timer = getTimer();
            timer.StartUpdate(action);
            runList.Add(timer.UID, timer);
            runListIds.Add(timer.UID);
            return timer.UID;
        }


        /// <summary>
        /// 停止一个定时器
        /// </summary>
        /// <param name="id"></param>
        public void Stop(int id)
        {
            Timer timer;
            if (runList.TryGetValue(id, out timer))
            {
                timer.Stop();
                runList.Remove(id);
                runListIds.Remove(timer.UID);
                cacheList.Enqueue(timer);
            }
        }

        /// <summary>
        /// 停止全部定时器
        /// </summary>
        public void StopAll()
        {
            foreach (Timer timer in runList.Values)
            {
                timer.Stop();
                cacheList.Enqueue(timer);
            }

            runList.Clear();
            runListIds.Clear();
        }

        /// <summary>
        /// 获取一个定时器
        /// </summary>
        /// <returns></returns>
        private Timer getTimer()
        {
            Timer timer;
            if (cacheList.Count < 1)
                timer = new Timer();
            else
            {
                timer = cacheList.Dequeue();
            }

            return timer;
        }

        public void timerUpdateEvent(float deltaTime, float realElapseSeconds)
        {
            for (int i = 0; i < runListIds.Count; i++)
            {
                runList[runListIds[i]].Update(deltaTime, realElapseSeconds);
            }
        }


        public void Dispose()
        {
        }
    }
}