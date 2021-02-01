using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Fuse.Editor
{
    public class UICodeGenerateHelper : ICodeGenerateHelper
    {
        public void GenerateCode(CompCollector collector)
        {
            string modName = SceneManager.GetActiveScene().name;
            string uiName  = collector.gameObject.name;
            CreateUI(modName, uiName);
            CreateUIView(modName, uiName, collector);
        }

        #region 生成代码

        private void CreateUI(string modName, string uiName)
        {
            string outPath = Path.GetFullPath($"Assets/Scripts/Hotfix/Module/{modName}/UI/{uiName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getUICodeStr(modName, uiName), false);
            AssetDatabase.Refresh();
        }

        private void CreateUIView(string modName, string uiName, CompCollector collector)
        {
            string outPath = Path.GetFullPath($"Assets/Scripts/Hotfix/Module/{modName}/UI/View/{uiName}View.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getUIViewCodeStr(modName, uiName, collector));
            AssetDatabase.Refresh();
        }

        private string getUICodeStr(string modName, string uiName)
        {
            StringBuilder codeStr = new StringBuilder();
            //命名空间
            codeStr.AppendLine("namespace Fuse.Hotfix." + modName);
            codeStr.AppendLine("{");

            //类名
            codeStr.AppendLine($"\tpublic partial class {uiName} : BaseUI");
            codeStr.AppendLine("\t{");

            codeStr.AppendLine($"\t\tpublic {uiName}()");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t\tUIGroup = EUIGroup.Default;");
            codeStr.AppendLine("\t\t}");
            codeStr.AppendLine();

            codeStr.AppendLine($"\t\tprotected override void Init(object userdata)");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t}");
            codeStr.AppendLine();

            codeStr.AppendLine($"\t\tprotected override void Awake(object userdata)");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t}");
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        private string getUIViewCodeStr(string modName, string uiName, CompCollector collector)
        {
            StringBuilder codeStr = new StringBuilder();

            //引用
            codeStr.AppendLine("//工具生成不要修改");
            codeStr.AppendLine($"//生成时间：{DateTime.Now}");
            codeStr.AppendLine("using UnityEngine;");
            codeStr.AppendLine("using UnityEngine.UI;");
            codeStr.AppendLine("");

            //命名空间
            codeStr.AppendLine("namespace Fuse.Hotfix." + modName);
            codeStr.AppendLine("{");

            //类名
            codeStr.AppendLine($"\tpublic partial class {uiName}");
            codeStr.AppendLine("\t{");

            StringBuilder compStr = new StringBuilder();

            string objType;
            //组件
            foreach (var variable in collector.CompCollectorInfos)
            {
                objType = getTypeName(variable.ComponentType);
                codeStr.AppendLine($"\t\tprivate {objType} {variable.Name};");
                if (objType == "GameObject")
                {
                    compStr.AppendLine($"\t\t\t{variable.Name} = Get(\"{variable.Name}\");");
                }
                else
                {
                    compStr.AppendLine($"\t\t\t{variable.Name} = Get<{objType}>(\"{variable.Name}\");");
                }
            }

            codeStr.AppendLine("\t\t/// <summary>初始化UI控件</summary>");
            codeStr.AppendLine("\t\tprotected override void InitializeComponent()");
            codeStr.AppendLine("\t\t{");

            codeStr.Append(compStr);

            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t}");
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        #endregion

        private string getTypeName(string fullName)
        {
            return fullName.Substring(fullName.LastIndexOf(".") + 1);
        }
    }
}