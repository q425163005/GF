using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix.Item
{
    /// <summary>
    /// Item���ݻ���
    /// </summary>
    public class ItemData
    {
        /// <summary>ģ��ID</summary>
        public int TempId => Config.id;

        /// <summary>��Ʒ����</summary>
        public ItemConfig Config;

        /// <summary>��Ʒ����</summary>
        public EItemType Type => (EItemType) Config.type;

        /// <summary>��ƷƷ��</summary>
        public EQuality Quality => (EQuality) Config.quality;

        /// <summary>����</summary>
        public int Num { get; protected set; } = 0;
        
        /// <summary>����</summary>
        public string ItemName => Config.name.Value;

        /// <summary>ͼƬ</summary>
        public string Icon => Config.icon;

        /// <summary>����</summary>
        public string Des => Config.des.Value;

        /// <summary>��Ʒ����</summary>
        public int subType => Config.subType;

        /// <summary></summary>
        /// <param name="tempId">��Ʒģ��ID</param>
        public ItemData(int tempId)
        {
            Config = Mgr.Config.Get<ItemConfig>(tempId);
        }

        /// <summary>
        /// ������Ʒ��ʾ����
        /// </summary>
        /// <param name="tempId"></param>
        /// <param name="Count"></param>
        public ItemData(int tempId, int Count)
        {
            Config = Mgr.Config.Get<ItemConfig>(tempId);
            Num    = Count;
        }
    }
}