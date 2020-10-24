using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

namespace ExcelExport
{
    public class ExcelExportParse
    {
        public static Dictionary<string, ExcelSheet> GetExcleSheet(List<string[]> fils)
        {
            Dictionary<string, ExcelSheet> dicSheet = new Dictionary<string, ExcelSheet>();
            foreach (string[] file in fils)
            {
                DataSet ds = ExcelUtil.ReadExcelSheetData(file[1]);
                foreach (object obj in ds.Tables)
                {
                    DataTable dt = (DataTable) obj;
                    string[] sheeltInfo = dt.TableName.Split(new char[]
                    {
                        '#'
                    });
                    bool flag2 = sheeltInfo.Length < 2;
                    if (!flag2)
                    {
                        string[] sheeltTypeInfo = sheeltInfo[0].Split(new char[]
                        {
                            '_'
                        });
                        string tableName = ExcelUtil.ToFirstUpper(sheeltTypeInfo[0]);
                        bool   isVert    = false;
                        bool   flag3     = sheeltTypeInfo.Length > 1;
                        if (flag3)
                        {
                            bool flag4 = sheeltTypeInfo[1].ToLower() == "v";
                            if (flag4)
                            {
                                isVert = true;
                            }
                        }

                        ExcelSheet sheet;
                        bool       flag5 = !dicSheet.TryGetValue(tableName, out sheet);
                        if (flag5)
                        {
                            sheet         = new ExcelSheet();
                            sheet.Name    = tableName;
                            sheet.NameDes = sheeltInfo[1];
                            bool flag6 = sheeltInfo.Length > 2;
                            if (flag6)
                            {
                                sheet.Interface = sheeltInfo[2];
                            }

                            sheet.IsVert   = isVert;
                            sheet.FileName = file[0];
                            sheet.Table    = new DataTable();
                            bool flag7 = !ExcelExportParse.setSheetTable(sheet, dt);
                            if (flag7)
                            {
                                return null;
                            }

                            dicSheet.Add(tableName, sheet);
                        }
                        else
                        {
                            bool isVert2 = sheet.IsVert;
                            if (isVert2)
                            {
                                Debug.LogError("表" + sheet.FullName + "是竖表结构,不能做分表");
                                return null;
                            }

                            ExcelSheet nSheet = new ExcelSheet();
                            nSheet.Table = new DataTable();
                            nSheet.Name  = dt.TableName;
                            bool flag8 = !ExcelExportParse.setSheetTable(nSheet, dt);
                            if (flag8)
                            {
                                return null;
                            }

                            bool flag9 = sheet.IsVert != isVert || !ExcelExportParse.checkSheetColumns(sheet, nSheet);
                            if (flag9)
                            {
                                Debug.LogError("表" + sheet.FullName + "定义的表结构不一至,请检查!!!!,目标表:" + dt.TableName);
                                return null;
                            }
                        }

                        ExcelExportParse.addSheetTableRow(sheet, dt);
                    }
                }
            }

            return dicSheet;
        }

        private static bool checkSheetColumns(ExcelSheet sheet, ExcelSheet nSheet)
        {
            int i = 0;
            while (i < sheet.Fields.Count)
            {
                bool flag = sheet.Fields[i].Type != nSheet.Fields[i].Type;
                bool result;
                if (flag)
                {
                    Debug.LogError("表" + sheet.FullName + sheet.Fields[i] + "字段类型不一至,目标表:" + nSheet.Name);
                    result = false;
                }
                else
                {
                    bool flag2 = sheet.Fields[i].Name != nSheet.Fields[i].Name;
                    if (!flag2)
                    {
                        i++;
                        continue;
                    }

                    Debug.LogError("表" + sheet.FullName + sheet.Fields[i] + "字段名不一至,目标表:" + nSheet.Name);
                    result = false;
                }

                return result;
            }

            return true;
        }

        private static bool setSheetTable(ExcelSheet sheet, DataTable dtSource)
        {
            bool flag = dtSource.Columns.Count > 100;
            bool result;
            if (flag)
            {
                Debug.LogError(sheet.FullName + "表列数太多,请检查表格式");
                result = false;
            }
            else
            {
                bool isVert = sheet.IsVert;
                if (isVert)
                {
                    bool flag2 = dtSource.Rows.Count < 1 && dtSource.Columns.Count < 5;
                    if (flag2)
                    {
                        Debug.LogError(sheet.FullName + "表列数不对");
                        result = false;
                    }
                    else
                    {
                        int emptyRow = 0;
                        for (int i = 1; i < dtSource.Rows.Count; i++)
                        {
                            bool flag3 = dtSource.Rows[i][0].ToString() == string.Empty;
                            if (flag3)
                            {
                                emptyRow++;
                                bool flag4 = emptyRow > 3;
                                if (flag4)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                emptyRow = 0;
                                ExcelSheetTableField field = new ExcelSheetTableField();
                                field.Name        = dtSource.Rows[i][0].ToString();
                                field.Export      = dtSource.Rows[i][1].ToString();
                                field.Type        = dtSource.Rows[i][2].ToString().ToLower();
                                field.Value       = dtSource.Rows[i][3].ToString();
                                field.Des         = dtSource.Rows[i][4].ToString();
                                field.IsInterface = (field.Export.IndexOf('I') != -1);
                                sheet.Table.Columns.Add(field.Name, ExcelUtil.GetStringType(field.Type));
                                sheet.Fields.Add(field);
                            }
                        }

                        sheet.Export    = dtSource.Rows[1][1].ToString().ToUpper();
                        sheet.IsEncrypt = (sheet.Export.IndexOf('@') != -1);
                        result          = true;
                    }
                }
                else
                {
                    bool flag5 = dtSource.Rows.Count < 4 && dtSource.Columns.Count < 2;
                    if (flag5)
                    {
                        Debug.LogError(sheet.FullName + "表列数不对");
                        result = false;
                    }
                    else
                    {
                        ExcelSheetTableField field2 = null;
                        for (int j = 0; j < dtSource.Columns.Count; j++)
                        {
                            string[] arrInfo = dtSource.Rows[3][j].ToString().Split(new char[]
                            {
                                '#'
                            });
                            string fieldName = arrInfo[0];
                            bool   flag6     = fieldName == string.Empty;
                            if (flag6)
                            {
                                break;
                            }

                            bool flag7 = field2 == null || field2.Name != fieldName;
                            if (flag7)
                            {
                                field2             = new ExcelSheetTableField();
                                field2.Des         = dtSource.Rows[0][j].ToString();
                                field2.Export      = dtSource.Rows[1][j].ToString();
                                field2.Type        = dtSource.Rows[2][j].ToString().ToLower();
                                field2.Name        = fieldName;
                                field2.IsInterface = (field2.Export.IndexOf('I') != -1);
                                field2.FieldCount  = 1;
                                sheet.Fields.Add(field2);
                                sheet.Table.Columns.Add(field2.Name, ExcelUtil.GetStringType(field2.Type));
                            }
                            else
                            {
                                ExcelSheetTableField excelSheetTableField = field2;
                                excelSheetTableField.Des =
                                    excelSheetTableField.Des + "\n" + dtSource.Rows[0][j].ToString();
                                field2.FieldCount++;
                            }
                        }

                        sheet.Export    = dtSource.Rows[1][0].ToString().ToUpper();
                        sheet.IsEncrypt = (sheet.Export.IndexOf('@') != -1);
                        result          = true;
                    }
                }
            }

            return result;
        }

