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
            CreateUI(modName, uiName);
            CreateUIView(modName, uiName, collector);
        }

        #region ���ɴ���

        private void CreateUI(string modName, string uiName)
        {
            string outPath = Path.GetFullPath($"Assets/Scripts/Hotfix/Module/{modName}/UI/{uiName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getUICodeStr(uiName), false);
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

        private string getUICodeStr(string uiName)
        {
            StringBuilder codeStr = new StringBuilder();
            //�����ռ�
            codeStr.AppendLine("namespace Fuse.Hotfix");
            codeStr.AppendLine("{");

            //����
            codeStr.AppendLine($"\tpublic partial class {uiName} : BaseUI");
            codeStr.AppendLine("\t{");

            codeStr.AppendLine($"\t\tpublic {uiName}()");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t\tUIGroup = EUIGroup.Default;");
            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine($"\t\tprotected override void Awake()");
            codeStr.AppendLine("\t\t{");
            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t}");
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        private string getUIViewCodeStr(string uiName, CompCollector collector)
        {
            StringBuilder codeStr = new StringBuilder();

            //����
            codeStr.AppendLine("//�������ɲ�Ҫ�޸�");
            codeStr.AppendLine($"//����ʱ�䣺{DateTime.Now}");
            codeStr.AppendLine("using UnityEngine;");
            codeStr.AppendLine("using UnityEngine.UI;");
            codeStr.AppendLine("");

            //�����ռ�
            codeStr.AppendLine("namespace Fuse.Hotfix");
            codeStr.AppendLine("{");

            //����
            codeStr.AppendLine($"\tpublic partial class {uiName} : BaseUI");
            codeStr.AppendLine("\t{");

            //���
            foreach (var variable in collector.CompCollectorInfos)
            {
                codeStr.AppendLine($"\t\tprivate {variable.ComponentType} {variable.Name};");
            }

            codeStr.AppendLine("\t\t/// <summary>��ʼ��UI�ؼ�</summary>");
            codeStr.AppendLine("\t\tprotected override void InitializeComponent()");
            codeStr.AppendLine("\t\t{");
            foreach (var variable in collector.CompCollectorInfos)
            {
                codeStr.AppendLine($"\t\t\t{variable.Name} = Get<{variable.ComponentType}>(\"{variable.Name}\");");
            }
            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t}");
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        #endregion
    }
}