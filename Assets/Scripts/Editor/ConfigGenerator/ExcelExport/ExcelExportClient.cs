using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using UnityEngine;

namespace ExcelExport
{
    public class ExcelExportClient
    {
        public static void GenerateFile(ExcelSheet sheet, Action callback = null)
        {
            if (ExcelExportOutSetting.IsCreateNew)
            {
                ExcelUtil.ResetDirectory(ExcelExportOutSetting.OutDataDir);
                ExcelUtil.ResetDirectory(ExcelExportOutSetting.OutClassDir);
            }
            else
            {
                ExcelUtil.CheckCreateDirectory(ExcelExportOutSetting.OutDataDir);
                ExcelUtil.CheckCreateDirectory(ExcelExportOutSetting.OutClassDir);
            }

            CreateConfigClass(sheet);
            CreateConfigData(sheet);
            callback?.Invoke();
        }

        private static void CreateConfigData(ExcelSheet sheet)
        {
            string savePath = $"{ExcelExportOutSetting.OutDataDir}/{sheet.ConfigName}.txt";
            string jsondata = JsonConvert.SerializeObject(sheet.Table);
            ExcelUtil.SaveFile(savePath, jsondata, true, false);
        }

        private static void CreateConfigClass(ExcelSheet sheet)
        {
            string        firstField = null;
            string        savePath   = $"{ExcelExportOutSetting.OutClassDir}/{sheet.ConfigName}.cs";
            StringBuilder fieldStrs  = new StringBuilder();

            fieldStrs.AppendLine("//------------------------------------------------------------");
            fieldStrs.AppendLine("// 此文件由工具自动生成，请勿直接修改。");
            fieldStrs.AppendLine("// 生成时间：" + DateTime.Now);
            fieldStrs.AppendLine("//------------------------------------------------------------");
            fieldStrs.AppendLine();
            fieldStrs.AppendLine("using System;");
            fieldStrs.AppendLine("using System.Collections.Generic;");
            fieldStrs.AppendLine();
            fieldStrs.AppendLine("namespace " + ExcelExportOutSetting.NameSpace);
            {
                fieldStrs.AppendLine("{");
                fieldStrs.AppendLine($"\t/// <summary>{sheet.NameDes}</summary>");
                fieldStrs.AppendLine($"\tpublic class {sheet.ConfigName} : BaseConfig");
                {
                    fieldStrs.AppendLine("\t{");
                    fieldStrs.AppendLine("\t\t/// <summary>唯一ID</summary>");
                    if (!sheet.IsVert)
                    {
                        fieldStrs.AppendLine("\t\tpublic override object UniqueID => id;");
                    }
                    else
                    {
                        fieldStrs.AppendLine($"\t\tpublic override object UniqueID => {sheet.Fields[0].Name};");
                    }


                    foreach (ExcelSheetTableField filed in sheet.Fields)
                    {
                        bool flag      = filed.IsInterface;
                        var  filedProp = flag ? " { get; set; }" : ";";

                        bool flag2 = firstField == null;
                        if (flag2)
                        {
                            firstField = filed.Name;
                        }

                        fieldStrs.AppendLine();
                        fieldStrs.AppendLine("\t\t/// <summary>");
                        string[] desStrings = filed.Des.Split('\n');
                        foreach (var variable in desStrings) fieldStrs.AppendLine($"\t\t/// {variable}");
                        fieldStrs.AppendLine("\t\t/// </summary>");
                        fieldStrs.AppendLine(
                            $"\t\tpublic {ExcelUtil.GetCSStringType(filed.Type, false)} {filed.Name}{filedProp}");
                    }

                    fieldStrs.AppendLine("\t}");
                }
                fieldStrs.AppendLine("}");
            }
            ExcelUtil.SaveFile(savePath, fieldStrs.ToString(), true, true);
        }

