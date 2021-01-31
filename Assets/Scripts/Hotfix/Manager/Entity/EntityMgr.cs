using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class EntityMgr
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;
        public static  int GenerateSerialId => --s_SerialId;

        public EntityComponent Component { get; }

        public EntityMgr(EntityComponent component)
        {
            Component = component;

            foreach (var variable in Enum.GetValues(typeof(EEntityGroup)))
            {
                Component.AddEntityGroup(Enum.GetName(typeof(EEntityGroup), variable), 60, 16, 60, 0);
            }
        }


        public T Show<T>(string       fullPath,
                         EEntityGroup eType    = EEntityGroup.Default,
                         object       userData = null,
                         int          prioity  = 0)
            where T : BaseEntityLogic, new()
        {
            T entity = new T();
            baseEntityAction baseEntityAction= entity.EntityAction(userData);
            baseEntityAction.HotfixObj = entity;
            Component.Show(GenerateSerialId, fullPath, eType.ToString(), prioity, baseEntityAction);
            return entity;
        }
        
        /// <summary>
        /// 隐藏实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        public void Hide(int entityId)
        {
            Component.HideEntity(entityId);
        }

        /// <summary>
        /// 隐藏实体。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void Hide(Entity entity, object userData)
        {
            Component.HideEntity(entity, userData);
        }

        /// <summary>
        /// 是否存在实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>是否存在实体。</returns>
        public bool HasEntity(int entityId)
        {
            return Component.HasEntity(entityId);
        }

        /// <summary>
        /// 获取实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>实体。</returns>
        public Entity GetEntity(int entityId)
        {
            return Component.GetEntity(entityId);
        }

        /// <summary>
        /// 获取实体。
        /// </summary>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <returns>要获取的实体。</returns>
        public Entity[] GetEntities(string entityAssetName)
        {
            return Component.GetEntities(entityAssetName);
        }

        /// <summary>
        /// 获取所有已加载的实体。
        /// </summary>
        /// <returns>所有已加载的实体。</returns>
        public Entity[] GetAllLoadedEntities()
        {
            return Component.GetAllLoadedEntities();
        }

        /// <summary>
        /// 获取所有正在加载实体的编号。
        /// </summary>
        /// <returns>所有正在加载实体的编号。</returns>
        public int[] GetAllLoadingEntityIds()
        {
            return Component.GetAllLoadingEntityIds();
        }

        /// <summary>
        /// 是否正在加载实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>是否正在加载实体。</returns>
        public bool IsLoadingEntity(int entityId)
        {
            return Component.IsLoadingEntity(entityId);
        }

        /// <summary>
        /// 是否是合法的实体。
        /// </summary>
        /// <param name="entity">实体。</param>
        /// <returns>实体是否合法。</returns>
        public bool IsValidEntity(Entity entity)
        {
            return Component.IsValidEntity(entity);
        }
    }
}