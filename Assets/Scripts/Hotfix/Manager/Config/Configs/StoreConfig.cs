//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
	/// <summary>商城</summary>
	public class StoreConfig : BaseConfig
	{
		/// <summary>唯一ID</summary>
		public override object UniqueID => id;

		/// <summary>
		/// 商品ID
		/// </summary>
		public int id;

		/// <summary>
		/// 商品名
		/// </summary>
		public Lang name;

		/// <summary>
		/// 商品类型
		/// 0道具
		/// 1装备
		/// </summary>
		public int type;

		/// <summary>
		/// 获得物品
		/// 1 钻石
		/// </summary>
		public List<int[]> items;

		/// <summary>
		/// 价格
		/// </summary>
		public int price;
	}
}

