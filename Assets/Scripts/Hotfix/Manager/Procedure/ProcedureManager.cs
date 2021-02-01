﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    /// <summary>
    /// 流程管理器。
    /// </summary>
    public sealed class ProcedureManager
    {
        private FsmManager m_FsmManager;
        private IFsm m_ProcedureFsm;

        private HotfixProcedureMgr EditorProcedureMgr;

        /// <summary>
        /// 初始化流程管理器的新实例。
        /// </summary>
        public ProcedureManager()
        {
            m_FsmManager = null;
            m_ProcedureFsm = null;

            GameObject go = new GameObject("__HotFixProcedure__");
            go.transform.parent = GameEntry.Base.transform.parent;
            EditorProcedureMgr = go.AddComponent<HotfixProcedureMgr>();
        }

        public void SetAllProcedureName(List<string> nameList)
        {
            EditorProcedureMgr.allHotfixProcedure = nameList;
        }

        /// <summary>
        /// 获取当前流程。
        /// </summary>
        public ProcedureBase CurrentProcedure
        {
            get
            {
                if (m_ProcedureFsm == null)
                {
                    throw new GameFrameworkException("You must initialize procedure first.");
                }

                return (ProcedureBase)m_ProcedureFsm.CurrentState;
            }
        }

        /// <summary>
        /// 获取当前流程持续时间。
        /// </summary>
        public float CurrentProcedureTime
        {
            get
            {
                if (m_ProcedureFsm == null)
                {
                    throw new GameFrameworkException("You must initialize procedure first.");
                }

                return m_ProcedureFsm.CurrentStateTime;
            }
        }

        /// <summary>
        /// 关闭并清理流程管理器。
        /// </summary>
        public void Shutdown()
        {
            if (m_FsmManager != null)
            {
                if (m_ProcedureFsm != null)
                {
                    m_FsmManager.DestroyFsm(m_ProcedureFsm);
                    m_ProcedureFsm = null;
                }

                m_FsmManager = null;
            }
        }

        /// <summary>
        /// 初始化流程管理器。
        /// </summary>
        /// <param name="fsmManager">有限状态机管理器。</param>
        /// <param name="procedures">流程管理器包含的流程。</param>
        public void Initialize(FsmManager fsmManager, params ProcedureBase[] procedures)
        {
            if (fsmManager == null)
            {
                throw new GameFrameworkException("FSM manager is invalid.");
            }

            m_FsmManager = fsmManager;
            m_ProcedureFsm = m_FsmManager.CreateFsm(this, procedures);
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <typeparam name="T">要开始的流程类型。</typeparam>
        public void StartProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start<T>();
            EditorProcedureMgr.nowHotfixProcedure = typeof(T).Name;
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureType">要开始的流程类型。</param>
        public void StartProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start(procedureType);
            EditorProcedureMgr.nowHotfixProcedure = procedureType.Name;
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <typeparam name="T">要检查的流程类型。</typeparam>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState<T>();
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureType">要检查的流程类型。</param>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState(procedureType);
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <typeparam name="T">要获取的流程类型。</typeparam>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            return m_ProcedureFsm.GetState<T>();
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <param name="procedureType">要获取的流程类型。</param>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            return (ProcedureBase)m_ProcedureFsm.GetState(procedureType);
        }
    }
}