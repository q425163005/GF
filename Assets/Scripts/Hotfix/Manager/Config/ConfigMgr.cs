//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/11/25 16:52:49
//------------------------------------------------------------

using System.Collections.Generic;
using Fuse.Tasks;

namespace Fuse.Hotfix
{
	public partial class ConfigMgr
	{
		/// <summary> 人物初始配置 </summary>
		public PlayerInitConfig playerInitConfig;


		private void LoadAllConfig()
		{
			readConfig<StoreConfig>().Run();//商城
			readConfig<PayConfig>().Run();//支付
			readConfig<ItemConfig>().Run();//物品表
			readConfig<ItemEquipConfig>().Run();//装备表
			readConfig<PlayerExpConfig>().Run();//玩家升级经验
			readConfig<HeroConfig>().Run();//英雄


			//读取竖表配置
			LoadAllConfigV().Run();

			CustomRead();
		}
		/// <summary>读取竖表配置</summary>
		private async CTask LoadAllConfigV()
		{
			playerInitConfig = await readConfigV<PlayerInitConfig>();

		}
	}
}

