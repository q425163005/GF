using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using Excel;
using UnityEngine;

namespace ExcelExport
{
    // Token: 0x02000028 RID: 40
    public class ExcelUtil
    {
        class Lang
        {
            public string key;
        }

        // Token: 0x060000AC RID: 172 RVA: 0x0000AF38 File Offset: 0x00009138
        public static DataSet ReadExcelSheetData(string filePath)
        {
            Dictionary<string, List<List<string>>> listTabs = new Dictionary<string, List<List<string>>>();
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            return excelReader.AsDataSet();
        }

        // Token: 0x060000AD RID: 173 RVA: 0x0000AF68 File Offset: 0x00009168
        public static bool isExport(string name, bool isServer)
        {
            bool flag = name == string.Empty;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = name.StartsWith("n_");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = isServer && name.StartsWith("c_");
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = !isServer && name.StartsWith("s_");
                        result = !flag4;
                    }
                }
            }
            return result;
        }

        // Token: 0x060000AE RID: 174 RVA: 0x0000AFD8 File Offset: 0x000091D8
        public static List<T> SplitToArr<T>(string str, bool isSetDef = false, char separator = ';')
        {
            bool flag = str == string.Empty || str == null;
            List<T> result;
            if (flag)
            {
                if (isSetDef)
                {
                    result = new List<T>
                    {
                        default(T)
                    };
                }
                else
                {
                    result = new List<T>();
                }
            }
            else
            {
                string[] objarr = str.Split(new char[]
                {
                    separator
                }, StringSplitOptions.RemoveEmptyEntries);
                List<T> tArr = new List<T>();
                for (int i = 0; i < objarr.Length; i++)
                {
                    try
                    {
                        bool flag2 = typeof(T) == typeof(bool);
                        if (flag2)
                        {
                            objarr[i] = ((objarr[i] == "1") ? "true" : objarr[i]);
                        }
                        bool flag3 = typeof(T) == typeof(int);
                        if (flag3)
                        {
                            double floatV;
                            double.TryParse(objarr[i], out floatV);
                            tArr.Add((T)((object)Convert.ChangeType(floatV, typeof(T))));
                        }
                        else
                        {
                            tArr.Add((T)((object)Convert.ChangeType(objarr[i], typeof(T))));
                        }
                    }
                    catch (Exception e)
                    {
                        tArr.Add(default(T));
                        Debug.LogError(e.Message+e.StackTrace);
                    }
                }
                result = tArr;
            }
            return result;
        }

        // Token: 0x060000AF RID: 175 RVA: 0x0000B168 File Offset: 0x00009368
        public static List<int[]> SplitToItems(List<string> itemsStr)
        {
            List<int[]> items = new List<int[]>();
            foreach (string itstr in itemsStr)
            {
                List<int> iteminfo = ExcelUtil.SplitToArr<int>(itstr, false, '_');
                items.Add(iteminfo.ToArray());
            }
            return items;
        }

        // Token: 0x060000B0 RID: 176 RVA: 0x0000B1D8 File Offset: 0x000093D8
        public static object GetObjectValue(string type, string value, int fieldCount = 1)
        {
            bool isSetDef = fieldCount > 1;

            switch (type)
            {
                case "string":
                    return value;
                case "string[]":
                    return ExcelUtil.SplitToArr<string>(value, isSetDef, ';');
                case "byte":
                    byte.TryParse(value, out byte byteVal);
                    return byteVal;
                case "byte[]":
                    return ExcelUtil.SplitToArr<byte>(value, isSetDef, ';');
                case "int":
                    double.TryParse(value, out double floatV);
                    return Convert.ToInt32(floatV);
                case "int[]":
                    return ExcelUtil.SplitToArr<int>(value, isSetDef, ';');
                case "long":
                    long.TryParse(value, out long longVal);
                    return longVal;
                case "long[]":
                    return ExcelUtil.SplitToArr<long>(value, isSetDef, ';');
                case "double":
                    double.TryParse(value, out double doubleVal);
                    return doubleVal;
                case "double[]":
                    return ExcelUtil.SplitToArr<double>(value, isSetDef, ';');
                case "float":
                    float.TryParse(value, out float floatVal);
                    return floatVal;
                case "float[]":
                    return ExcelUtil.SplitToArr<float>(value, isSetDef, ';');
                case "short":
                    double.TryParse(value, out floatV);
                    short shortVal = Convert.ToInt16(floatV);
                    return shortVal;
                case "short[]":
                    return ExcelUtil.SplitToArr<short>(value, isSetDef, ';');
                case "bool":
                    bool flag = value == "1";
                    if (flag)
                    {
                        return true;
                    }
                    bool boolVal = false;
                    bool.TryParse(value, out boolVal);
                    return boolVal;
                case "bool[]":
                    return ExcelUtil.SplitToArr<bool>(value, isSetDef, ';');
                case "list<int[]>":
                    return SplitToItems(SplitToArr<string>(value, false, ';'));
                case "list<list<int[]>>":
                    return new List<List<int[]>>
                    {
                        ExcelUtil.SplitToItems(ExcelUtil.SplitToArr<string>(value, false, ';'))
                    };
                case "lang":
                    return new Lang
                    {
                        key = value
                    };
                default:
                    return null;
            }
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x0000B700 File Offset: 0x00009900
        public static void AddListValue(string type, string value, ref object obj)
        {
            switch (type)
            {
                case "int[]":
                    double.TryParse(value, out double floatV);
                    int intVal = Convert.ToInt32(floatV);
                    (obj as List<int>)?.Add(intVal);
                    return;
                case "string[]":
                    (obj as List<string>)?.Add(value);
                    return;
                case "float[]":
                    float floatVal = 0f;
                    float.TryParse(value, out floatVal);
                    (obj as List<float>)?.Add(floatVal);
                    return;
                case "list<list<int[]>>":
                    (obj as List<List<int[]>>)?.Add(SplitToItems(SplitToArr<string>(value, false, ';')));
                    return;
                case "list<int[]>":
                    (obj as List<int[]>)?.Add(SplitToArr<int>(value, false, '_').ToArray());
                    return;
                case "bool[]":
                    bool boolVal = value != "1";
                    bool.TryParse(value, out boolVal);
                    (obj as List<bool>)?.Add(boolVal);
                    return;
                case "byte[]":
                    byte byteVal = 0;
                    byte.TryParse(value, out byteVal);
                    (obj as List<byte>)?.Add(byteVal);
                    return;
                case "short[]":
                    double.TryParse(value, out floatV);
                    short shortVal = Convert.ToInt16(floatV);
                    (obj as List<short>)?.Add(shortVal);
                    return;
                case "long[]":
                    long longVal = 0L;
                    long.TryParse(value, out longVal);
                    (obj as List<long>)?.Add(longVal);
                    return;
                case "double[]":
                    double doubleVal = 0.0;
                    double.TryParse(value, out doubleVal);
                    (obj as List<double>)?.Add(doubleVal);
                    return;
                default:
                    return;
            }
        }

        // Token: 0x060000B2 RID: 178 RVA: 0x0000BA00 File Offset: 0x00009C00
        public static Type GetStringType(string type)
        {
            switch (type)
            {
                case "string":
                    return typeof(string);
                case "string[]":
                    return typeof(List<string>);
                case "list<list<int[]>>":
                    return typeof(List<List<int[]>>); ;
                case "list<int[]>":
                    return typeof(List<int[]>);
                case "int[]":
                    return typeof(List<int>);
                case "byte":
                    return typeof(byte);
                case "float[]":
                    return typeof(List<float>);
                case "int":
                    return typeof(int);
                case "lang":
                    return typeof(Lang);
                case "byte[]":
                    return typeof(List<byte>);
                case "double":
                    return typeof(double);
                case "bool[]":
                    return typeof(List<bool>);
                case "double[]":
                    return typeof(List<double>);
                case "short":
                    return typeof(short);
                case "float":
                    return typeof(float);
                case "bool":
                    return typeof(bool);
                case "long":
                    return typeof(long);
                case "short[]":
                    return typeof(List<short>);
                case "long[]":
                    return typeof(List<long>);
                default:
                    return typeof(string);
            }
        }

        // Token: 0x060000B3 RID: 179 RVA: 0x0000BE48 File Offset: 0x0000A048
        public static string GetCSStringType(string type, bool isNull = true)
        {
            if (type != null)
            {
                if (type == "date")
                {
                    return isNull ? "DateTime?" : "DateTime";
                }
                if (type == "lang")
                {
                    return "Lang";
                }
                if (type == "list<int[]>")
                {
                    return "List<int[]>";
                }
                if (type == "list<list<int[]>>")
                {
                    return "List<List<int[]>>";
                }
            }
            return type;
        }
        
        public static string ToFirstUpper(string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        
        public static string ToFirstLower(string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static void CheckCreateDirectory(string path)
        {
            bool flag = !Directory.Exists(path);
            if (flag)
            {
                Directory.CreateDirectory(path);
            }
        }
        
        public static void DeleteDirectory(string path)
        {
            bool flag = Directory.Exists(path);
            if (flag)
            {
                Directory.Delete(path, true);
            }
        }
        
        public static void ResetDirectory(string path)
        {
            try
            {
                bool flag = Directory.Exists(path);
                if (flag)
                {
                    Directory.Delete(path, true);
                }
                Thread.Sleep(10);
                Directory.CreateDirectory(path);
            }
            catch
            {
            }
        }

        public static void SaveFile(string path, object content, bool iscover = true, bool isLog = true)
        {
            FileInfo info = new FileInfo(path);
            bool     flag = !iscover && info.Exists;
            if (flag)
            {
                if (isLog)
                {
                    Debug.LogWarning("文件已存在，不进行覆盖操作!! " + path);
                }
            }
            else
            {
                CheckCreateDirectory(info.DirectoryName);
                FileStream fs    = new FileStream(path, FileMode.Create);
                bool       flag2 = content is MemoryStream;
                if (flag2)
                {
                    BinaryWriter w = new BinaryWriter(fs);
                    w.Write(((MemoryStream)content).ToArray());
                    w.Close();
                }
                else
                {
                    bool flag3 = content is byte[];
                    if (flag3)
                    {
                        BinaryWriter w2 = new BinaryWriter(fs);
                        w2.Write((byte[])content);
                        w2.Close();
                    }
                    else
                    {
                        StreamWriter sWriter = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
                        sWriter.WriteLine(content);
                        sWriter.Flush();
                        sWriter.Close();
                    }
                }
                fs.Close();
                if (isLog)
                {
                    Debug.Log("成功生成文件 " + path);
                }
            }
        }
    }
}

