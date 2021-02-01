using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fuse
{
    /// <summary>
    /// 组件自动绑定工具
    /// </summary>
    public class CompCollector : MonoBehaviour
    {
#if UNITY_EDITOR
        public string m_SelRuleName   = "";
        public string m_SelCodeName   = "";
        public string m_SelSearchName = "";
        public string m_EntityName    = "";
#endif

        [System.Serializable]
        public class CompCollectorInfo
        {
            public string Name = "";

            public string ComponentType = "";

            public Object Object;
        }

        public List<CompCollectorInfo> CompCollectorInfos = new List<CompCollectorInfo>();
    }
}