using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    /// <summary>
    /// �Զ��󶨹��������ӿ�
    /// </summary>
    public interface IAutoBindRuleHelper
    {
        /// <summary>
        /// �Ƿ�Ϊ��Ч��
        /// </summary>
        bool IsValidBind(Transform target, List<string> filedNames, List<string> componentTypeNames);

        /// <summary>��ȡ��������</summary>
        string GetBindTips();

        /// <summary>��������</summary>
        string[] SearchNames();

        /// <summary>�Ƿ�ƥ��</summary>
        bool IsAccord(int searchType, string inputStr, string targetStr);

        /// <summary>���ɰ󶨴���</summary>
        void GenerateCode(CompCollector collector);
    }
}