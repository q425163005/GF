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
        /// CTask组件
        /// </summary>
        public static CTaskComponent CTask { get; private set; }

        public static LuaComponent Lua { get; private set; }


        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            CTask       = UnityGameFramework.Runtime.GameEntry.GetComponent<CTaskComponent>();
            Lua         = UnityGameFramework.Runtime.GameEntry.GetComponent<LuaComponent>();
        }
    }
}