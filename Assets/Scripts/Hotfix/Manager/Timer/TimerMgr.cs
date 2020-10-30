using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
    /// <summary>
    /// ��ʱ��������
    /// </summary>
    public class TimerMgr
    {
        private Queue<Timer>           cacheList  = new Queue<Timer>();
        private Dictionary<int, Timer> runList    = new Dictionary<int, Timer>();
        private List<int>              runListIds = new List<int>();

        /// <summary>
        /// ѭ����ʱ��
        /// </summary>
        /// <param name="interval">���ʱ��</param>
        /// <param name="action"></param>
        /// <param name="isStartExecute">�Ƿ�����ʱ������һ��</param>
        /// <param name="num">���д��� -1�޴�������</param>
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
        /// ����һ��
        /// </summary>
        /// <param name="interval">��ʱʱ��</param>
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
        /// ÿ֡����
        /// </summary>
        /// <param name="action">����Time.deltaTime</param>
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
        /// ֹͣһ����ʱ��
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
        /// ֹͣȫ����ʱ��
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
        /// ��ȡһ����ʱ��
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