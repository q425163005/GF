//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2020/10/24 18:15:58
//------------------------------------------------------------

using System.Collections.Generic;
using Fuse.Tasks;

namespace Fuse.Hotfix
{
	public partial class ConfigMgr
	{




		private void LoadAllConfig()
		{


			//读取竖表配置
			LoadAllConfigV().Run();

			CustomRead();
		}
		/// <summary>读取竖表配置</summary>
		private async CTask LoadAllConfigV()
		{

		}
	}
}

