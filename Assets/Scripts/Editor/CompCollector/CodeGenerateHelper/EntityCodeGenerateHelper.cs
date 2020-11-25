using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fuse.Editor
{
    public class EntityCodeGenerateHelper : ICodeGenerateHelper
    {
        private string path_entity = "Assets/Scripts/Hotfix/Library";

        public void GenerateCode(CompCollector collector)
        {
            string entityName = collector.m_EntityName;
            if (string.IsNullOrEmpty(entityName))
            {
                Debug.LogError("Entity����Ϊ�գ�");
                return;
            }
            CreateLogic(entityName);
            CreateLogicView(entityName,collector);
            CreateData(entityName);
        }

        #region ���ɴ���

        private void CreateLogic(string entityName)
        {
            string outPath = Path.GetFullPath($"{path_entity}/Entity/{entityName}/EntityLogic_{entityName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getLogicCodeStr(entityName), false);
            AssetDatabase.Refresh();
        }

        private void CreateLogicView(string entityName, CompCollector collector)
        {
            string outPath = Path.GetFullPath($"{path_entity}/Entity/{entityName}/EntityView_{entityName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getViewCodeStr(entityName, collector));
            AssetDatabase.Refresh();
        }

        private void CreateData(string entityName)
        {
            string outPath = Path.GetFullPath($"{path_entity}/Entity/{entityName}/EntityData_{entityName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);

            ToolsHelper.SaveFile(outPath, getDataCodeStr(entityName),false);
            AssetDatabase.Refresh();
        }

        private string getLogicCodeStr(string entityName)
        {
            StringBuilder codeStr = new StringBuilder();
            //�����ռ�
            codeStr.AppendLine("namespace Fuse.Hotfix");
            codeStr.AppendLine("{");
            {
                codeStr.AppendLine($"\tpublic partial class EntityLogic_{entityName} : BaseEntityLogic");
                codeStr.AppendLine("\t{");
                {
                    codeStr.AppendLine("\t\tprotected override void Refresh(object userdata)");
                    codeStr.AppendLine("\t\t{");
                    codeStr.AppendLine("\t\t\tbase.Refresh(userdata);");
                    codeStr.AppendLine("\t\t}");
                }
                codeStr.AppendLine("\t}");
            }
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        private string getViewCodeStr(string entityName, CompCollector collector)
        {
            StringBuilder codeStr = new StringBuilder();

            //����
            codeStr.AppendLine("//�������ɲ�Ҫ�޸�");
            codeStr.AppendLine($"//����ʱ�䣺{DateTime.Now}");
            codeStr.AppendLine("using UnityEngine;");
            codeStr.AppendLine("");

            //�����ռ�
            codeStr.AppendLine("namespace Fuse.Hotfix");
            codeStr.AppendLine("{");

            //����
            codeStr.AppendLine($"\tpublic partial class EntityLogic_{entityName}");
            codeStr.AppendLine("\t{");

            StringBuilder compStr = new StringBuilder();

            string objType;
            //���
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

            codeStr.AppendLine("\t\t/// <summary>��ʼ��Entity�ؼ�</summary>");
            codeStr.AppendLine("\t\tprotected override void InitializeComponent()");
            codeStr.AppendLine("\t\t{");

            codeStr.Append(compStr);

            codeStr.AppendLine("\t\t}");

            codeStr.AppendLine("\t}");
            codeStr.AppendLine("}");
            return codeStr.ToString();
        }

        private string getDataCodeStr(string entityName)
        {
            StringBuilder codeStr = new StringBuilder();
            //�����ռ�
            codeStr.AppendLine("namespace Fuse.Hotfix");
            codeStr.AppendLine("{");
            {
                codeStr.AppendLine($"\tpublic class EntityData_{entityName} : BaseEntityData");
                codeStr.AppendLine("\t{");
                {
                    codeStr.AppendLine();
                }
                codeStr.AppendLine("\t}");
            }
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