//工具生成不要修改
//生成时间：2020/10/14 20:23:48
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Hotfix.Login
{
	public partial class LoginUI : BaseUI
	{
		private UnityEngine.UI.Button Btn_Login;
		/// <summary>初始化UI控件</summary>
		protected override void InitializeComponent()
		{
            base.InitializeComponent();
            Btn_Login = Get<UnityEngine.UI.Button>("Btn_Login");
		}
	}
}

