using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class EntityMgr
    {
        // ���� EntityId ��Լ����
        // 0 Ϊ��Ч
        // ��ֵ���ںͷ�����ͨ�ŵ�ʵ�壨����ҽ�ɫ��NPC���ֵȣ�������ֻ������ֵ��
        // ��ֵ���ڱ������ɵ���ʱʵ�壨����Ч��FakeObject�ȣ�
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
        /// ����ʵ�塣
        /// </summary>
        /// <param name="entityId">ʵ���š�</param>
        public void Hide(int entityId)
        {
            Component.HideEntity(entityId);
        }

        /// <summary>
        /// ����ʵ�塣
        /// </summary>
        /// <param name="entity">ʵ�塣</param>
        /// <param name="userData">�û��Զ������ݡ�</param>
        public void Hide(Entity entity, object userData)
        {
            Component.HideEntity(entity, userData);
        }

        /// <summary>
        /// �Ƿ����ʵ�塣
        /// </summary>
        /// <param name="entityId">ʵ���š�</param>
        /// <returns>�Ƿ����ʵ�塣</returns>
        public bool HasEntity(int entityId)
        {
            return Component.HasEntity(entityId);
        }

        /// <summary>
        /// ��ȡʵ�塣
        /// </summary>
        /// <param name="entityId">ʵ���š�</param>
        /// <returns>ʵ�塣</returns>
        public Entity GetEntity(int entityId)
        {
            return Component.GetEntity(entityId);
        }

        /// <summary>
        /// ��ȡʵ�塣
        /// </summary>
        /// <param name="entityAssetName">ʵ����Դ���ơ�</param>
        /// <returns>Ҫ��ȡ��ʵ�塣</returns>
        public Entity[] GetEntities(string entityAssetName)
        {
            return Component.GetEntities(entityAssetName);
        }

        /// <summary>
        /// ��ȡ�����Ѽ��ص�ʵ�塣
        /// </summary>
        /// <returns>�����Ѽ��ص�ʵ�塣</returns>
        public Entity[] GetAllLoadedEntities()
        {
            return Component.GetAllLoadedEntities();
        }

        /// <summary>
        /// ��ȡ�������ڼ���ʵ��ı�š�
        /// </summary>
        /// <returns>�������ڼ���ʵ��ı�š�</returns>
        public int[] GetAllLoadingEntityIds()
        {
            return Component.GetAllLoadingEntityIds();
        }

        /// <summary>
        /// �Ƿ����ڼ���ʵ�塣
        /// </summary>
        /// <param name="entityId">ʵ���š�</param>
        /// <returns>�Ƿ����ڼ���ʵ�塣</returns>
        public bool IsLoadingEntity(int entityId)
        {
            return Component.IsLoadingEntity(entityId);
        }

        /// <summary>
        /// �Ƿ��ǺϷ���ʵ�塣
        /// </summary>
        /// <param name="entity">ʵ�塣</param>
        /// <returns>ʵ���Ƿ�Ϸ���</returns>
        public bool IsValidEntity(Entity entity)
        {
            return Component.IsValidEntity(entity);
        }
    }
}