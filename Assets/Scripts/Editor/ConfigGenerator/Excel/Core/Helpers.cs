namespace Excel.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class Helpers
    {
        private static Regex re = new Regex("_x([0-9A-F]{4,4})_");

        public static void AddColumnHandleDuplicate(DataTable table, string columnName)
        {
            string str = columnName;
            DataColumn column = table.Columns[columnName];
            for (int i = 1; column != null; i++)
            {
                str = string.Format("{0}_{1}", columnName, i);
                column = table.Columns[str];
            }
            table.Columns.Add(str, typeof(object));
        }

        public static string ConvertEscapeChars(string input)
        {
            return re.Replace(input, m => ((char) uint.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString());
        }

        public static object ConvertFromOATime(double value)
        {
            if ((value >= 0.0) && (value < 60.0))
            {
                value++;
            }
            return DateTime.FromOADate(value);
        }

        internal static void FixDataTypes(DataSet dataset)
        {
            List<DataTable> list = new List<DataTable>(dataset.Tables.Count);
            bool flag = false;
            foreach (DataTable table in dataset.Tables)
            {
                if (table.Rows.Count == 0)
                {
                    list.Add(table);
                }
                else
                {
                    DataTable item = null;
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        Type type = null;
                        foreach (DataRow row in table.Rows)
                        {
                            if (!row.IsNull(i))
                            {
                                Type type2 = row[i].GetType();
                                if (type2 != type)
                                {
                                    if (type == null)
                                    {
                                        type = type2;
                                    }
                                    else
                                    {
                                        type = null;
                                        break;
                                    }
                                }
                            }
                        }
                        if (type != null)
                        {
                            flag = true;
                            if (item == null)
                            {
                                item = table.Clone();
                            }
                            item.Columns[i].DataType = type;
                        }
                    }
                    if (item != null)
                    {
                        item.BeginLoadData();
                        foreach (DataRow row2 in table.Rows)
                        {
                            item.ImportRow(row2);
                        }
                        item.EndLoadData();
                        list.Add(item);
                    }
                    else
                    {
                        list.Add(table);
                    }
                }
            }
            if (flag)
            {
                dataset.Tables.Clear();
                dataset.Tables.AddRange(list.ToArray());
            }
        }

        public static double Int64BitsToDouble(long value)
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(value), 0);
        }

        public static bool IsSingleByteEncoding(Encoding encoding)
        {
            return encoding.IsSingleByte;
        }
    }
}

