//工具生成不要修改
//生成时间：2020/11/27 20:14:58
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
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
			FrameItem = Get<CompCollector>("FrameItem");
			FrameContent = Get<RectTransform>("FrameContent");
			SquareContent = Get<RectTransform>("SquareContent");
			SquareItem = Get<CompCollector>("SquareItem");
		}
	}
}

