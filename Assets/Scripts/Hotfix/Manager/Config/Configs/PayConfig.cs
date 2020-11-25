//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
	/// <summary>支付</summary>
	public class PayConfig : BaseConfig
	{
		/// <summary>唯一ID</summary>
		public override object UniqueID => id;

		/// <summary>
		/// 充值档位ID
		/// </summary>
		public int id;

		/// <summary>
		/// 商品名
		/// </summary>
		public Lang name;

		/// <summary>
		/// 商品类型
		/// 0普通充值
		/// 1月卡
		/// 2礼包
		/// </summary>
		public int type;

		/// <summary>
		/// 显示排序
		/// 越小越靠前
		/// </summary>
		public int sort;

		/// <summary>
		/// 标记
		/// 0无
		/// 1热销
		/// 2新品
		/// 3超值
		/// 4特价
		/// </summary>
		public int flag;

		/// <summary>
		/// 获得物品
		/// 1 钻石
		/// </summary>
		public List<int[]> items;

		/// <summary>
		/// 价格
		/// （美元）
		/// </summary>
		public int price;

		/// <summary>
		/// GooglePay商品Id
		/// </summary>
		public string gpId;

		/// <summary>
		/// iOS商品Id
		/// </summary>
		public string iosId;
	}
}

