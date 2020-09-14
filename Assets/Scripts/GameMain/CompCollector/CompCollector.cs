using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    /// <summary>
    /// 组件自动绑定工具
    /// </summary>
    public class CompCollector : MonoBehaviour
    {
#if UNITY_EDITOR

        public IAutoBindRuleHelper RuleHelper {
            get;
            set;
        }


#endif


        [System.Serializable]
        public class CompCollectorInfo
        {
            public string Name;

            public string ComponentType;

            public Object Object;

        }
        public List<CompCollectorInfo> CompCollectorInfos = new List<CompCollectorInfo>();
    }
}

