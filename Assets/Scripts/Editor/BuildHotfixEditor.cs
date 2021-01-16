using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace Fuse.Editor
{
    public static class BuildHotfixEditor
    {
        class luaList
        {
            public List<string> toLua = new List<string>();
            public List<string> Lua   = new List<string>();
        }

        private const string CodeDir = "Assets/Res/BundleRes/Data/HotFix";

        public static void CopyLuaFile()
        {
            ClearLuaFile();
            string toLuaPath = CodeDir + "/ToLua";
            string LuaPath   = CodeDir + "/Lua";

            luaList list = new luaList();
            list.toLua = CopyLuaBytesFiles(LuaConst.toluaDir, toLuaPath);
            list.Lua = CopyLuaBytesFiles(LuaConst.luaDir, LuaPath);

            string json = JsonUtility.ToJson(list);
            ToolsHelper.SaveFile(CodeDir+"/LuaList.txt" ,json);
            
            AssetDatabase.Refresh();
            Debug.Log("Copy lua files over");
        }

        public static void ClearLuaFile()
        {
            if (Directory.Exists(CodeDir))
            {
                Directory.Delete(CodeDir, true);
            }

            AssetDatabase.Refresh();
            Debug.Log("清理完毕！");
        }

        static List<string> CopyLuaBytesFiles(string       sourceDir, string destDir, bool appendext = true,
                                              string       searchPattern = "*.lua",
                                              SearchOption option        = SearchOption.AllDirectories)
        {
            
            List<string > retList=new List<string>();
            if (!Directory.Exists(sourceDir))
            {
                return retList;
            }

            string[] files = Directory.GetFiles(sourceDir, searchPattern, option);
            int      len   = sourceDir.Length;

            if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
            {
                --len;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string str          = files[i].Remove(0, len);
                
                retList.Add(files[i].Remove(0, len+1).Replace("\\","/"));
                
                string dest         = destDir + "/" + str;
                if (appendext) dest += ".bytes";
                string dir          = Path.GetDirectoryName(dest);
                Directory.CreateDirectory(dir);
                File.Copy(files[i], dest, true);
            }

            return retList;
        }
    }
}