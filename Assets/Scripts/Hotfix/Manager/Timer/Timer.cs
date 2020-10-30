using System;

namespace Fuse.Hotfix
{
    public class Timer
    {
        private static readonly object obj          = new object();
        static                  int    _identityUID = 0;

        public  float         m_interval;
        public  Action        m_action;
        public  Action<float> m_actionUpdate;
        public  int           m_num;
        public  int           UID              = 0;
        private bool          isStart          = false;
        private bool          isUpdateInterval = false;
        private float         m_TimeTotal      = 0;


        public Timer()
        {
        }

        public void Start(float interval, Action action, int num = -1)
        {
            lock (obj)
                UID = ++_identityUID;
            m_interval       = interval;
            m_action         = action;
            m_num            = num;
            isStart          = true;
            isUpdateInterval = false;
        }

        public void StartUpdate(Action<float> action)
        {
            lock (obj)
                UID = ++_identityUID;
            isStart          = true;
            m_actionUpdate   = action;
            isUpdateInterval = true;
        }

        public void Stop()
        {
            isStart     = false;
            m_TimeTotal = 0;
        }

        //管理器进行调用
        public void Update(float deltaTime, float realElapseSeconds)
        {
            if (!isStart)
                return;

            if (isUpdateInterval)
            {
                m_actionUpdate?.Invoke(deltaTime);
            }
            else
            {
                m_TimeTotal += deltaTime;
                if (m_TimeTotal > m_interval)
                {
                    m_TimeTotal -= m_interval;
                    m_action?.Invoke();
                    if (m_num > 0)
                    {
                        m_num -= 1;
                        if (m_num == 0)
                        {
                            Mgr.Timer.Stop(UID);
                            return;
                        }
                    }
                }
            }
        }
    }
}