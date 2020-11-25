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
    public sealed class baseUIAction
    {
        public object                     InitUserData;
        public Action<GameObject, object> OnInit;
        public Action<GameObject, object> OnOpen;
        public Action<bool, object>       OnClose;
        public Action                     OnPause;
        public Action                     OnResume;
        public Action                     OnCover;
        public Action                     OnReveal;
        public Action<object>             OnRefocus;
        public Action<float, float>       OnUpdate;
        public Action<int, int>           OnDepthChanged;
        public Action                     OnDestroy;
    }

    [RequireComponent(typeof(CompCollector))]
    public class HotfixUGuiForm : UIFormLogic
    {
        private baseUIAction BaseUiAction;

        protected override void OnInit(object userData)
        {
            RectTransform rectTransform = transform.GetComponent<RectTransform>();
            rectTransform.anchorMin        = Vector2.zero;
            rectTransform.anchorMax        = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta        = Vector2.zero;

            Name = Name.Replace("(Clone)", string.Empty);
            gameObject.SetLayerRecursively(5);

            base.OnInit(userData);
            BaseUiAction = (baseUIAction) userData;
            BaseUiAction?.OnInit?.Invoke(gameObject, BaseUiAction.InitUserData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            BaseUiAction = (baseUIAction) userData;
            BaseUiAction?.OnOpen?.Invoke(gameObject, BaseUiAction.InitUserData);
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

        private void OnDestroy()
        {
            BaseUiAction?.OnDestroy?.Invoke();
        }
    }
}