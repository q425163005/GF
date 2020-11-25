//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
	/// <summary>人物初始配置</summary>
	public class PlayerInitConfig : BaseConfig
	{
		/// <summary>唯一ID</summary>
		public override object UniqueID => initHero;

		/// <summary>
		/// 初始英雄
		/// </summary>
		public int initHero;

		/// <summary>
		/// 初始枪支
		/// </summary>
		public int initWeapon;
	}
}

