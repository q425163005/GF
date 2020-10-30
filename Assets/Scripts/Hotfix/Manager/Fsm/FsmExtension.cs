using System;
using System.Collections;
using System.Collections.Generic;
using Fuse.Hotfix.Common;
using Fuse.Tasks;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public static class FsmExtension
    {
        /// <summary>
        /// �л���ǰ����״̬��״̬��
        /// </summary>
        /// <typeparam name="TState">Ҫ�л���������״̬��״̬���͡�</typeparam>
        /// <param name="fsm">����״̬�����á�</param>
        public static void ChangeState<TState>(this IFsm fsm) where TState : FsmState
        {
            Fsm fsmImplement = (Fsm)fsm;
            if (fsmImplement == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }
            
            fsmImplement.ChangeState<TState>();
        }

        /// <summary>
        /// �л���ǰ����״̬��״̬��
        /// </summary>
        /// <param name="fsm">����״̬�����á�</param>
        /// <param name="stateType">Ҫ�л���������״̬��״̬���͡�</param>
        public static void ChangeState(this IFsm fsm, Type stateType)
        {
            Fsm fsmImplement = (Fsm)fsm;
            if (fsmImplement == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }

            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(FsmState).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            fsmImplement.ChangeState(stateType);
        }

        public static async CTask ChangeScene(this IFsm fsm, string sceneName, string procedure)
        {
            await LoadingUI.Show();
            fsm.SetData<VarString>("SceneName", sceneName);
            fsm.SetData<VarString>("TargetProcedure", procedure);
            Fsm fsmImplement = (Fsm)fsm;
            if (fsmImplement == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }
            fsmImplement.ChangeState<ProcedureChangeScene>();
        }

        public static async CTask ChangeScene<TState>(this IFsm fsm, string sceneName) where TState : FsmState
        {
            await LoadingUI.Show();
            fsm.SetData<VarString>("SceneName", sceneName);
            fsm.SetData<VarString>("TargetProcedure", typeof(TState).Name);
            Fsm fsmImplement = (Fsm)fsm;
            if (fsmImplement == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }
            fsmImplement.ChangeState<ProcedureChangeScene>();
        }
    }
}
