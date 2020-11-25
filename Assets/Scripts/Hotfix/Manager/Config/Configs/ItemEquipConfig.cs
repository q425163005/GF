//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Fuse.Hotfix
{
	/// <summary>装备表</summary>
	public class ItemEquipConfig : BaseConfig
	{
		/// <summary>唯一ID</summary>
		public override object UniqueID => id;

		/// <summary>
		/// 装备ID
		/// 对应Item表
		/// 物品ID
		/// </summary>
		public int id;

		/// <summary>
		/// 模型名
		/// </summary>
		public string model;

		/// <summary>
		/// 
		/// </summary>
		public string bulletModel;

		/// <summary>
		/// 武器节点
		/// 0 无
		/// 1 手枪 PistolPoint
		/// 2 长枪 WeaponPoint
		/// </summary>
		public int modelNode;

		/// <summary>
		/// 攻击间隔
		/// </summary>
		public double attackTime;

		/// <summary>
		/// 基础值
		/// 攻击力
		/// </summary>
		public int baseAttack;

		/// <summary>
		/// 升级加成
		/// 攻击力
		/// </summary>
		public int addAttack;
	}
}

