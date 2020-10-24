//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace Fuse
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static BuiltinDataComponent BuiltinData { get; private set; }

        /// <summary>
        /// ET网络组件
        /// </summary>
        public static ETNetworkComponent ETNetwork { get; private set; }

        /// <summary>
        /// ILRuntime组件
        /// </summary>
        public static ILRuntimeComponent ILRuntime { get; private set; }

        /// <summary>
        /// CTask组件
        /// </summary>
        public static CTaskComponent CTask { get; private set; }


        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            ETNetwork   = UnityGameFramework.Runtime.GameEntry.GetComponent<ETNetworkComponent>();
            ILRuntime   = UnityGameFramework.Runtime.GameEntry.GetComponent<ILRuntimeComponent>();
            CTask       = UnityGameFramework.Runtime.GameEntry.GetComponent<CTaskComponent>();
        }
    }
}