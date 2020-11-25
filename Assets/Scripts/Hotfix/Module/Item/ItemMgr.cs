using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.Hotfix.Item
{
    public class ItemMgr : BaseDataMgr<ItemMgr>, IDisposable
    {
        /// <summary>
        /// 装备列表
        /// </summary>
        public Dictionary<int, EquipData> DicEquip { get; } = new Dictionary<int, EquipData>();

        /// <summary>
        /// 道具列表
        /// </summary>
        public Dictionary<int, PropData> DicProp { get; } = new Dictionary<int, PropData>();

        public void AddOrUpdateEquip(EquipData it)
        {
            if (!DicEquip.TryGetValue(it.EquipId, out var item))
            {
                item = new EquipData(it.EquipId);

                DicEquip.Add(it.EquipId, item);
            }

            item.SetData(it.EquipId);
        }

        public override void Dispose()
        {
            base.Dispose();
            DicEquip.Clear();
            DicProp.Clear();
        }
    }
}