        public static void CreateConfigMgr(List<ExcelSheet> configList, Action callback = null)
        {
            ExcelUtil.CheckCreateDirectory(ExcelExportOutSetting.OutConfigMgrDir);
            string savePath = $"{ExcelExportOutSetting.OutConfigMgrDir}/ConfigMgr.cs";
            
            StringBuilder dicStrs_V = new StringBuilder();
            StringBuilder funStrs_H = new StringBuilder();
            StringBuilder funStrs_V = new StringBuilder();

            foreach (var sheet in configList)
            {
                if (sheet.Name.Equals("Language")) continue;
                if (sheet.IsVert)
                {
                    dicStrs_V.AppendLine($"\t\t/// <summary> {sheet.NameDes} </summary>");
                    dicStrs_V.AppendLine($"\t\tpublic {sheet.ConfigName} {ExcelUtil.ToFirstLower(sheet.ConfigName)};");
                    funStrs_V.AppendLine(
                        $"\t\t\t{ExcelUtil.ToFirstLower(sheet.ConfigName)} = await readConfigV<{sheet.ConfigName}>();");
                }
                else
                {
                    funStrs_H.AppendLine($"\t\t\treadConfig<{sheet.Name}Config>().Run();//{sheet.NameDes}");
                }
            }

            StringBuilder fieldStrs = new StringBuilder();
            fieldStrs.AppendLine("//------------------------------------------------------------");
            fieldStrs.AppendLine("// 此文件由工具自动生成，请勿直接修改。");
            fieldStrs.AppendLine("// 生成时间：" + DateTime.Now);
            fieldStrs.AppendLine("//------------------------------------------------------------");
            fieldStrs.AppendLine();
            fieldStrs.AppendLine("using System.Collections.Generic;");
            fieldStrs.AppendLine("using Fuse.Tasks;");
            fieldStrs.AppendLine();
            fieldStrs.AppendLine("namespace " + ExcelExportOutSetting.NameSpace);
            fieldStrs.AppendLine("{");
            {
                fieldStrs.AppendLine("\tpublic partial class ConfigMgr");
                fieldStrs.AppendLine("\t{");
                {
                    fieldStrs.AppendLine(dicStrs_V.ToString());
                    fieldStrs.AppendLine();

                    fieldStrs.AppendLine("\t\tprivate void LoadAllConfig()");
                    fieldStrs.AppendLine("\t\t{");
                    {
                        fieldStrs.AppendLine(funStrs_H.ToString());
                        fieldStrs.AppendLine();
                        fieldStrs.AppendLine("\t\t\t//读取竖表配置");
                        fieldStrs.AppendLine("\t\t\tLoadAllConfigV().Run();");
                        fieldStrs.AppendLine();
                        fieldStrs.AppendLine("\t\t\tCustomRead();");
                    }
                    fieldStrs.AppendLine("\t\t}");
                    fieldStrs.AppendLine("\t\t/// <summary>读取竖表配置</summary>");
                    fieldStrs.AppendLine("\t\tprivate async CTask LoadAllConfigV()");
                    fieldStrs.AppendLine("\t\t{");
                    {
                        fieldStrs.AppendLine(funStrs_V.ToString());
                    }
                    fieldStrs.AppendLine("\t\t}");
                }
                fieldStrs.AppendLine("\t}");
            }
            fieldStrs.AppendLine("}");
            ExcelUtil.SaveFile(savePath, fieldStrs.ToString(), true, true);

            callback?.Invoke();
        }

        public static void CreateLangXml(ExcelSheet sheet)
        {
            if (ExcelExportOutSetting.IsCreateNew)
                ExcelUtil.ResetDirectory(ExcelExportOutSetting.OutLanguageDir);
            else
                ExcelUtil.CheckCreateDirectory(ExcelExportOutSetting.OutLanguageDir);

            for (int i = 1; i < sheet.Fields.Count; i++)
            {
                ExcelSheetTableField excelSheetTableField = sheet.Fields[i];
                CreatXml(excelSheetTableField.Name, sheet);
            }
        }

        private static void CreatXml(string fileName, ExcelSheet sheet)
        {
            var         localPath = ExcelExportOutSetting.OutLanguageDir + "/" + fileName+ ".xml";
            XmlDocument xml       = new XmlDocument();
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", "");        //设置xml文件编码格式为UTF-8
            xml.AppendChild(xmlDeclaration);

            XmlElement root = xml.CreateElement("Dictionaries"); //创建根节点
            //创建子节点
            XmlElement element = xml.CreateElement("Dictionary");
            //设置节点的属性
            element.SetAttribute("Language", fileName);

            foreach (DataRow variable in sheet.Table.Rows)
            {
                XmlElement element2 = xml.CreateElement("String");
                element2.SetAttribute("Key", variable[0].ToString());
                element2.SetAttribute("Value", variable[fileName].ToString());
                element.AppendChild(element2);
            }
            //把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
            root.AppendChild(element);
            xml.AppendChild(root);
            xml.Save(localPath); //保存xml到路径位置

            //Debug.Log("创建XML成功！");
        }
    }
}