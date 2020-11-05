using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fuse;
using Fuse.Hotfix.Manager;
using Fuse.Tasks;

namespace Fuse.Hotfix
{
    /// <summary>
    /// 热更新层UGUI界面
    /// </summary>
    public class BaseUI
    {
        /// <summary>主工程的界面逻辑脚本</summary>
        protected Fuse.HotfixUGuiForm UIFormLogic { get; private set; }

        /// <summary>组件列表</summary>
        protected Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();

        public EUIGroup UIGroup  = EUIGroup.Default;
        public int      SerialId = -99;
        public bool     isInstance { get; private set; }

        public baseUIAction UiAction(object userData)
        {
            return new baseUIAction
            {
                InitUserData   = userData,
                OnInit         = OnInit,
                OnOpen         = OnOpen,
                OnClose        = OnClose,
                OnPause        = OnPause,
                OnResume       = OnResume,
                OnCover        = OnCover,
                OnReveal       = OnReveal,
                OnRefocus      = OnRefocus,
                OnUpdate       = OnUpdate,
                OnDepthChanged = OnDepthChanged
            };
        }
        
        /// <summary>
        /// 界面初始化
        /// </summary>
        public void OnInit(HotfixUGuiForm uiFormLogic, object userdata)
        {
            UIFormLogic = uiFormLogic;
            foreach (var variable in uiFormLogic.gameObject.GetComponent<CompCollector>().CompCollectorInfos)
                objectList.Add(variable.Name, variable.Object as GameObject);
            InitializeComponent();
            isInstance = true;
            Awake(userdata);
        }

        /// <summary>初始化UI控件</summary>
        protected virtual void InitializeComponent()
        {
        }

        protected virtual void Awake(object userdata)
        {
        }

        /// <summary>
        /// 界面打开
        /// </summary>
        protected virtual void OnOpen(object userdata)
        {
        }

        /// <summary>
        /// 界面关闭
        /// </summary>
        protected virtual void OnClose(bool isShutdown, object userdata)
        {
            objectList    = null;
        }

        /// <summary>关闭当前UI</summary>
        protected void CloseSelf()
        {
            Mgr.UI.CloseForName(GetType().Name);
        }

        /// <summary>
        /// 界面暂停
        /// </summary>
        protected virtual void OnPause()
        {
        }

        /// <summary>
        /// 界面暂停恢复
        /// </summary>
        protected virtual void OnResume()
        {
        }

        /// <summary>
        /// 界面遮挡
        /// </summary>
        protected virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复
        /// </summary>
        protected virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活
        /// </summary>
        protected virtual void OnRefocus(object userData)
        {
        }

        /// <summary>
        /// 界面轮询
        /// </summary>
        protected virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 界面深度改变
        /// </summary>
        protected virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
        }

        /// <summary>
        /// 等待界面加载完成
        /// </summary>
        public virtual async CTask Await()
        {
            await CTask.WaitUntil(() => UIFormLogic!=null && UIFormLogic.gameObject != null);
        }

        /// <summary>
        /// 获取控件引用对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">控件名</param>
        /// <returns></returns>
        protected T Get<T>(string name) where T : Component
        {
            GameObject obj = Get(name);
            if (obj == null)
                return null;
            return obj.GetComponent<T>();
        }

        /// <summary>
        /// 获取引用对象
        /// </summary>
        protected GameObject Get(string name)
        {
            GameObject obj;
            if (!objectList.TryGetValue(name, out obj))
            {
                Log.Error($"未找到GameObject对象,请在CompCollector中设置:{name}");
            }

            return obj;
        }
    }
}