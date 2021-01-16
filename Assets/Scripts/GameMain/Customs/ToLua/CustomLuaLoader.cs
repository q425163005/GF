using LuaInterface;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace Fuse
{
    /// <summary>
    /// Lua资源加载器
    /// </summary>
    public class CustomLuaLoader : LuaFileUtils
    {
        public delegate bool GetScript(string fileName, out byte[] buffer);

        private GetScript m_GetScript;

        public CustomLuaLoader(GetScript getScript) : base()
        {
            m_GetScript = getScript;
        }

        public override byte[] ReadFile(string fileName)
        {
            if (Application.isEditor && GameEntry.Base.EditorResourceMode)
            {
                return base.ReadFile(fileName);
            }

            if (!fileName.EndsWith(".lua"))
            {
                fileName += ".lua";
            }

            byte[] buffer;
            if (!m_GetScript(fileName, out buffer))
            {
                throw new GameFrameworkException(Utility.Text.Format("File '{0}' not loaded.", fileName));
                //buffer = LoadLuaFile(fileName);
            }

            return buffer;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Dictionary<string, UnityEngine.AssetBundle> LuaBundleDic = new Dictionary<string, AssetBundle>();

        private byte[] LoadLuaFile(string fileName)
        {
            byte[] buffer = null;
            string assetname = "Assets/Lua/" + fileName + LuaComponent.LuaAssetExtInBundle;
            buffer = LoadLua(assetname);
            return buffer;
        }


        private string GetResourceNameWithSuffix(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new GameFrameworkException("Resource Name is invalid.");
            }
            return Utility.Text.Format("{0}.dat", resourceName);
        }

        public byte[] LoadLua(string assetName)
        {
            byte[] bytes = null;


            string abFullPath = GetResourceNameWithSuffix(GameEntry.Resource.ReadWritePath + "/lua");
            UnityEngine.AssetBundle bundle = null;
            if (LuaBundleDic.ContainsKey(abFullPath))
            {
                bundle = LuaBundleDic[abFullPath];
            }
            else
            {
                bundle = UnityEngine.AssetBundle.LoadFromFile(abFullPath);
                LuaBundleDic.Add(abFullPath, bundle);
            }

            var m_Request = bundle.LoadAsset(assetName, typeof(TextAsset));
            if (m_Request == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("lua包体不包含file {0}", assetName));
            }

            bytes = (m_Request as TextAsset).bytes;
            return bytes;
        }
    }
}
