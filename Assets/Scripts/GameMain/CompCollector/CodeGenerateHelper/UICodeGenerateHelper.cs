using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Fuse
{
    public class UICodeGenerateHelper : ICodeGenerateHelper
    {
        public void GenerateCode(CompCollector collector)
        {
            string modName = SceneManager.GetActiveScene().name;
            string uiName  = collector.gameObject.name;
            CreateUIView(modName, uiName, collector);
        }

        #region 生成代码

        private void CreateUI(string modName, string uiName, CompCollector collector)
        {
            string outPath = Path.GetFullPath($"Assets/Scripts/Hotfix/Module/{modName}/UI/{uiName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getUICodeStr(uiName, collector), false);
            AssetDatabase.Refresh();
        }

        private void CreateUIView(string modName, string uiName, CompCollector collector)
        {
            string outPath = Path.GetFullPath($"Assets/Scripts/Hotfix/Module/{modName}/UI/View/{uiName}View.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getUIViewCodeStr(uiName, collector));
            AssetDatabase.Refresh();
        }

        private static string getTypeName(string fullName)
        {
            return fullName.Substring(fullName.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }

        private string getUICodeStr(string uiName, CompCollector collector)
        {
            StringBuilder codeStr = new StringBuilder();

            return codeStr.ToString();
        }

        private string getUIViewCodeStr(string uiName, CompCollector collector)
        {
            StringBuilder codeStr = new StringBuilder();

            //引用
            codeStr.AppendLine("//工具生成不要修改");
            codeStr.AppendLine($"//生成时间：{DateTime.Now}");
            codeStr.AppendLine("using UnityEngine;");
            codeStr.AppendLine("using UnityEngine.UI;");
            codeStr.AppendLine("");

            //命名空间
            codeStr.AppendLine("namespace Fuse.Hotfix");
            codeStr.AppendLine("{");

            //类名
            codeStr.AppendLine($"\tpublic partial class {uiName} ");
            codeStr.AppendLine("\t{");

            //组件
            foreach (var variable in collector.CompCollectorInfos)
            {
                codeStr.AppendLine($"\t\tprivate {variable.ComponentType} {variable.Name};");
            }


            codeStr.AppendLine("\t}");
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        #endregion
    }
}