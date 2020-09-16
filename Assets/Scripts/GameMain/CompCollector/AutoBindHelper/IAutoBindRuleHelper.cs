using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    /// <summary>
    /// �Զ��󶨹��������ӿ�
    /// </summary>
    public interface IAutoBindRuleHelper
    {
        /// <summary>�Ƿ�Ϊ��Ч��</summary>
        bool IsValidBind(Transform target, List<string> filedNames, List<string> componentTypeNames);

        /// <summary>��ȡ��������</summary>
        string GetBindTips();
    }
}