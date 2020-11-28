//工具生成不要修改
//生成时间：2020/11/28 17:04:48
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.War
{
	public partial class WarUI
	{
		private CompCollector FrameItem;
		private RectTransform FrameContent;
		private RectTransform SquareContent;
		private CompCollector SquareItem;
		private RectTransform CreatePoint;
		private GameObject Txt_FreeMove;
		private Button Btn_Close;
		private Button Btn_Drop;
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
			FrameItem = Get<CompCollector>("FrameItem");
			FrameContent = Get<RectTransform>("FrameContent");
			SquareContent = Get<RectTransform>("SquareContent");
			SquareItem = Get<CompCollector>("SquareItem");
			CreatePoint = Get<RectTransform>("CreatePoint");
			Txt_FreeMove = Get("Txt_FreeMove");
			Btn_Close = Get<Button>("Btn_Close");
			Btn_Drop = Get<Button>("Btn_Drop");
		}
	}
}

