using Fuse.Hotfix.Common;
using Fuse.Hotfix.Home;
using Fuse.Hotfix.Item;
using Fuse.Hotfix.War;
using Fuse.Tasks;
using UnityEngine;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public class ProcedureHotfix_War : ProcedureBase
    {
        private WarUI warUi;
        private bool  exitWar = false;
        


        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            exitWar = false;
            ShowWarUI(procedureOwner).Run();
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
                                                  float          realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (exitWar)
            {
                exitWar = false;
                procedureOwner.ChangeScene<ProcedureHotfix_Home>("Home").Run();
            }
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        private async CTask ShowWarUI(ProcedureOwner procedureOwner)
        {
            WarData wardata=new WarData();
            wardata.half_Width = 5;
            wardata.half_Height = 10;

            warUi           = Mgr.UI.Show<WarUI>(wardata);
            warUi.procedure = this;
            await warUi.Await();
            LoadingUI.Hide();
        }

        public void ExitWar()
        {
            exitWar = true;
        }
    }
}