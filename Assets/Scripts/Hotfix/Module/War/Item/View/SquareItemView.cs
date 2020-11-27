//工具生成不要修改
//生成时间：2020/11/27 20:12:17
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.War
{
	public partial class SquareItem : BaseItem
	{
		private Text Txt_Num;
		private Image Img_This;
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
			Txt_Num = Get<Text>("Txt_Num");
			Img_This = Get<Image>("Img_This");
		}
	}
}

