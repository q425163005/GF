﻿using System;
using System.Collections;
using System.Collections.Generic;
using ETHotfix;
using Fuse.Hotfix.Common;
using Fuse.Hotfix.Login;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class ProcedureHotfixTest : ProcedureBase
    {


        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            Log.Info("进入了热更新测试流程");
            Mgr.UI.Show<LoginUI>();
        }

        protected internal override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
//            if (Input.GetKeyDown(KeyCode.A))
//            {
//                Session session = Mgr.ETNetwork.CreateHotfixSession();
//                session.Send(new HotfixTestMessage() { Info = "6666" });
//
//                HotfixRpcResponse response = (HotfixRpcResponse)await session.Call(new HotfixRpcRequest() { Info = "Hello Server" });
//                Debug.Log(response.Info);
//                session.Dispose();
//            }
        }

        

    }
}

