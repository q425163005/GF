using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public static partial class Constant
    {
        private const string ResRoot = "Assets/Res/BundleRes";

        public static class AssetPath
        {
            public static string Config(string fileName) => $"{ResRoot}/Data/Config/{fileName}.txt";
            public static string Localization(string lang) =>
                $"{ResRoot}/Data/Localization/{lang}.xml";
            public static string Scene(string sceneName)            => $"{ResRoot}/Scene/{sceneName}.unity";
            public static string Ui(string    space, string uiName) => $"{ResRoot}/UI/{space}/{uiName}.prefab";
            public static string UiItem(string space, string itemName) =>
                $"{ResRoot}/UI/{space}/{itemName}.prefab";
            
        }
    }
}