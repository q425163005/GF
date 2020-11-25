using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix.Item
{
    public class EquipData : ItemData
    {
        /// <summary>ģ��ID</summary>
        public int EquipId { get; private set; }

        /// <summary>װ������</summary>
        public ItemEquipConfig equipConfig;
        

        public EquipData(int tempId) : base(tempId)
        {
            equipConfig = Mgr.Config.Get<ItemEquipConfig>(tempId);
            EquipId = equipConfig.id;
        }

        public void SetData(int tempId)
        {
            equipConfig = Mgr.Config.Get<ItemEquipConfig>(tempId);
            EquipId     = equipConfig.id;
        }
    }
}
