using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fuse;
using Fuse.Hotfix.Manager;

namespace Fuse.Hotfix
{
    /// <summary>
    /// 热更新层UGUI界面
    /// </summary>
    public class BaseUI
    {
        /// <summary>
        /// 主工程的界面逻辑脚本
        /// </summary>
        protected Fuse.HotfixUGuiForm UIFormLogic { get; private set; }

        public    EUIGroup                       UIGroup    = EUIGroup.Default;
        public    int                            SerialId   = -99;
        protected bool                           isInstance = false;
        protected object                         AwakeUserData;
        protected Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void OnInit(Fuse.HotfixUGuiForm uiFormLogic, object userdata)
        {
            UIFormLogic      = uiFormLogic;
            SerialId         = UIFormLogic.UIForm.SerialId;
            UIFormLogic.Name = UIFormLogic.Name.Replace("(Clone)", "");
            Mgr.UI.SetUIBase(this, UIFormLogic.Name);
            AwakeUserData = userdata;
            CompCollector collector = uiFormLogic.gameObject.GetComponent<CompCollector>();
            foreach (var variable in collector.CompCollectorInfos)
                objectList.Add(variable.Name, variable.Object as GameObject);
            isInstance = true;
            InitializeComponent();
            Awake();
        }

        /// <summary>初始化UI控件</summary>
        protected virtual void InitializeComponent()
        {
        }

        protected virtual void Awake()
        {
        }

        /// <summary>
        /// 界面打开
        /// </summary>
        public virtual void OnOpen(object userdata)
        {
        }

        /// <summary>
        /// 界面关闭
        /// </summary>
        protected void OnClose(bool isShutdown, object userdata)
        {
            Close(isShutdown, userdata);
            AwakeUserData = null;
            objectList    = null;
        }

        /// <summary>
        /// 界面关闭
        /// </summary>
        protected virtual void Close(bool isShutdown, object userdata)
        {
        }

        /// <summary>关闭当前UI</summary>
        public virtual void CloseSelf()
        {
            Mgr.UI.CloseForName(GetType().Name);
        }

        /// <summary>
        /// 界面暂停
        /// </summary>
        public virtual void OnPause()
        {
        }

        /// <summary>
        /// 界面暂停恢复
        /// </summary>
        public virtual void OnResume()
        {
        }

        /// <summary>
        /// 界面遮挡
        /// </summary>
        public virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复
        /// </summary>
        public virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活
        /// </summary>
        public virtual void OnRefocus(object userData)
        {
        }

        /// <summary>
        /// 界面轮询
        /// </summary>
        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 界面深度改变
        /// </summary>
        public virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
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