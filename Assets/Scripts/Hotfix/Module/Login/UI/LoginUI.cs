using Fuse.Tasks;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix.Login
{
    public partial class LoginUI : BaseUI
    {
        public ProcedureHotfix_Login procedure;

        public LoginUI()
        {
            UIGroup = EUIGroup.Default;
        }

        protected override void Refresh(object userData = null)
        {
            UpdateResourceForm.Close();

            Btn_Login.AddClick(Btn_Login_Click);
        }
        

        private void Btn_Login_Click()
        {
            procedure.Login();
            VirtualServer.Start();
//            GameEntry.Setting.SetString(Constant.Setting.Language,
//                                        GameEntry.Localization.Language == Language.ChineseSimplified
//                                            ? Language.English.ToString()
//                                            : Language.ChineseSimplified.ToString());
//            GameEntry.Setting.Save();
//            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }
        
    }
}