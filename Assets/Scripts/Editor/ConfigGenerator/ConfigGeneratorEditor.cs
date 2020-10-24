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
                (ConfigGeneratorEditor) EditorWindow.GetWindow(typeof(ConfigGeneratorEditor), true, "ExcelתConfig����",
                                                               true); //��������
            myWindow.minSize = new Vector2(425, 540);
            myWindow.maxSize = new Vector2(425, 540);
            myWindow.Show(); //չʾ
        }

        private       string ExcelPath           = string.Empty;
        private const string NameSpace           = "Fuse.Hotfix";
        private const string ConfigOutPutPath    = "Assets/Res/BundleRes/Data/Config";
        private const string ScriptOutPutPath    = "Assets/Scripts/Hotfix/Manager/Config/Configs";
        private const string ScriptMgrOutPutPath = "Assets/Scripts/Hotfix/Manager/Config";
        private const string LanguageOutPutPath  = "Assets/Res/BundleRes/Data/Localization";

        private Vector2 scrollPos;
        private bool    selectAll   = true;
        private bool    isCreateNew = false;

        private List<string> nameList = new List<string>();
        private List<bool>   selList  = new List<bool>();

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
                EditorGUILayout.LabelField("Excel���·��: ", GUILayout.Width(85f));
                ExcelPath = EditorGUILayout.TextField(ExcelPath);
                if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                {
                    BrowseOutputDirectory();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField($"Config���·��: {ConfigOutPutPath}");
            EditorGUILayout.LabelField($"  Class���·��: {ScriptOutPutPath}");

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                int toggWidth = selectAll ? 100 : 380;
                var lodSelect = selectAll;
                selectAll = EditorGUILayout.ToggleLeft("ȫѡ/ȡ��ȫѡ", selectAll, GUILayout.Width(toggWidth));

                if (lodSelect != selectAll) SelectAll();
                int selNum = selList.FindAll(s => s).Count;
                if (selNum      == nameList.Count) selectAll = true;
                else if (selNum == 0) selectAll              = false;

                if (selectAll)
                {
                    isCreateNew = EditorGUILayout.ToggleLeft("New/Cover", isCreateNew, GUILayout.Width(280));
                }

                EditorGUILayout.LabelField($"{selNum}/{nameList.Count}");
            }
            EditorGUILayout.EndHorizontal();

            GUI.Box(new Rect(5, 90, 415, 402), "");

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(420),
                                                        GUILayout.Height(400));
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

            //����
            if (allNum > 0)
            {
                if (EditorUtility.DisplayCancelableProgressBar("��ǰ����", $"{completedNum}/{allNum}",
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
            selList  = new List<bool>();
            DirectoryInfo folder = new DirectoryInfo(ExcelPath);
            foreach (FileInfo file in folder.GetFiles("*.xlsx"))
            {
                if (file.Name.StartsWith("~$") || file.Name.StartsWith("_"))
                    continue;
                nameList.Add(file.Name);
                selList.Add(true);
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
                selList[index] =
                    EditorGUILayout.ToggleLeft($" {index + 1}.{nameList[index]}", selList[index], GUILayout.Width(360));
                if (GUILayout.Button("...", GUILayout.Width(25)))
                {
                    ToolsHelper.ExecuteCommand($"start {ExcelPath}/{nameList[index]}");
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SelectAll()
        {
            for (int i = 0; i < selList.Count; i++)
            {
                selList[i] = selectAll;
            }
        }

        private void OutPut()
        {
            ToolsHelper.ClearConsole();
            completedNum = 0;
            List<string[]> selFileList = new List<string[]>();
            for (var index = 0; index < selList.Count; index++)
            {
                var variable = selList[index];
                if (variable) selFileList.Add(new[] {nameList[index], $"{ExcelPath}/{nameList[index]}"});
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
            ExcelExportOutSetting.IsCreateNew     = isCreateNew;

            List<ExcelSheet> clientSheet =
                ExcelExportParse.ExcleSheetFilter(ExcelExportParse.GetExcleSheet(fileList), 'C');
            allNum = clientSheet.Count;

            List<Task> list = new List<Task>();
            Debug.Log($"��ʼ����Config��Class ���� : {clientSheet.Count}");
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
                Debug.Log($"Config�������");
                AssetDatabase.Refresh();
            }));
        }
    }
}