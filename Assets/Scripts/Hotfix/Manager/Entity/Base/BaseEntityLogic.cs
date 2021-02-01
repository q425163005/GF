using System.Collections;
using System.Collections.Generic;
using Fuse.Tasks;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class BaseEntityLogic : BaseObject
    {
        /// <summary>主工程的实体逻辑脚本</summary>
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
        /// 等待实体加载完成
        /// </summary>
        public virtual async CTask Await()
        {
            await CTask.WaitUntil(() => isShow && isInstance);
        }

        #region EntityLogic生命周期

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected void OnInit(GameObject entityObj, object userData)
        {
//            EntityLogic = entityObj.GetComponent<HotfixEntityLogic>();
//            InitCollect(entityObj);
            Init(userData);
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected virtual void OnShow(GameObject entityObj, object userData)
        {
            EntityLogic = entityObj.GetComponent<HotfixEntityLogic>();
            InitCollect(entityObj);
            Refresh(userData);
            isShow = true;
        }

        /// <summary>
        /// 实体回收。
        /// </summary>
        protected virtual void OnRecycle()
        {
            Disposed();
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected virtual void OnHide(bool isShutdown, object userData)
        {
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected virtual void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected virtual void OnDetached(EntityLogic childEntity, object userData)
        {
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected virtual void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected virtual void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
        }

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
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
        /// 实体释放
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