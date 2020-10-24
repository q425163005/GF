namespace Excel
{
    using Excel.Core;
    using Excel.Core.OpenXmlFormat;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    public class ExcelOpenXmlReader : IExcelDataReader, IDataReader, IDisposable, IDataRecord
    {
        private object[] _cellsValues;
        private List<int> _defaultDateTimeStyles = new List<int>(new int[] { 14, 15, 0x10, 0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x2d, 0x2e, 0x2f });
        private int _depth;
        private int _emptyRowCount;
        private string _exceptionMessage;
        private bool _isClosed;
        private bool _isFirstRead = true;
        private bool _isFirstRowAsColumnNames;
        private bool _isValid = true;
        private int _resultIndex;
        private object[] _savedCellsValues;
        private Stream _sheetStream;
        private XlsxWorkbook _workbook;
        private XmlReader _xmlReader;
        private ZipWorker _zipWorker;
        private const string COLUMN = "Column";
        private bool disposed;
        private string instanceId = Guid.NewGuid().ToString();

        internal ExcelOpenXmlReader()
        {
        }

        public DataSet AsDataSet()
        {
            return this.AsDataSet(true);
        }

        public DataSet AsDataSet(bool convertOADateTime)
        {
            if (!this._isValid)
            {
                return null;
            }
            DataSet dataset = new DataSet();
            for (int i = 0; i < this._workbook.Sheets.Count; i++)
            {
                DataTable table = new DataTable(this._workbook.Sheets[i].Name);
                this.ReadSheetGlobals(this._workbook.Sheets[i]);
                if (this._workbook.Sheets[i].Dimension != null)
                {
                    this._depth = 0;
                    this._emptyRowCount = 0;
                    if (!this._isFirstRowAsColumnNames)
                    {
                        for (int j = 0; j < this._workbook.Sheets[i].ColumnsCount; j++)
                        {
                            table.Columns.Add(null, typeof(object));
                        }
                    }
                    else
                    {
                        if (!this.ReadSheetRow(this._workbook.Sheets[i]))
                        {
                            continue;
                        }
                        for (int k = 0; k < this._cellsValues.Length; k++)
                        {
                            if ((this._cellsValues[k] != null) && (this._cellsValues[k].ToString().Length > 0))
                            {
                                Helpers.AddColumnHandleDuplicate(table, this._cellsValues[k].ToString());
                            }
                            else
                            {
                                Helpers.AddColumnHandleDuplicate(table, "Column" + k);
                            }
                        }
                    }
                    table.BeginLoadData();
                    while (this.ReadSheetRow(this._workbook.Sheets[i]))
                    {
                        table.Rows.Add(this._cellsValues);
                    }
                    if (table.Rows.Count > 0)
                    {
                        dataset.Tables.Add(table);
                    }
                    table.EndLoadData();
                }
            }
            dataset.AcceptChanges();
            Helpers.FixDataTypes(dataset);
            return dataset;
        }

        private void CheckDateTimeNumFmts(List<XlsxNumFmt> list)
        {
            if (list.Count != 0)
            {
                foreach (XlsxNumFmt fmt in list)
                {
                    if (!string.IsNullOrEmpty(fmt.FormatCode))
                    {
                        int num;
                        string str = fmt.FormatCode.ToLower();
                        while ((num = str.IndexOf('"')) > 0)
                        {
                            int index = str.IndexOf('"', num + 1);
                            if (index > 0)
                            {
                                str = str.Remove(num, (index - num) + 1);
                            }
                        }
                        FormatReader reader = new FormatReader {
                            FormatString = str
                        };
                        if (reader.IsDateFormatString())
                        {
                            this._defaultDateTimeStyles.Add(fmt.Id);
                        }
                    }
                }
            }
        }

        public void Close()
        {
            this._isClosed = true;
            if (this._xmlReader != null)
            {
                this._xmlReader.Close();
            }
            if (this._sheetStream != null)
            {
                this._sheetStream.Close();
            }
            if (this._zipWorker != null)
            {
                this._zipWorker.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this._xmlReader != null)
                    {
                        this._xmlReader.Dispose();
                    }
                    if (this._sheetStream != null)
                    {
                        this._sheetStream.Dispose();
                    }
                    if (this._zipWorker != null)
                    {
                        this._zipWorker.Dispose();
                    }
                }
                this._zipWorker = null;
                this._xmlReader = null;
                this._sheetStream = null;
                this._workbook = null;
                this._cellsValues = null;
                this._savedCellsValues = null;
                this.disposed = true;
            }
        }

        ~ExcelOpenXmlReader()
        {
            this.Dispose(false);
        }

        public bool GetBoolean(int i)
        {
            if (this.IsDBNull(i))
            {
                return false;
            }
            return bool.Parse(this._cellsValues[i].ToString());
        }

        public byte GetByte(int i)
        {
            throw new NotSupportedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public char GetChar(int i)
        {
            throw new NotSupportedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotSupportedException();
        }

        public DateTime GetDateTime(int i)
        {
            if (this.IsDBNull(i))
            {
                return DateTime.MinValue;
            }
            try
            {
                return (DateTime) this._cellsValues[i];
            }
            catch (InvalidCastException)
            {
                return DateTime.MinValue;
            }
        }

        public decimal GetDecimal(int i)
        {
            if (this.IsDBNull(i))
            {
                return -79228162514264337593543950335M;
            }
            return decimal.Parse(this._cellsValues[i].ToString());
        }

        public double GetDouble(int i)
        {
            if (this.IsDBNull(i))
            {
                return double.MinValue;
            }
            return double.Parse(this._cellsValues[i].ToString());
        }

        public Type GetFieldType(int i)
        {
            throw new NotSupportedException();
        }

        public float GetFloat(int i)
        {
            if (this.IsDBNull(i))
            {
                return float.MinValue;
            }
            return float.Parse(this._cellsValues[i].ToString());
        }

        public Guid GetGuid(int i)
        {
            throw new NotSupportedException();
        }

        public short GetInt16(int i)
        {
            if (this.IsDBNull(i))
            {
                return -32768;
            }
            return short.Parse(this._cellsValues[i].ToString());
        }

        public int GetInt32(int i)
        {
            if (this.IsDBNull(i))
            {
                return -2147483648;
            }
            return int.Parse(this._cellsValues[i].ToString());
        }

        public long GetInt64(int i)
        {
            if (this.IsDBNull(i))
            {
                return -9223372036854775808L;
            }
            return long.Parse(this._cellsValues[i].ToString());
        }

        public string GetName(int i)
        {
            throw new NotSupportedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotSupportedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotSupportedException();
        }

        public string GetString(int i)
        {
            if (this.IsDBNull(i))
            {
                return null;
            }
            return this._cellsValues[i].ToString();
        }

        public object GetValue(int i)
        {
            return this._cellsValues[i];
        }

        public int GetValues(object[] values)
        {
            throw new NotSupportedException();
        }

        public void Initialize(Stream fileStream)
        {
            this._zipWorker = new ZipWorker();
            this._zipWorker.Extract(fileStream);
            if (!this._zipWorker.IsValid)
            {
                this._isValid = false;
                this._exceptionMessage = this._zipWorker.ExceptionMessage;
                this.Close();
            }
            else
            {
                this.ReadGlobals();
            }
        }

        private bool InitializeSheetRead()
        {
            if (this.ResultsCount <= 0)
            {
                return false;
            }
            this.ReadSheetGlobals(this._workbook.Sheets[this._resultIndex]);
            if (this._workbook.Sheets[this._resultIndex].Dimension == null)
            {
                return false;
            }
            this._isFirstRead = false;
            this._depth = 0;
            this._emptyRowCount = 0;
            return true;
        }

        private bool IsDateTimeStyle(int styleId)
        {
            return this._defaultDateTimeStyles.Contains(styleId);
        }

        public bool IsDBNull(int i)
        {
            if (this._cellsValues[i] != null)
            {
                return (DBNull.Value == this._cellsValues[i]);
            }
            return true;
        }

        public bool NextResult()
        {
            if (this._resultIndex >= (this.ResultsCount - 1))
            {
                return false;
            }
            this._resultIndex++;
            this._isFirstRead = true;
            return true;
        }

        public bool Read()
        {
            if (!this._isValid)
            {
                return false;
            }
            if (this._isFirstRead && !this.InitializeSheetRead())
            {
                return false;
            }
            return this.ReadSheetRow(this._workbook.Sheets[this._resultIndex]);
        }

        private void ReadGlobals()
        {
            this._workbook = new XlsxWorkbook(this._zipWorker.GetWorkbookStream(), this._zipWorker.GetWorkbookRelsStream(), this._zipWorker.GetSharedStringsStream(), this._zipWorker.GetStylesStream());
            this.CheckDateTimeNumFmts(this._workbook.Styles.NumFmts);
        }

        private void ReadSheetGlobals(XlsxWorksheet sheet)
        {
            if (this._xmlReader != null)
            {
                this._xmlReader.Close();
            }
            if (this._sheetStream != null)
            {
                this._sheetStream.Close();
            }
            this._sheetStream = this._zipWorker.GetWorksheetStream(sheet.Path);
            if (this._sheetStream != null)
            {
                this._xmlReader = XmlReader.Create(this._sheetStream);
                while (this._xmlReader.Read())
                {
                    if ((this._xmlReader.NodeType == XmlNodeType.Element) && (this._xmlReader.Name == "dimension"))
                    {
                        string attribute = this._xmlReader.GetAttribute("ref");
                        sheet.Dimension = new XlsxDimension(attribute);
                        break;
                    }
                }
                this._xmlReader.ReadToFollowing("sheetData");
                if (this._xmlReader.IsEmptyElement)
                {
                    sheet.IsEmpty = true;
                }
            }
        }

        private bool ReadSheetRow(XlsxWorksheet sheet)
        {
            if (this._xmlReader == null)
            {
                return false;
            }
            if (this._emptyRowCount != 0)
            {
                this._cellsValues = new object[sheet.ColumnsCount];
                this._emptyRowCount--;
                this._depth++;
                return true;
            }
            if (this._savedCellsValues != null)
            {
                this._cellsValues = this._savedCellsValues;
                this._savedCellsValues = null;
                this._depth++;
                return true;
            }
            if (((this._xmlReader.NodeType != XmlNodeType.Element) || (this._xmlReader.Name != "row")) && !this._xmlReader.ReadToFollowing("row"))
            {
                this._xmlReader.Close();
                if (this._sheetStream != null)
                {
                    this._sheetStream.Close();
                }
                return false;
            }
            this._cellsValues = new object[sheet.ColumnsCount];
            int num = int.Parse(this._xmlReader.GetAttribute("r"));
            if (num != (this._depth + 1))
            {
                this._emptyRowCount = (num - this._depth) - 1;
            }
            bool flag = false;
            string s = string.Empty;
            string attribute = string.Empty;
            int num2 = 0;
            int num3 = 0;
            while (this._xmlReader.Read())
            {
                if (this._xmlReader.Depth == 2)
                {
                    break;
                }
                if (this._xmlReader.NodeType == XmlNodeType.Element)
                {
                    flag = false;
                    if (this._xmlReader.Name == "c")
                    {
                        s = this._xmlReader.GetAttribute("s");
                        attribute = this._xmlReader.GetAttribute("t");
                        XlsxDimension.XlsxDim(this._xmlReader.GetAttribute("r"), out num2, out num3);
                    }
                    else if (this._xmlReader.Name == "v")
                    {
                        flag = true;
                    }
                }
                if ((this._xmlReader.NodeType == XmlNodeType.Text) && flag)
                {
                    double num4;
                    object obj2 = this._xmlReader.Value;
                    if (double.TryParse(obj2.ToString(), out num4))
                    {
                        obj2 = num4;
                    }
                    if ((attribute != null) && (attribute == "s"))
                    {
                        obj2 = Helpers.ConvertEscapeChars(this._workbook.SST[int.Parse(obj2.ToString())]);
                    }
                    else if (attribute == "b")
                    {
                        obj2 = this._xmlReader.Value == "1";
                    }
                    else if (s != null)
                    {
                        XlsxXf xf = this._workbook.Styles.CellXfs[int.Parse(s)];
                        if ((xf.ApplyNumberFormat && (obj2 != null)) && ((obj2.ToString() != string.Empty) && this.IsDateTimeStyle(xf.NumFmtId)))
                        {
                            obj2 = Helpers.ConvertFromOATime(num4);
                        }
                        else if (xf.NumFmtId == 0x31)
                        {
                            obj2 = obj2.ToString();
                        }
                    }
                    if ((num2 - 1) < this._cellsValues.Length)
                    {
                        this._cellsValues[num2 - 1] = obj2;
                    }
                }
            }
            if (this._emptyRowCount > 0)
            {
                this._savedCellsValues = this._cellsValues;
                return this.ReadSheetRow(sheet);
            }
            this._depth++;
            return true;
        }

        public int Depth
        {
            get
            {
                return this._depth;
            }
        }

        public string ExceptionMessage
        {
            get
            {
                return this._exceptionMessage;
            }
        }

        public int FieldCount
        {
            get
            {
                if ((this._resultIndex >= 0) && (this._resultIndex < this.ResultsCount))
                {
                    return this._workbook.Sheets[this._resultIndex].ColumnsCount;
                }
                return -1;
            }
        }

        public bool IsClosed
        {
            get
            {
                return this._isClosed;
            }
        }

        public bool IsFirstRowAsColumnNames
        {
            get
            {
                return this._isFirstRowAsColumnNames;
            }
            set
            {
                this._isFirstRowAsColumnNames = value;
            }
        }

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
        }

        public object this[int i]
        {
            get
            {
                return this._cellsValues[i];
            }
        }

        public object this[string name]
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public string Name
        {
            get
            {
                if ((this._resultIndex >= 0) && (this._resultIndex < this.ResultsCount))
                {
                    return this._workbook.Sheets[this._resultIndex].Name;
                }
                return null;
            }
        }

        public int RecordsAffected
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public int ResultsCount
        {
            get
            {
                if (this._workbook != null)
                {
                    return this._workbook.Sheets.Count;
                }
                return -1;
            }
        }
    }
}

