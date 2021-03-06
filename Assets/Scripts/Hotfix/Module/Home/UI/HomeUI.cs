﻿using System;
using DG.Tweening;
using Fuse.Hotfix.Common;
using Fuse.Hotfix.Item;
using Fuse.Hotfix.Store;
using Fuse.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fuse.Hotfix.Home
{
    public partial class HomeUI : BaseUI
    {
        public ProcedureHotfix_Home procedure;

        public HomeUI()
        {
            UIGroup = EUIGroup.Main;
        }
        

        protected override void Init(object userdata)
        {
        }

        protected override void Refresh(object userdata)
        {
        }

        protected override void Disposed()
        {
        }
    }
}