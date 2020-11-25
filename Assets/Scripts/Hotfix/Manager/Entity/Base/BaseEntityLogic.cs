using System.Collections;
using System.Collections.Generic;
using Fuse.Tasks;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class BaseEntityLogic : BaseObject
    {
        /// <summary>�����̵�ʵ���߼��ű�</summary>
        protected Fuse.HotfixEntityLogic EntityLogic { get; private set; }

        public baseEntityAction EntityAction(object userData)
        {
            return new baseEntityAction
            {
                InitUserData = userData,
                OnInit       = OnInit,
                OnRecycle    = OnRecycle,
                OnShow       = OnShow,
                OnHide       = OnHide,
                OnAttached   = OnAttached,
                OnDetached   = OnDetached,
                OnAttachTo   = OnAttachTo,
                OnDetachFrom = OnDetachFrom,
                OnUpdate     = OnUpdate
            };
        }

        public int  entityId => EntityLogic.Entity.Id;
        public bool isShow   { get; private set; } = false;

        /// <summary>
        /// �ȴ�ʵ��������
        /// </summary>
        public virtual async CTask Await()
        {
            await CTask.WaitUntil(() => isShow && isInstance);
        }

        #region EntityLogic��������

        /// <summary>
        /// ʵ���ʼ����
        /// </summary>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected void OnInit(GameObject entityObj, object userData)
        {
//            EntityLogic = entityObj.GetComponent<HotfixEntityLogic>();
//            InitCollect(entityObj);
            Init(userData);
        }

        /// <summary>
        /// ʵ����ʾ��
        /// </summary>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected virtual void OnShow(GameObject entityObj, object userData)
        {
            EntityLogic = entityObj.GetComponent<HotfixEntityLogic>();
            InitCollect(entityObj);
            Refresh(userData);
            isShow = true;
        }

        /// <summary>
        /// ʵ����ա�
        /// </summary>
        protected virtual void OnRecycle()
        {
            Disposed();
        }

        /// <summary>
        /// ʵ�����ء�
        /// </summary>
        /// <param name="isShutdown">�Ƿ��ǹر�ʵ�������ʱ������</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected virtual void OnHide(bool isShutdown, object userData)
        {
        }

        /// <summary>
        /// ʵ�帽����ʵ�塣
        /// </summary>
        /// <param name="childEntity">���ӵ���ʵ�塣</param>
        /// <param name="parentTransform">�����Ӹ�ʵ���λ�á�</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected virtual void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
        }

        /// <summary>
        /// ʵ������ʵ�塣
        /// </summary>
        /// <param name="childEntity">�������ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected virtual void OnDetached(EntityLogic childEntity, object userData)
        {
        }

        /// <summary>
        /// ʵ�帽����ʵ�塣
        /// </summary>
        /// <param name="parentEntity">�����ӵĸ�ʵ�塣</param>
        /// <param name="parentTransform">�����Ӹ�ʵ���λ�á�</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected virtual void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
        }

        /// <summary>
        /// ʵ������ʵ�塣
        /// </summary>
        /// <param name="parentEntity">������ĸ�ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        protected virtual void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
        }

        /// <summary>
        /// ʵ����ѯ��
        /// </summary>
        /// <param name="elapseSeconds">�߼�����ʱ�䣬����Ϊ��λ��</param>
        /// <param name="realElapseSeconds">��ʵ����ʱ�䣬����Ϊ��λ��</param>
        protected virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        #endregion

        #region BaseEntityLogic

        protected virtual void Init(object userdata)
        {
        }

        protected virtual void Refresh(object userData = null)
        {
        }

        /// <summary>
        /// ʵ���ͷ�
        /// </summary>
        protected virtual void Disposed()
        {
        }

        #endregion


        public void HideSelf()
        {
            if (EntityLogic != null)
            {
                Mgr.Entity.Hide(entityId);
            }
        }
    }
}