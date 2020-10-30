using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Fuse.Editor
{
    public class CodeGeneratorEditor : EditorWindow
    {
        private string path_procedure  = "Assets/Scripts/Hotfix/Manager/Procedure";
        private string input_procedure = string.Empty;

        public static void ShowWindow()
        {
            CodeGeneratorEditor myWindow =
                (CodeGeneratorEditor) GetWindow(typeof(CodeGeneratorEditor), true, "代码模板生成工具", true); //创建窗口
            myWindow.minSize = new Vector2(425, 540);
            myWindow.maxSize = new Vector2(425, 540);
            myWindow.Show(); //展示
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ProcedureHotfix_", GUILayout.Width(98f));
                input_procedure = EditorGUILayout.TextField(new GUIContent(""), input_procedure, GUILayout.Width(200));
                if (string.IsNullOrEmpty(input_procedure)) GUI.enabled = false;

                if (GUILayout.Button("生成"))
                {
                    CreateProcedureCode();
                }

                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }

        private void CreateProcedureCode()
        {
            if (string.IsNullOrEmpty(input_procedure)) return;

            string outPath = Path.GetFullPath($"{path_procedure}/ProcedureHotfix_{input_procedure}.cs");
            
            StringBuilder codeStr = new StringBuilder();
            codeStr.AppendLine("using ProcedureOwner = Fuse.Hotfix.IFsm;");
            codeStr.AppendLine();

            //命名空间
            codeStr.AppendLine("namespace Fuse.Hotfix");
            codeStr.AppendLine("{");
            {
                codeStr.AppendLine($"\tpublic class ProcedureHotfix_{input_procedure} : ProcedureBase");
                codeStr.AppendLine("\t{");
                {
                    codeStr.AppendLine("\t\tprotected internal override void OnEnter(ProcedureOwner procedureOwner)");
                    codeStr.AppendLine("\t\t{");
                    codeStr.AppendLine("\t\t\tbase.OnEnter(procedureOwner);");
                    codeStr.AppendLine("\t\t}");

                    codeStr.AppendLine("\t\tprotected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,");
                    codeStr.AppendLine("\t\t                                          float          realElapseSeconds)");
                    codeStr.AppendLine("\t\t{");
                    codeStr.AppendLine("\t\t\tbase.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);");
                    codeStr.AppendLine("\t\t}");

                    codeStr.AppendLine("\t\tprotected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)");
                    codeStr.AppendLine("\t\t{");
                    codeStr.AppendLine("\t\t\tbase.OnLeave(procedureOwner, isShutdown);");
                    codeStr.AppendLine("\t\t}");

                }
                codeStr.AppendLine("\t}");
            }
            codeStr.AppendLine("}");
      
            CreateFile(outPath, codeStr.ToString());
        }

        private void CreateFile(string outPath,string content)
        {
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, content, false);
            AssetDatabase.Refresh();
        }
    }
}