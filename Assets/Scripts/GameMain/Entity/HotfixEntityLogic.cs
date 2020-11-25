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
        /// ʵ���ʼ����
        /// </summary>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected override void OnInit(object userData)
        {
            Name    = Name.Replace("(Clone)", string.Empty);
            ins_obj = this.gameObject;
            base.OnInit(userData);
            BaseEntityAction = (baseEntityAction) userData;
            BaseEntityAction?.OnInit?.Invoke(ins_obj, BaseEntityAction.InitUserData);
        }

        /// <summary>
        /// ʵ����ʾ��
        /// </summary>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            BaseEntityAction = (baseEntityAction) userData;
            BaseEntityAction?.OnShow?.Invoke(gameObject, BaseEntityAction.InitUserData);
        }

        /// <summary>
        /// ʵ�����ء�
        /// </summary>
        /// <param name="isShutdown">�Ƿ��ǹر�ʵ�������ʱ������</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            BaseEntityAction?.OnHide?.Invoke(isShutdown, userData);
        }

        /// <summary>
        /// ʵ����ա�
        /// </summary>
        protected override void OnRecycle()
        {
            base.OnRecycle();
            BaseEntityAction?.OnRecycle?.Invoke();
        }

        /// <summary>
        /// ʵ����ѯ��
        /// </summary>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            BaseEntityAction?.OnUpdate?.Invoke(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// ʵ�帽����ʵ�塣
        /// </summary>
        /// <param name="childEntity">���ӵ���ʵ�塣</param>
        /// <param name="parentTransform">�����Ӹ�ʵ���λ�á�</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            BaseEntityAction?.OnAttached?.Invoke(childEntity, parentTransform, userData);
        }

        /// <summary>
        /// ʵ������ʵ�塣
        /// </summary>
        /// <param name="childEntity">�������ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
            BaseEntityAction?.OnDetached?.Invoke(childEntity, userData);
        }

        /// <summary>
        /// ʵ�帽����ʵ�塣
        /// </summary>
        /// <param name="parentEntity">�����ӵĸ�ʵ�塣</param>
        /// <param name="parentTransform">�����Ӹ�ʵ���λ�á�</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            BaseEntityAction?.OnAttachTo?.Invoke(parentEntity, parentTransform, userData);
        }

        /// <summary>
        /// ʵ������ʵ�塣
        /// </summary>
        /// <param name="parentEntity">������ĸ�ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
            BaseEntityAction?.OnDetachFrom?.Invoke(parentEntity, userData);
        }
    }
}