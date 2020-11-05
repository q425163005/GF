using Fuse.Tasks;
using GameFramework.Localization;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix.Login
{
    public partial class LoginUI : BaseUI
    {
        private IFsm m_IFsm;
        private bool enterHome = false;

        public LoginUI()
        {
            UIGroup = EUIGroup.Default;
        }

        protected override void Awake(object userdata)
        {
            Btn_Login.AddClick(Btn_Login_Click);
        }

        protected override void OnOpen(object userdata)
        {
            base.OnOpen(userdata);
            m_IFsm = (IFsm) userdata;
            UpdateResourceForm.Close();
            if (m_IFsm == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
        }

        private void Btn_Login_Click()
        {
            if (enterHome) return;
            enterHome = true;
            EnterHome().Run();

//            GameEntry.Setting.SetString(Constant.Setting.Language,
//                                        GameEntry.Localization.Language == Language.ChineseSimplified
//                                            ? Language.English.ToString()
//                                            : Language.ChineseSimplified.ToString());
//            GameEntry.Setting.Save();
//            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }

        private async CTask EnterHome()
        {
            await m_IFsm.ChangeScene<ProcedureHotfix_Home>("Home");
          
            enterHome = false;
        }
    }
}