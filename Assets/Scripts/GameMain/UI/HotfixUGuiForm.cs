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
    public class HotfixUGuiForm : UIFormLogic
    {
        /// <summary>
        /// 对应的热更新层实体逻辑类实例
        /// </summary>
        private object m_HotfixInstance;

        //热更新层的方法缓存
        private ILInstanceMethod m_OnOpen;
        private ILInstanceMethod m_OnClose;
        private ILInstanceMethod m_OnPause;
        private ILInstanceMethod m_OnResume;
        private ILInstanceMethod m_OnCover;
        private ILInstanceMethod m_OnReveal;
        private ILInstanceMethod m_OnRefocus;
        private IMethod          m_OnUpdate;
        private IMethod          m_OnDepthChanged;
        

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_HotfixInstance = this;
            string[] arry = UIForm.UIFormAssetName.Split('/');
            string hotfixUGuiFormFullName = $"Fuse.Hotfix.{arry[arry.Length-2]}.{arry.Last().Split('.')[0]}";

            //获取热更新层的实例
            IType  type           = GameEntry.ILRuntime.AppDomain.LoadedTypes[hotfixUGuiFormFullName];
            object hotfixInstance = ((ILType) type).Instantiate();

            //获取热更新层的方法并缓存
            m_OnOpen         = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnOpen", 1);
            m_OnClose        = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnClose", 2);
            m_OnPause        = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnPause", 0);
            m_OnResume       = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnResume", 0);
            m_OnCover        = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnCover", 0);
            m_OnReveal       = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnReveal", 0);
            m_OnRefocus      = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnRefocus", 1);
            m_OnUpdate       = type.GetMethod("OnUpdate", 2);
            m_OnDepthChanged = type.GetMethod("OnDepthChanged", 2);

            //调用热更新层的OnInit
            GameEntry.ILRuntime.AppDomain.Invoke(hotfixUGuiFormFullName, "OnInit", hotfixInstance, this, userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_OnOpen?.Invoke(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            m_OnClose?.Invoke(isShutdown, userData);
        }

        protected override void OnPause()
        {
            base.OnPause();

            m_OnPause?.Invoke();
        }

        protected override void OnResume()
        {
            base.OnResume();

            m_OnResume?.Invoke();
        }

        protected override void OnCover()
        {
            base.OnCover();

            m_OnCover?.Invoke();
        }

        protected override void OnReveal()
        {
            base.OnReveal();

            m_OnReveal?.Invoke();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);

            m_OnRefocus?.Invoke(userData);
        }
        

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            using (var ctx = GameEntry.ILRuntime.AppDomain.BeginInvoke(m_OnUpdate))
            {
                ctx.PushObject(m_HotfixInstance);
                ctx.PushFloat(elapseSeconds);
                ctx.PushFloat(realElapseSeconds);
                ctx.Invoke();
            }
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);

            using (var ctx = GameEntry.ILRuntime.AppDomain.BeginInvoke(m_OnDepthChanged))
            {
                ctx.PushObject(m_HotfixInstance);
                ctx.PushInteger(uiGroupDepth);
                ctx.PushInteger(depthInUIGroup);
                ctx.Invoke();
            }
        }
    }
}