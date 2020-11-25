//工具生成不要修改
//生成时间：2020/11/8 17:24:00
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.Store
{
	public partial class PayItem : BaseItem
	{
		private UnityEngine.UI.Text Txt_Price;
		private UnityEngine.UI.Text Txt_Name;
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
			Txt_Price = Get<UnityEngine.UI.Text>("Txt_Price");
			Txt_Name = Get<UnityEngine.UI.Text>("Txt_Name");
		}
	}
}

