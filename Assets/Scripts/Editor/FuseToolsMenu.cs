using Fuse.Editor.DataTableTools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using GameFramework;

namespace Fuse.Editor
{
    /// <summary>
    /// 框架工具菜单
    /// </summary>
    public class FuseToolsMenu
    {
        #region 快捷键       

        private const string LastScenePrefKey = "CSF.LastSceneOpen";

        /// <summary>
        /// 打开主场景之前的一个场景
        /// </summary>
        [MenuItem("★工具★/快捷键/打开上个场景 _F4", false, -20)]
        public static void OpenLastScene()
        {
            var lastScene = EditorPrefs.GetString(LastScenePrefKey);
            if (!string.IsNullOrEmpty(lastScene))
                ToolsHelper.OpenScene(lastScene);
            else
                Debug.LogError("Not found last scene!");
        }

        /// <summary>
        /// 打开主场景
        /// </summary>
        [MenuItem("★工具★/快捷键/开始游戏 _F5", false, -10)]
        public static void OpenMainScene()
        {
            var currentScene = EditorSceneManager.GetActiveScene().path;
            var mainScene = "Assets/Main.unity";
            if (mainScene != currentScene)
                EditorPrefs.SetString(LastScenePrefKey, currentScene);

            ToolsHelper.OpenScene(mainScene);

            if (!EditorApplication.isPlaying)
                EditorApplication.isPlaying = true;
        }

        //[MenuItem("★工具★/关卡编辑器")]
        //public static void OpenEdtiroScene()
        //{
        //    var currentScene = EditorSceneManager.GetActiveScene().path;
        //    var mainScene = "Assets/EditorTools/MapEditor/MapEditor.unity";
        //    if (mainScene != currentScene)
        //        EditorPrefs.SetString(LastScenePrefKey, currentScene);

        //    ToolsHelper.OpenScene(mainScene);
        //    if (!EditorApplication.isPlaying)
        //        EditorApplication.isPlaying = true;

        //    //设置关卡编辑分辨率
        //    var type = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
        //    var window = EditorWindow.GetWindow(type);
        //    var SizeSelectionCallback = type.GetMethod("SizeSelectionCallback",
        //        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
        //        System.Reflection.BindingFlags.NonPublic);
        //    SizeSelectionCallback.Invoke(window, new object[] { 0, null });
        //}

        #endregion

        // [MenuItem("★工具★/拷贝热更新DLL",false,1)]
        // public static void BuildHotfixDLL()
        // {
        //     BuildHotfixEditor.BuildHotfixDLL();
        // }
        
        [MenuItem("★工具★/Lua/Copy",false)]
        public static void CopyLuaFile()
        {
            BuildHotfixEditor.CopyLuaFile();
        }
        
        [MenuItem("★工具★/Lua/Clear",false)]
        public static void ClearLuaFile()
        {
            BuildHotfixEditor.ClearLuaFile();
        }

        [MenuItem("★工具★/Excel转Config工具", false, 2)]
        public static void Excel2Config()
        {
            ConfigGeneratorEditor.ShowWindow();
        }

        [MenuItem("★工具★/代码模板生成工具", false, 3)]
        public static void CodeGenerator()
        {
            CodeGeneratorEditor.ShowWindow();
        }

        
        

    }
}