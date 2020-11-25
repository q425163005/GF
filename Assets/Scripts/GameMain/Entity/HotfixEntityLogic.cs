using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse
{
    public sealed class baseEntityAction
    {
        public object                                 HotfixObj;
        public object                                 InitUserData;
        public Action<GameObject, object>             OnInit;
        public Action<GameObject, object>             OnShow;
        public Action<bool, object>                   OnHide;
        public Action                                 OnRecycle;
        public Action<float, float>                   OnUpdate;
        public Action<EntityLogic, Transform, object> OnAttached;
        public Action<EntityLogic, object>            OnDetached;
        public Action<EntityLogic, Transform, object> OnAttachTo;
        public Action<EntityLogic, object>            OnDetachFrom;
    }

    public class HotfixEntityLogic : EntityLogic
    {
        [HideInInspector] public baseEntityAction BaseEntityAction;
        [HideInInspector] public GameObject       ins_obj;

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnInit(object userData)
        {
            Name    = Name.Replace("(Clone)", string.Empty);
            ins_obj = this.gameObject;
            base.OnInit(userData);
            BaseEntityAction = (baseEntityAction) userData;
            BaseEntityAction?.OnInit?.Invoke(ins_obj, BaseEntityAction.InitUserData);
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            BaseEntityAction = (baseEntityAction) userData;
            BaseEntityAction?.OnShow?.Invoke(gameObject, BaseEntityAction.InitUserData);
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            BaseEntityAction?.OnHide?.Invoke(isShutdown, userData);
        }

        /// <summary>
        /// 实体回收。
        /// </summary>
        protected override void OnRecycle()
        {
            base.OnRecycle();
            BaseEntityAction?.OnRecycle?.Invoke();
        }

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            BaseEntityAction?.OnUpdate?.Invoke(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            BaseEntityAction?.OnAttached?.Invoke(childEntity, parentTransform, userData);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
            BaseEntityAction?.OnDetached?.Invoke(childEntity, userData);
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            BaseEntityAction?.OnAttachTo?.Invoke(parentEntity, parentTransform, userData);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
            BaseEntityAction?.OnDetachFrom?.Invoke(parentEntity, userData);
        }
    }
}