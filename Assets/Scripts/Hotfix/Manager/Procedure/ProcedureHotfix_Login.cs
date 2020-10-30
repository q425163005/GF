using Fuse.Hotfix.Login;
using ProcedureOwner = Fuse.Hotfix.IFsm;


namespace Fuse.Hotfix
{
    public class ProcedureHotfix_Login : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            Mgr.UI.Show<LoginUI>(procedureOwner);
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            Mgr.UI.Close<LoginUI>();
        }
    }
}