        private static void addSheetTableRow(ExcelSheet sheet, DataTable dtSource)
        {
            bool isVert = sheet.IsVert;
            if (isVert)
            {
                DataRow expRow = sheet.Table.NewRow();
                for (int c = 0; c < sheet.Fields.Count; c++)
                {
                    ExcelSheetTableField field = sheet.Fields[c];
                    bool                 flag  = field.Export == string.Empty;
                    if (!flag)
                    {
                        object val   = ExcelUtil.GetObjectValue(field.Type, field.Value, 1);
                        bool   flag2 = val == null;
                        if (flag2)
                        {
                            Debug.LogWarning(sheet.FullName + "表," + field.Name + "字段" + field.Type +
                                             "类型未写转换规则，已转换为string类型");
                            val = string.Empty;
                        }

                        expRow[c] = val;
                    }
                }

                sheet.Table.Rows.Add(expRow);
            }
            else
            {
                for (int i = 4; i < dtSource.Rows.Count; i++)
                {
                    DataRow expRow2 = sheet.Table.NewRow();
                    bool    flag3   = string.IsNullOrEmpty(dtSource.Rows[i][0].ToString());
                    if (!flag3)
                    {
                        int cellIndex = 0;
                        for (int f = 0; f < sheet.Fields.Count; f++)
                        {
                            ExcelSheetTableField field2 = sheet.Fields[f];
                            bool                 flag4  = field2.Export == string.Empty;
                            if (flag4)
                            {
                                cellIndex++;
                            }
                            else
                            {
                                string cellVal = dtSource.Rows[i][cellIndex].ToString();
                                cellIndex++;
                                object val2  = ExcelUtil.GetObjectValue(field2.Type, cellVal, field2.FieldCount);
                                bool   flag5 = val2 == null;
                                if (flag5)
                                {
                                    Debug.LogWarning(sheet.FullName + "表," + field2.Name + "字段" + field2.Type +
                                                     "类型未写转换规则，已转换为string类型");
                                    val2 = string.Empty;
                                }

                                bool flag6 = field2.FieldCount > 1;
                                if (flag6)
                                {
                                    for (int j = 1; j < field2.FieldCount; j++)
                                    {
                                        ExcelUtil.AddListValue(field2.Type, dtSource.Rows[i][cellIndex].ToString(),
                                                               ref val2);
                                        cellIndex++;
                                    }
                                }
                                expRow2[f] = val2;
                            }
                        }

                        sheet.Table.Rows.Add(expRow2);
                    }
                }
            }
        }

        public static List<ExcelSheet> ExcleSheetFilter(Dictionary<string, ExcelSheet> dicSheet, char filter)
        {
            List<ExcelSheet>           list     = new List<ExcelSheet>();
            List<string>               colNames = new List<string>();
            List<ExcelSheetTableField> filelds  = new List<ExcelSheetTableField>();
            foreach (ExcelSheet sheet in dicSheet.Values)
            {
                bool flag = sheet.Export.IndexOf(filter) != -1;
                if (flag)
                {
                    colNames.Clear();
                    filelds = new List<ExcelSheetTableField>();
                    foreach (ExcelSheetTableField field in sheet.Fields)
                    {
                        bool flag2 = field.Export.IndexOf(filter) != -1;
                        if (flag2)
                        {
                            colNames.Add(field.Name);
                            filelds.Add(field);
                        }
                    }

                    DataTable filterTable = sheet.Table.DefaultView.ToTable(false, colNames.ToArray());
                    list.Add(new ExcelSheet
                    {
                        Name      = sheet.Name,
                        NameDes   = sheet.NameDes,
                        IsVert    = sheet.IsVert,
                        FileName  = sheet.FileName,
                        IsEncrypt = sheet.IsEncrypt,
                        Fields    = filelds,
                        Table     = filterTable,
                        Interface = sheet.Interface
                    });
                }
            }

            return list;
        }
    }
}