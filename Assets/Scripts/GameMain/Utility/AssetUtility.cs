﻿//------------------------------------------------------------
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
        public static string GetConfigAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("Assets/Res/FrameConfigs/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        }

        //        public static string GetDataTableAsset(string assetName, bool fromBytes)
        //        {
        //            return Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        //        }

        public static string GetDictionaryAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("Assets/Res/FrameConfigs/{0}.{1}", assetName, fromBytes ? "bytes" : "xml");
        }

        //        public static string GetFontAsset(string assetName)
        //        {
        //            return Utility.Text.Format("Assets/Res/Fonts/{0}.ttf", assetName);
        //        }
        //
        //        public static string GetSceneAsset(string assetName)
        //        {
        //            return Utility.Text.Format("Assets/Res/Scenes/{0}.unity", assetName);
        //        }
        //
        //        public static string GetMusicAsset(string assetName)
        //        {
        //            return Utility.Text.Format("Assets/Res/Music/{0}.mp3", assetName);
        //        }
        //
        //        public static string GetSoundAsset(string assetName)
        //        {
        //            return Utility.Text.Format("Assets/Res/Sounds/{0}.wav", assetName);
        //        }
        //
        //        public static string GetEntityAsset(string assetName)
        //        {
        //            return Utility.Text.Format("Assets/Res/Entities/{0}.prefab", assetName);
        //        }
        //
        //        public static string GetUIFormAsset(string assetName)
        //        {
        //            return Utility.Text.Format("Assets/Res/UI/UIForms/{0}.prefab", assetName);
        //        }
        //
        //        public static string GetUISoundAsset(string assetName)
        //        {
        //            return Utility.Text.Format("Assets/Res/UI/UISounds/{0}.wav", assetName);
        //        }

        public static string UpdateFormAsset = "Assets/Res/VersionCheck/UpdateResourceForm.prefab";

        public static string GetHotfixDLLAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Res/BundleRes/Data/HotFix/{0}.bytes", assetName);
        }
    }
}