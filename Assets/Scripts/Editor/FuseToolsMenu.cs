using Fuse.Editor.DataTableTools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using GameFramework;

namespace Fuse.Editor
{
    /// <summary>
    /// ��ܹ��߲˵�
    /// </summary>
    public class FuseToolsMenu
    {
        #region ��ݼ�       

        private const string LastScenePrefKey = "CSF.LastSceneOpen";

        /// <summary>
        /// ��������֮ǰ��һ������
        /// </summary>
        [MenuItem("�﹤�ߡ�/��ݼ�/���ϸ����� _F4", false, -20)]
        public static void OpenLastScene()
        {
            var lastScene = EditorPrefs.GetString(LastScenePrefKey);
            if (!string.IsNullOrEmpty(lastScene))
                ToolsHelper.OpenScene(lastScene);
            else
                Debug.LogError("Not found last scene!");
        }

        /// <summary>
        /// ��������
        /// </summary>
        [MenuItem("�﹤�ߡ�/��ݼ�/��ʼ��Ϸ _F5", false, -10)]
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

        //[MenuItem("�﹤�ߡ�/�ؿ��༭��")]
        //public static void OpenEdtiroScene()
        //{
        //    var currentScene = EditorSceneManager.GetActiveScene().path;
        //    var mainScene = "Assets/EditorTools/MapEditor/MapEditor.unity";
        //    if (mainScene != currentScene)
        //        EditorPrefs.SetString(LastScenePrefKey, currentScene);

        //    ToolsHelper.OpenScene(mainScene);
        //    if (!EditorApplication.isPlaying)
        //        EditorApplication.isPlaying = true;

        //    //���ùؿ��༭�ֱ���
        //    var type = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
        //    var window = EditorWindow.GetWindow(type);
        //    var SizeSelectionCallback = type.GetMethod("SizeSelectionCallback",
        //        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public |
        //        System.Reflection.BindingFlags.NonPublic);
        //    SizeSelectionCallback.Invoke(window, new object[] { 0, null });
        //}

        #endregion

        [MenuItem("�﹤�ߡ�/�����ȸ���DLL",false,1)]
        public static void BuildHotfixDLL()
        {
            BuildHotfixEditor.BuildHotfixDLL();
        }

        [MenuItem("�﹤�ߡ�/ExcelתConfig����", false, 2)]
        public static void Excel2Config()
        {
            ConfigGeneratorEditor.ShowWindow();
        }

        [MenuItem("�﹤�ߡ�/����ģ�����ɹ���", false, 3)]
        public static void CodeGenerator()
        {
            CodeGeneratorEditor.ShowWindow();
        }


        [MenuItem("�﹤�ߡ�/ILRuntime/Generate ILRuntime CLR Binding Code by Analysis")]
        public static void GenerateCLRBindingByAnalysis()
        {
            ILRuntimeCLRBinding.GenerateCLRBindingByAnalysis();
        }
    }
}