using Fuse.Hotfix.Login;
using Fuse.Tasks;
using ProcedureOwner = Fuse.Hotfix.IFsm;


namespace Fuse.Hotfix
{
    public class ProcedureHotfix_Login : ProcedureBase
    {
        private bool login = false;

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            login = false;
            Mgr.UI.Show<LoginUI>().procedure = this;
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
                                                  float          realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (login)
            {
                login = false;
                procedureOwner.ChangeScene<ProcedureHotfix_Home>("Home").Run();
            }
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            Mgr.UI.Close<LoginUI>();
        }

        public void Login()
        {
            login = true;
        }
    }
}