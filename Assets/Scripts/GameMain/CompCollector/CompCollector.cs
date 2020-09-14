using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    /// <summary>
    /// ����Զ��󶨹���
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

