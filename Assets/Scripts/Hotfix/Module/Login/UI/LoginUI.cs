namespace Fuse.Hotfix
{
	public partial class LoginUI : BaseUI
	{
		public LoginUI()
		{
			UIGroup = EUIGroup.Default;
		}
		protected override void Awake()
		{
            Btn_Login.onClick.AddListener(Btn_Login_Click);
		}

        private void Btn_Login_Click()
        {
            Log.Info("click Btn_Login");
        }
    }
}

