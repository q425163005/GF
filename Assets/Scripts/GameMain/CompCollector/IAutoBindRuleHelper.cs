using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    /// <summary>
    /// 自动绑定规则辅助器接口
    /// </summary>
    public interface IAutoBindRuleHelper
    {
        /// <summary>
        /// 是否为有效绑定
        /// </summary>
        bool IsValidBind(Transform target, List<string> filedNames, List<string> componentTypeNames);

        /// <summary>获取绑定索引表</summary>
        string GetBindTips();

        /// <summary>搜索类型</summary>
        string[] SearchNames();

        /// <summary>是否匹配</summary>
        bool IsAccord(int searchType, string inputStr, string targetStr);

        /// <summary>生成绑定代码</summary>
        void GenerateCode(CompCollector collector);
    }
}