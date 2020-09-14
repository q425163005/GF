using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace Fuse.Editor
{
    public static class BuildHotfixEditor
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/Res/BundleRes/Data/HotFix/";
        private const string HotfixDll = "Fuse.Hotfix.dll";
        private const string HotfixPdb = "Fuse.Hotfix.pdb";

        
        public static void BuildHotfixDLL()
        {
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            Debug.Log("复制Hotfix.dll, Hotfix.pdb到Assets/Res/BundleRes/Data/HotFix完成");
            AssetDatabase.Refresh();

        }
    }
}

