using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Excel;
using ExcelExport;
using UnityEditor;
using UnityEngine;

namespace Fuse.Editor
{
    public class ConfigGeneratorEditor : EditorWindow
    {
        public static void ShowWindow()
        {
            ConfigGeneratorEditor myWindow =
                (ConfigGeneratorEditor) EditorWindow.GetWindow(typeof(ConfigGeneratorEditor), true, "Excel转Config工具",
                                                               true); //创建窗口
            myWindow.minSize = new Vector2(425, 540);
            myWindow.maxSize = new Vector2(425, 540);
            myWindow.Show(); //展示
        }

        private       string ExcelPath           = string.Empty;
        private const string NameSpace           = "Fuse.Hotfix";
        private const string ConfigOutPutPath    = "Assets/Res/BundleRes/Data/Config";
        private const string ScriptOutPutPath    = "Assets/Scripts/Hotfix/Manager/Config/Configs";
        private const string ScriptMgrOutPutPath = "Assets/Scripts/Hotfix/Manager/Config";
        private const string LanguageOutPutPath  = "Assets/Res/BundleRes/Data/Localization";

        private Vector2 scrollPos;

        private List<string> nameList = new List<string>();

        private static int completedNum;
        private static int allNum;

        private void OnEnable()
        {
            ExcelPath = PlayerPrefs.GetString("ExcelPath");
            GetAllFile();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Excel存放路径: ", GUILayout.Width(85f));
                ExcelPath = EditorGUILayout.TextField(ExcelPath);
                if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                {
                    BrowseOutputDirectory();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField($"Config输出路径: {ConfigOutPutPath}");
            EditorGUILayout.LabelField($"  Class输出路径: {ScriptOutPutPath}");

            EditorGUILayout.Space();
            

            GUI.Box(new Rect(5, 65, 415, 427), "");

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(420),
                                                        GUILayout.Height(425));
            for (int i = 0; i < nameList.Count; i++)
            {
                CreatOneItem(i);
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                if (string.IsNullOrEmpty(ExcelPath) || nameList.Count == 0)
                {
                    GUI.enabled = false;
                }

                if (GUILayout.Button("Excel ==> Config", GUILayout.Height(30)))
                {
                    OutPut();
                }

                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();

            //进度
            if (allNum > 0)
            {
                if (EditorUtility.DisplayCancelableProgressBar("当前进度", $"{completedNum}/{allNum}",
                                                               (float) completedNum / allNum))
                {
                    EditorUtility.ClearProgressBar();
                    completedNum = 0;
                    allNum       = 0;
                }
            }
        }

        private void GetAllFile()
        {
            if (string.IsNullOrEmpty(ExcelPath)) return;

            nameList = new List<string>();
            DirectoryInfo folder = new DirectoryInfo(ExcelPath);
            foreach (FileInfo file in folder.GetFiles("*.xlsx"))
            {
                if (file.Name.StartsWith("~$") || file.Name.StartsWith("_"))
                    continue;
                nameList.Add(file.Name);
            }
        }

        private void BrowseOutputDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Output Directory", ExcelPath, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                ExcelPath = directory;
                PlayerPrefs.SetString("ExcelPath", ExcelPath);
                PlayerPrefs.Save();
                GetAllFile();
            }
        }

        private void CreatOneItem(int index)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField($"  ", GUILayout.Width(5));
                EditorGUILayout.LabelField($" {index + 1}.{nameList[index]}", GUILayout.Width(360));
                if (GUILayout.Button("...", GUILayout.Width(25)))
                {
                    ToolsHelper.ExecuteCommand($"start {ExcelPath}/{nameList[index]}");
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OutPut()
        {
            ToolsHelper.ClearConsole();
            completedNum = 0;
            List<string[]> selFileList = new List<string[]>();
            for (var index = 0; index < nameList.Count; index++)
            {
                selFileList.Add(new[] { nameList[index], $"{ExcelPath}/{nameList[index]}" });
            }

            ProgressWindow(selFileList);
        }


        private void ProgressWindow(List<string[]> fileList)
        {
            ExcelExportOutSetting.NameSpace       = NameSpace;
            ExcelExportOutSetting.OutDataDir      = ConfigOutPutPath;
            ExcelExportOutSetting.OutClassDir     = ScriptOutPutPath;
            ExcelExportOutSetting.OutConfigMgrDir = ScriptMgrOutPutPath;
            ExcelExportOutSetting.OutLanguageDir = LanguageOutPutPath;

            List<ExcelSheet> clientSheet =
                ExcelExportParse.ExcleSheetFilter(ExcelExportParse.GetExcleSheet(fileList), 'C');
            allNum = clientSheet.Count;

            List<Task> list = new List<Task>();
            Debug.Log($"开始导出Config和Class 数量 : {clientSheet.Count}");
            for (int i = 0; i < clientSheet.Count; i++)
            {
                int        index = i;
                ExcelSheet sheet = clientSheet[index];
                Task createFileTask = Task.Factory.StartNew(delegate()
                {
                    if (sheet.Name.Equals("Language"))
                        ExcelExportClient.CreateLangXml(sheet);
                    else
                        ExcelExportClient.GenerateFile(sheet);
                    completedNum++;
                });
                list.Add(createFileTask);
            }

            Task createMgrTask = Task.Factory.StartNew(delegate()
            {
                ExcelExportClient.CreateConfigMgr(clientSheet);
            });
            list.Add(createMgrTask);

            TaskFactory taskFactory = new TaskFactory();
            list.Add(taskFactory.ContinueWhenAll(list.ToArray(), delegate(Task[] tArray)
            {
                completedNum = 0;
                allNum       = 0;
                Debug.Log($"Config生成完毕");
                AssetDatabase.Refresh();
            }));
        }
    }
}