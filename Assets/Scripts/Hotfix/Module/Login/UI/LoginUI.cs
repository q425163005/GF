using GameFramework.Localization;
using UnityGameFramework.Runtime;

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

        public override void OnOpen(object userdata)
        {
            base.OnOpen(userdata);
            UpdateResourceForm.Close();
        }

        private void Btn_Login_Click()
        {
            Log.Info("click Btn_Login");

            GameEntry.Setting.SetString(Constant.Setting.Language,
                                        GameEntry.Localization.Language == Language.ChineseSimplified
                                            ? Language.English.ToString()
                                            : Language.ChineseSimplified.ToString());
            GameEntry.Setting.Save();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }
    }
}

