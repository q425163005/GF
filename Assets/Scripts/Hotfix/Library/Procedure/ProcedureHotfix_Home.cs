using Fuse.Hotfix.Common;
using Fuse.Hotfix.Home;
using Fuse.Tasks;
using UnityEngine;
using ProcedureOwner = Fuse.Hotfix.IFsm;

namespace Fuse.Hotfix
{
    public class ProcedureHotfix_Home : ProcedureBase
    {
        private HomeUI homeUi;
        private bool startGame = false;

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            startGame = false;
            ShowHomeUI(procedureOwner).Run();
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
                                                  float          realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (startGame)
            {
                startGame = false;
                procedureOwner.ChangeScene<ProcedureHotfix_War>("War").Run();
            }
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        private async CTask ShowHomeUI(ProcedureOwner procedureOwner)
        {
            homeUi           = Mgr.UI.Show<HomeUI>();
            homeUi.procedure = this;
            await homeUi.Await();
            
            LoadingUI.Hide();
        }

        public void StartGame()
        {
            startGame = true;
        }
    }
}