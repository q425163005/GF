using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fuse.Editor
{
    public class UIItemCodeGenerateHelper : ICodeGenerateHelper
    {
        public void GenerateCode(CompCollector collector)
        {
            string modName = SceneManager.GetActiveScene().name;
            string itemName  = collector.gameObject.name;
            if (!itemName.EndsWith("Item"))
            {
                Debug.LogError("命名结尾必须为Item");
                return;
            }

            CreateItem(modName, itemName);
            CreateItemView(modName, itemName, collector);
        }

        #region 生成代码

        private void CreateItem(string modName, string itemName)
        {
            string outPath = Path.GetFullPath($"Assets/Scripts/Hotfix/Module/{modName}/Item/{itemName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getItemCodeStr(modName, itemName), false);
            AssetDatabase.Refresh();
        }

        private void CreateItemView(string modName, string itemName, CompCollector collector)
        {
            string outPath = Path.GetFullPath($"Assets/Scripts/Hotfix/Module/{modName}/Item/View/{itemName}View.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getItemViewCodeStr(modName, itemName, collector));
            AssetDatabase.Refresh();
        }

        private string getItemCodeStr(string modName, string itemName)
        {
            StringBuilder codeStr = new StringBuilder();
            //命名空间
            codeStr.AppendLine("namespace Fuse.Hotfix." + modName);
            codeStr.AppendLine("{");

            //类名
            codeStr.AppendLine($"\tpublic partial class {itemName} : BaseItem");
            codeStr.AppendLine("\t{");

            codeStr.AppendLine("\t\tprotected override void Awake()");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t\tbase.Awake();");
            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t\tpublic void SetData()");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t\tpublic override void Disposed()");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t\tbase.Disposed();");
            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t}");
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        private string getItemViewCodeStr(string modName, string itemName, CompCollector collector)
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
            codeStr.AppendLine($"\tpublic partial class {itemName} : BaseItem");
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