//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
	/// <summary>物品表</summary>
	public class ItemConfig : BaseConfig
	{
		/// <summary>唯一ID</summary>
		public override object UniqueID => id;

		/// <summary>
		/// 物品ID
		/// </summary>
		public int id;

		/// <summary>
		/// 物品名称
		/// 语言ID
		/// </summary>
		public Lang name;

		/// <summary>
		/// 物品图标
		/// </summary>
		public string icon;

		/// <summary>
		/// 品质
		/// EQuality
		/// 1 1颗星
		/// 2 2颗星
		/// 3 3颗星
		/// 4 4颗星
		/// 5 5颗星
		/// </summary>
		public int quality;

		/// <summary>
		/// 物品大类
		/// 0虚拟物品
		/// 1道具
		/// 2装备
		/// </summary>
		public int type;

		/// <summary>
		/// 物品子类
		/// </summary>
		public int subType;

		/// <summary>
		/// 参数1
		/// 使用参照物品类型说明
		/// </summary>
		public int arg1;

		/// <summary>
		/// 参数2
		/// 使用参照物品类型
		/// 说明
		/// </summary>
		public int arg2;

		/// <summary>
		/// 卖出价格
		/// （金币）
		/// 0 不可出售
		/// </summary>
		public int sell;

		/// <summary>
		/// 物品描述
		/// 语言ID
		/// </summary>
		public Lang des;
	}
}

