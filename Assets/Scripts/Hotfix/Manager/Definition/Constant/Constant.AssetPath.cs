using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public static partial class Constant
    {
        public static class AssetPath
        {
            public static string Config(string       fileName)  => $"Assets/Res/BundleRes/Data/Config/{fileName}.txt";
            public static string Localization(string lang)      => $"Assets/Res/BundleRes/Data/Localization/{lang}.xml";
            public static string Scene(string        sceneName) => $"Assets/Res/BundleRes/Scene/{sceneName}.unity";
        }
    }
}