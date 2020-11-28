//工具生成不要修改
//生成时间：2020/11/28 17:48:13
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.War
{
	public partial class SquareItem : BaseItem
	{
		private Text Txt_Num;
		private Image Img_This;
		private GameObject imgSel;
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
			Txt_Num = Get<Text>("Txt_Num");
			Img_This = Get<Image>("Img_This");
			imgSel = Get("imgSel");
		}
	}
}

