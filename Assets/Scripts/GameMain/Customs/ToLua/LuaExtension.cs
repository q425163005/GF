using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    public static class LuaExtension
    {
        public static void TryGC()
        {
            GameEntry.Lua.TryFormGC();
        }


        public static void SwithLogModle()
        {

        }

    }
}
