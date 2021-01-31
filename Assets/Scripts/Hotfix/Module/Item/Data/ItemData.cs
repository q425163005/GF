using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix.Item
{
    /// <summary>
    /// Item数据基类
    /// </summary>
    public class ItemData
    {
        /// <summary>模板ID</summary>
        public int TempId => Config.id;

        /// <summary>物品配置</summary>
        public ItemConfig Config;

        /// <summary>物品大类</summary>
        public EItemType Type => (EItemType) Config.type;

        /// <summary>物品品质</summary>
        public EQuality Quality => (EQuality) Config.quality;

        /// <summary>数量</summary>
        public int Num { get; protected set; } = 0;
        
        /// <summary>名称</summary>
        public string ItemName => Config.name.Value;

        /// <summary>图片</summary>
        public string Icon => Config.icon;

        /// <summary>描述</summary>
        public string Des => Config.des.Value;

        /// <summary>物品子类</summary>
        public int subType => Config.subType;

        /// <summary></summary>
        /// <param name="tempId">物品模板ID</param>
        public ItemData(int tempId)
        {
            Config = Mgr.Config.Get<ItemConfig>(tempId);
        }

        /// <summary>
        /// 用于商品显示数量
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