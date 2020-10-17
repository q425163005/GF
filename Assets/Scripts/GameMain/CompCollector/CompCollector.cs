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
        [SerializeField] public string m_SelRuleName   { get; set; }
        [SerializeField] public string m_SelCodeName   { get; set; }
        [SerializeField] public string m_SelSearchName { get; set; }
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