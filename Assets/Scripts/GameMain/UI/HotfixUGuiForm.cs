using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;


namespace Fuse
{
    public class baseUIAction
    {
        public object                 InitUserData;
        public Action<HotfixUGuiForm, object> OnInit;
        public Action<object>         OnOpen;
        public Action<bool, object>   OnClose;
        public Action                 OnPause;
        public Action                 OnResume;
        public Action                 OnCover;
        public Action                 OnReveal;
        public Action<object>         OnRefocus;
        public Action<float, float>   OnUpdate;
        public Action<int, int>       OnDepthChanged;
    }

    public class HotfixUGuiForm : UIFormLogic
    {
        private baseUIAction BaseUiAction;

        protected override void OnInit(object userData)
        {
            Name = Name.Replace("(Clone)", string.Empty);
            base.OnInit(userData);
            BaseUiAction = (baseUIAction) userData;
            BaseUiAction?.OnInit?.Invoke(this, BaseUiAction.InitUserData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            BaseUiAction?.OnOpen?.Invoke(BaseUiAction.InitUserData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            BaseUiAction?.OnClose?.Invoke(isShutdown, userData);
        }

        protected override void OnPause()
        {
            base.OnPause();
            BaseUiAction?.OnPause?.Invoke();
        }

        protected override void OnResume()
        {
            base.OnResume();
            BaseUiAction?.OnResume?.Invoke();
        }

        protected override void OnCover()
        {
            base.OnCover();
            BaseUiAction?.OnCover?.Invoke();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
            BaseUiAction?.OnReveal?.Invoke();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
            BaseUiAction?.OnRefocus?.Invoke(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            BaseUiAction?.OnUpdate?.Invoke(elapseSeconds, realElapseSeconds);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            BaseUiAction?.OnDepthChanged?.Invoke(uiGroupDepth, depthInUIGroup);
        }
    }
}