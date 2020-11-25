//工具生成不要修改
//生成时间：2020/11/8 19:14:57
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.Store
{
	public partial class StoreUI : BaseUI
	{
		private Button Btn_Back;
		private Toggle Tog_Pay;
		private Toggle Tog_Gift;
		private Toggle Tog_Equip;
		private Toggle Tog_Prop;
		private GameObject PayContent;
		private GameObject GiftContent;
		private GameObject EquipContent;
		private GameObject PropContent;
		private CompCollector PayItem;
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
			Btn_Back = Get<Button>("Btn_Back");
			Tog_Pay = Get<Toggle>("Tog_Pay");
			Tog_Gift = Get<Toggle>("Tog_Gift");
			Tog_Equip = Get<Toggle>("Tog_Equip");
			Tog_Prop = Get<Toggle>("Tog_Prop");
			PayContent = Get("PayContent");
			GiftContent = Get("GiftContent");
			EquipContent = Get("EquipContent");
			PropContent = Get("PropContent");
			PayItem = Get<CompCollector>("PayItem");
		}
	}
}

