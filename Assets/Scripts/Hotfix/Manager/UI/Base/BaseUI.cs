using System.Collections.Generic;
using UnityEngine;
using Fuse.Tasks;

namespace Fuse.Hotfix
{
    /// <summary>
    /// 热更新层UGUI界面
    /// </summary>
    public class BaseUI : BaseObject
    {
        /// <summary>主工程的界面逻辑脚本</summary>
        protected Fuse.HotfixUGuiForm UIFormLogic { get; private set; }

        public EUIGroup UIGroup = EUIGroup.Default;
        public bool     realeaseLock { get; protected set; } = false;

        public int  SerialId { get; private set; } = -99;
        public bool isOpen   { get; private set; } = false;

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
                OnDepthChanged = OnDepthChanged,
                OnDestroy      = OnDestroy
            };
        }

        /// <summary>
        /// 等待界面加载完成
        /// </summary>
        public virtual async CTask Await()
        {
            await CTask.WaitUntil(() => isInstance && isOpen);
        }

        #region UILogic生命周期

        /// <summary>界面初始化</summary>
        private void OnInit(GameObject uiFormObj, object userData)
        {
            UIFormLogic = uiFormObj.GetComponent<HotfixUGuiForm>();
            Mgr.UI.SetUIInstanceLocked(UIFormLogic.UIForm, realeaseLock);
            InitCollect(UIFormLogic.gameObject);
            Init(userData);
        }

        /// <summary>界面打开</summary>
        private void OnOpen(GameObject uiFormObj, object userData)
        {
            SerialId = UIFormLogic.UIForm.SerialId;
            isOpen   = true;
            Refresh(userData);
        }

        /// <summary>界面关闭</summary>
        private void OnClose(bool isShutdown, object userData)
        {
            isOpen = false;
            Disposed();
        }

        /// <summary>界面销毁</summary>
        private void OnDestroy()
        {
            m_isDispose = true;
            Disposed();
            objectList.Clear();
            Mgr.UI.DeatroyUI(GetType().Name);
            UIFormLogic = null;
        }

        /// <summary>界面暂停</summary>
        protected virtual void OnPause()
        {
        }

        /// <summary>界面暂停恢复</summary>
        protected virtual void OnResume()
        {
        }

        /// <summary>界面遮挡</summary>
        protected virtual void OnCover()
        {
        }

        /// <summary>界面遮挡恢复</summary>
        protected virtual void OnReveal()
        {
        }

        /// <summary>界面激活</summary>
        protected virtual void OnRefocus(object userData)
        {
        }

        /// <summary>界面轮询</summary>
        protected virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>界面深度改变</summary>
        protected virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
        }

        #endregion

        #region BaseUI

        /// <summary>界面初始化</summary>
        protected virtual void Init(object userdata)
        {
        }

        /// <summary>界面刷新</summary>
        protected virtual void Refresh(object userdata = null)
        {
        }

        /// <summary>界面释放</summary>
        protected virtual void Disposed()
        {
        }

        #endregion

        /// <summary>关闭当前UI</summary>
        protected void CloseSelf()
        {
            Mgr.UI.CloseForName(GetType().Name);
        }
    }
}