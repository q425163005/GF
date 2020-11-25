//工具生成不要修改
//生成时间：2020/10/28 12:33:15
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.Common
{
	public partial class LoadingUI
	{
		private UnityEngine.UI.Slider Slider_Progress;
		private UnityEngine.UI.Text Txt_Progress;
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
            base.InitializeComponent();
            Slider_Progress = Get<UnityEngine.UI.Slider>("Slider_Progress");
			Txt_Progress = Get<UnityEngine.UI.Text>("Txt_Progress");
		}
	}
}

