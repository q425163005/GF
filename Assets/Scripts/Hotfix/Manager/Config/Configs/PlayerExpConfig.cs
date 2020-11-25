//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
	/// <summary>玩家升级经验</summary>
	public class PlayerExpConfig : BaseConfig
	{
		/// <summary>唯一ID</summary>
		public override object UniqueID => id;

		/// <summary>
		/// 等级
		/// </summary>
		public int id;

		/// <summary>
		/// 升到下级所需经验
		/// </summary>
		public int exp;

		/// <summary>
		/// 体力上限
		/// </summary>
		public int powerLimit;
	}
}

