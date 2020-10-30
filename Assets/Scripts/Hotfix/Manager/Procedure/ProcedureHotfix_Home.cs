using Fuse.Hotfix.Common;
using Fuse.Hotfix.Home;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
	public class ProcedureHotfix_Home : ProcedureBase
	{
		protected internal override void OnEnter(ProcedureOwner procedureOwner)
		{
			base.OnEnter(procedureOwner);
            LoadingUI.Hide();
            Mgr.UI.Show<HomeUI>();
        }
		protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
		                                          float          realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
		}
		protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			base.OnLeave(procedureOwner, isShutdown);
		}
	}
}

