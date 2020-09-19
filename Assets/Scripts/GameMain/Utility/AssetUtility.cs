//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace Fuse
{
    public static class AssetUtility
    {
        

        public static string UpdateFormAsset = "Assets/Res/VersionCheck/UpdateResourceForm.prefab";

        public static string GetHotfixDLLAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Res/BundleRes/Data/HotFix/{0}.bytes", assetName);
        }
    }
}