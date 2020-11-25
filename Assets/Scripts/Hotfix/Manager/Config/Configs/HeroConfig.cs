//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
	/// <summary>英雄</summary>
	public class HeroConfig : BaseConfig
	{
		/// <summary>唯一ID</summary>
		public override object UniqueID => id;

		/// <summary>
		/// 英雄ID
		/// </summary>
		public int id;

		/// <summary>
		/// 英雄名
		/// 语言表ID
		/// </summary>
		public Lang name;

		/// <summary>
		/// 星级
		/// </summary>
		public int star;

		/// <summary>
		/// 英雄模型
		/// </summary>
		public string model;
	}
}

