namespace Excel
{
    using Excel.Core;
    using Excel.Core.BinaryFormat;
    using Excel.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ExcelBinaryReader : IExcelDataReader, IDataReader, IDisposable, IDataRecord
    {
        private bool _isFirstRowAsColumnNames;
        private const string BOOK = "Book";
        private const string COLUMN = "Column";
        private bool disposed;
        private bool m_canRead;
        private int m_cellOffset;
        private object[] m_cellsValues;
        private bool m_ConvertOADate;
        private XlsBiffRow m_currentRowRecord;
        private uint[] m_dbCellAddrs;
        private int m_dbCellAddrsIndex;
        private readonly Encoding m_Default_Encoding = Encoding.UTF8;
        private int m_depth;
        private Encoding m_encoding;
        private string m_exceptionMessage;
        private Stream m_file;
        private XlsWorkbookGlobals m_globals;
        private XlsHeader m_hdr;
        private bool m_isClosed;
        private bool m_IsFirstRead;
        private bool m_isValid;
        private int m_maxCol;
        private int m_maxRow;
        private bool m_noIndex;
        private int m_SheetIndex;
        private List<XlsWorksheet> m_sheets;
        private XlsBiffStream m_stream;
        private ushort m_version;
        private DataSet m_workbookData;
        private const string WORKBOOK = "Workbook";

        internal ExcelBinaryReader()
        {
            this.m_encoding = this.m_Default_Encoding;
            this.m_version = 0x600;
            this.m_isValid = true;
            this.m_SheetIndex = -1;
            this.m_IsFirstRead = true;
        }

        public DataSet AsDataSet()
        {
            return this.AsDataSet(false);
        }

        public DataSet AsDataSet(bool convertOADateTime)
        {
            if (!this.m_isValid)
            {
                return null;
            }
            if (!this.m_isClosed)
            {
                this.m_ConvertOADate = convertOADateTime;
                this.m_workbookData = new DataSet();
                for (int i = 0; i < this.ResultsCount; i++)
                {
                    DataTable table = this.readWholeWorkSheet(this.m_sheets[i]);
                    if (table != null)
                    {
                        this.m_workbookData.Tables.Add(table);
                    }
                }
                this.m_file.Close();
                this.m_isClosed = true;
                this.m_workbookData.AcceptChanges();
                Helpers.FixDataTypes(this.m_workbookData);
            }
            return this.m_workbookData;
        }

        public void Close()
        {
            this.m_file.Close();
            this.m_isClosed = true;
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
                    if (this.m_workbookData != null)
                    {
                        this.m_workbookData.Dispose();
                    }
                    if (this.m_sheets != null)
                    {
                        this.m_sheets.Clear();
                    }
                }
                this.m_workbookData = null;
                this.m_sheets = null;
                this.m_stream = null;
                this.m_globals = null;
                this.m_encoding = null;
                this.m_hdr = null;
                this.disposed = true;
            }
        }

        private void DumpBiffRecords()
        {
            XlsBiffRecord record = null;
            int position = this.m_stream.Position;
            do
            {
                record = this.m_stream.Read();
                Console.WriteLine(record.ID.ToString());
            }
            while ((record != null) && (this.m_stream.Position < this.m_stream.Size));
            this.m_stream.Seek(position, SeekOrigin.Begin);
        }

        private void fail(string message)
        {
            this.m_exceptionMessage = message;
            this.m_isValid = false;
            this.m_file.Close();
            this.m_isClosed = true;
            this.m_workbookData = null;
            this.m_sheets = null;
            this.m_stream = null;
            this.m_globals = null;
            this.m_encoding = null;
            this.m_hdr = null;
        }

        ~ExcelBinaryReader()
        {
            this.Dispose(false);
        }

        private int findFirstDataCellOffset(int startOffset)
        {
            XlsBiffRecord record = this.m_stream.ReadAt(startOffset);
            while (!(record is XlsBiffDbCell))
            {
                if (this.m_stream.Position >= this.m_stream.Size)
                {
                    return -1;
                }
                if (record is XlsBiffEOF)
                {
                    return -1;
                }
                record = this.m_stream.Read();
            }
            XlsBiffDbCell cell = (XlsBiffDbCell) record;
            XlsBiffRow row = null;
            int rowAddress = cell.RowAddress;
            do
            {
                row = this.m_stream.ReadAt(rowAddress) as XlsBiffRow;
                if (row == null)
                {
                    return rowAddress;
                }
                rowAddress += row.Size;
            }
            while (row != null);
            return rowAddress;
        }

        public bool GetBoolean(int i)
        {
            if (this.IsDBNull(i))
            {
                return false;
            }
            return bool.Parse(this.m_cellsValues[i].ToString());
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
            double num;
            if (this.IsDBNull(i))
            {
                return DateTime.MinValue;
            }
            string s = this.m_cellsValues[i].ToString();
            try
            {
                num = double.Parse(s);
            }
            catch (FormatException)
            {
                return DateTime.Parse(s);
            }
            return DateTime.FromOADate(num);
        }

        public decimal GetDecimal(int i)
        {
            if (this.IsDBNull(i))
            {
                return -79228162514264337593543950335M;
            }
            return decimal.Parse(this.m_cellsValues[i].ToString());
        }

        public double GetDouble(int i)
        {
            if (this.IsDBNull(i))
            {
                return double.MinValue;
            }
            return double.Parse(this.m_cellsValues[i].ToString());
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
            return float.Parse(this.m_cellsValues[i].ToString());
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
            return short.Parse(this.m_cellsValues[i].ToString());
        }

        public int GetInt32(int i)
        {
            if (this.IsDBNull(i))
            {
                return -2147483648;
            }
            return int.Parse(this.m_cellsValues[i].ToString());
        }

        public long GetInt64(int i)
        {
            if (this.IsDBNull(i))
            {
                return -9223372036854775808L;
            }
            return long.Parse(this.m_cellsValues[i].ToString());
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
            return this.m_cellsValues[i].ToString();
        }

        public object GetValue(int i)
        {
            return this.m_cellsValues[i];
        }

        public int GetValues(object[] values)
        {
            throw new NotSupportedException();
        }

        public void Initialize(Stream fileStream)
        {
            this.m_file = fileStream;
            this.readWorkBookGlobals();
        }

        private void initializeSheetRead()
        {
            if (this.m_SheetIndex != this.ResultsCount)
            {
                XlsBiffIndex index;
                this.m_dbCellAddrs = null;
                this.m_IsFirstRead = false;
                if (this.m_SheetIndex == -1)
                {
                    this.m_SheetIndex = 0;
                }
                if (!this.readWorkSheetGlobals(this.m_sheets[this.m_SheetIndex], out index, out this.m_currentRowRecord))
                {
                    this.m_SheetIndex++;
                    this.initializeSheetRead();
                }
                else if (index == null)
                {
                    this.m_noIndex = true;
                }
                else
                {
                    this.m_dbCellAddrs = index.DbCellAddresses;
                    this.m_dbCellAddrsIndex = 0;
                    this.m_cellOffset = this.findFirstDataCellOffset((int) this.m_dbCellAddrs[this.m_dbCellAddrsIndex]);
                    if (this.m_cellOffset < 0)
                    {
                        this.fail("Badly formed binary file. Has INDEX but no DBCELL");
                    }
                }
            }
        }

        public bool IsDBNull(int i)
        {
            if (this.m_cellsValues[i] != null)
            {
                return (DBNull.Value == this.m_cellsValues[i]);
            }
            return true;
        }

        private bool isV8()
        {
            return (this.m_version >= 0x600);
        }

        private bool moveToNextRecord()
        {
            if (this.m_noIndex)
            {
                return this.moveToNextRecordNoIndex();
            }
            if (((this.m_dbCellAddrs == null) || (this.m_dbCellAddrsIndex == this.m_dbCellAddrs.Length)) || (this.m_depth == this.m_maxRow))
            {
                return false;
            }
            this.m_canRead = this.readWorkSheetRow();
            if (!this.m_canRead && (this.m_depth > 0))
            {
                this.m_canRead = true;
            }
            if (!this.m_canRead && (this.m_dbCellAddrsIndex < (this.m_dbCellAddrs.Length - 1)))
            {
                this.m_dbCellAddrsIndex++;
                this.m_cellOffset = this.findFirstDataCellOffset((int) this.m_dbCellAddrs[this.m_dbCellAddrsIndex]);
                if (this.m_cellOffset < 0)
                {
                    return false;
                }
                this.m_canRead = this.readWorkSheetRow();
            }
            return this.m_canRead;
        }

        private bool moveToNextRecordNoIndex()
        {
            XlsBiffRow currentRowRecord = this.m_currentRowRecord;
            if (currentRowRecord == null)
            {
                return false;
            }
            if (currentRowRecord.RowIndex < this.m_depth)
            {
                this.m_stream.Seek(currentRowRecord.Offset + currentRowRecord.Size, SeekOrigin.Begin);
                do
                {
                    if (this.m_stream.Position >= this.m_stream.Size)
                    {
                        return false;
                    }
                    XlsBiffRecord record = this.m_stream.Read();
                    if (record is XlsBiffEOF)
                    {
                        return false;
                    }
                    currentRowRecord = record as XlsBiffRow;
                }
                while ((currentRowRecord == null) || (currentRowRecord.RowIndex < this.m_depth));
            }
            this.m_currentRowRecord = currentRowRecord;
            XlsBiffBlankCell cell = null;
            do
            {
                if (this.m_stream.Position >= this.m_stream.Size)
                {
                    return false;
                }
                XlsBiffRecord record2 = this.m_stream.Read();
                if (record2 is XlsBiffEOF)
                {
                    return false;
                }
                if (record2.IsCell)
                {
                    XlsBiffBlankCell cell2 = record2 as XlsBiffBlankCell;
                    if ((cell2 != null) && (cell2.RowIndex == this.m_currentRowRecord.RowIndex))
                    {
                        cell = cell2;
                    }
                }
            }
            while (cell == null);
            this.m_cellOffset = cell.Offset;
            this.m_canRead = this.readWorkSheetRow();
            return this.m_canRead;
        }

        public bool NextResult()
        {
            if (this.m_SheetIndex >= (this.ResultsCount - 1))
            {
                return false;
            }
            this.m_SheetIndex++;
            this.m_IsFirstRead = true;
            return true;
        }

        private void pushCellValue(XlsBiffBlankCell cell)
        {
            double num;
            switch (cell.ID)
            {
                case BIFFRECORDTYPE.RK:
                    num = ((XlsBiffRKCell) cell).Value;
                    this.m_cellsValues[cell.ColumnIndex] = !this.m_ConvertOADate ? num : this.tryConvertOADateTime(num, cell.XFormat);
                    return;

                case BIFFRECORDTYPE.FORMULA:
                case BIFFRECORDTYPE.FORMULA_OLD:
                {
                    object obj2 = ((XlsBiffFormulaCell) cell).Value;
                    if ((obj2 != null) && (obj2 is FORMULAERROR))
                    {
                        obj2 = null;
                        return;
                    }
                    this.m_cellsValues[cell.ColumnIndex] = !this.m_ConvertOADate ? obj2 : this.tryConvertOADateTime(obj2, cell.XFormat);
                    break;
                }
                case BIFFRECORDTYPE.BLANK:
                case BIFFRECORDTYPE.BLANK_OLD:
                case BIFFRECORDTYPE.MULBLANK:
                    break;

                case BIFFRECORDTYPE.INTEGER:
                case BIFFRECORDTYPE.INTEGER_OLD:
                    this.m_cellsValues[cell.ColumnIndex] = ((XlsBiffIntegerCell) cell).Value;
                    return;

                case BIFFRECORDTYPE.NUMBER:
                case BIFFRECORDTYPE.NUMBER_OLD:
                    num = ((XlsBiffNumberCell) cell).Value;
                    this.m_cellsValues[cell.ColumnIndex] = !this.m_ConvertOADate ? num : this.tryConvertOADateTime(num, cell.XFormat);
                    return;

                case BIFFRECORDTYPE.LABEL:
                case BIFFRECORDTYPE.LABEL_OLD:
                case BIFFRECORDTYPE.RSTRING:
                    this.m_cellsValues[cell.ColumnIndex] = ((XlsBiffLabelCell) cell).Value;
                    return;

                case BIFFRECORDTYPE.BOOLERR:
                    if (cell.ReadByte(7) != 0)
                    {
                        break;
                    }
                    this.m_cellsValues[cell.ColumnIndex] = cell.ReadByte(6) != 0;
                    return;

                case BIFFRECORDTYPE.LABELSST:
                {
                    string str = this.m_globals.SST.GetString(((XlsBiffLabelSSTCell) cell).SSTIndex);
                    this.m_cellsValues[cell.ColumnIndex] = str;
                    return;
                }
                case BIFFRECORDTYPE.BOOLERR_OLD:
                    if (cell.ReadByte(8) != 0)
                    {
                        break;
                    }
                    this.m_cellsValues[cell.ColumnIndex] = cell.ReadByte(7) != 0;
                    return;

                case BIFFRECORDTYPE.MULRK:
                {
                    XlsBiffMulRKCell cell2 = (XlsBiffMulRKCell) cell;
                    for (ushort i = cell.ColumnIndex; i <= cell2.LastColumnIndex; i = (ushort) (i + 1))
                    {
                        num = cell2.GetValue(i);
                        this.m_cellsValues[i] = !this.m_ConvertOADate ? num : this.tryConvertOADateTime(num, cell2.GetXF(i));
                    }
                    return;
                }
                default:
                    return;
            }
        }

        public bool Read()
        {
            if (!this.m_isValid)
            {
                return false;
            }
            if (this.m_IsFirstRead)
            {
                this.initializeSheetRead();
            }
            return this.moveToNextRecord();
        }

        private DataTable readWholeWorkSheet(XlsWorksheet sheet)
        {
            XlsBiffIndex index;
            if (!this.readWorkSheetGlobals(sheet, out index, out this.m_currentRowRecord))
            {
                return null;
            }
            DataTable table = new DataTable(sheet.Name);
            bool triggerCreateColumns = true;
            if (index != null)
            {
                this.readWholeWorkSheetWithIndex(index, triggerCreateColumns, table);
            }
            else
            {
                this.readWholeWorkSheetNoIndex(triggerCreateColumns, table);
            }
            table.EndLoadData();
            return table;
        }

        private void readWholeWorkSheetNoIndex(bool triggerCreateColumns, DataTable table)
        {
            while (this.Read())
            {
                if (this.m_depth == this.m_maxRow)
                {
                    break;
                }
                bool flag = false;
                if (triggerCreateColumns)
                {
                    if (this._isFirstRowAsColumnNames || (this._isFirstRowAsColumnNames && (this.m_maxRow == 1)))
                    {
                        for (int i = 0; i < this.m_maxCol; i++)
                        {
                            if ((this.m_cellsValues[i] != null) && (this.m_cellsValues[i].ToString().Length > 0))
                            {
                                Helpers.AddColumnHandleDuplicate(table, this.m_cellsValues[i].ToString());
                            }
                            else
                            {
                                Helpers.AddColumnHandleDuplicate(table, "Column" + i);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < this.m_maxCol; j++)
                        {
                            table.Columns.Add(null, typeof(object));
                        }
                    }
                    triggerCreateColumns = false;
                    flag = true;
                    table.BeginLoadData();
                }
                if ((!flag && (this.m_depth > 0)) && (!this._isFirstRowAsColumnNames || (this.m_maxRow != 1)))
                {
                    table.Rows.Add(this.m_cellsValues);
                }
            }
            if ((this.m_depth > 0) && (!this._isFirstRowAsColumnNames || (this.m_maxRow != 1)))
            {
                table.Rows.Add(this.m_cellsValues);
            }
        }

        private void readWholeWorkSheetWithIndex(XlsBiffIndex idx, bool triggerCreateColumns, DataTable table)
        {
            this.m_dbCellAddrs = idx.DbCellAddresses;
            for (int i = 0; i < this.m_dbCellAddrs.Length; i++)
            {
                if (this.m_depth == this.m_maxRow)
                {
                    return;
                }
                this.m_cellOffset = this.findFirstDataCellOffset((int) this.m_dbCellAddrs[i]);
                if (this.m_cellOffset < 0)
                {
                    return;
                }
                if (triggerCreateColumns)
                {
                    if ((this._isFirstRowAsColumnNames && this.readWorkSheetRow()) || (this._isFirstRowAsColumnNames && (this.m_maxRow == 1)))
                    {
                        for (int j = 0; j < this.m_maxCol; j++)
                        {
                            if ((this.m_cellsValues[j] != null) && (this.m_cellsValues[j].ToString().Length > 0))
                            {
                                Helpers.AddColumnHandleDuplicate(table, this.m_cellsValues[j].ToString());
                            }
                            else
                            {
                                Helpers.AddColumnHandleDuplicate(table, "Column" + j);
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < this.m_maxCol; k++)
                        {
                            table.Columns.Add(null, typeof(object));
                        }
                    }
                    triggerCreateColumns = false;
                    table.BeginLoadData();
                }
                while (this.readWorkSheetRow())
                {
                    table.Rows.Add(this.m_cellsValues);
                }
                if ((this.m_depth > 0) && (!this._isFirstRowAsColumnNames || (this.m_maxRow != 1)))
                {
                    table.Rows.Add(this.m_cellsValues);
                }
            }
        }

        private void readWorkBookGlobals()
        {
            XlsBiffRecord record;
            bool flag;
            try
            {
                this.m_hdr = XlsHeader.ReadHeader(this.m_file);
            }
            catch (HeaderException exception)
            {
                this.fail(exception.Message);
                return;
            }
            catch (FormatException exception2)
            {
                this.fail(exception2.Message);
                return;
            }
            XlsRootDirectory rootDir = new XlsRootDirectory(this.m_hdr);
            XlsDirectoryEntry entry = rootDir.FindEntry("Workbook") ?? rootDir.FindEntry("Book");
            if (entry != null)
            {
                if (entry.EntryType != STGTY.STGTY_STREAM)
                {
                    this.fail("Error: Workbook directory entry is not a Stream.");
                    return;
                }
                this.m_stream = new XlsBiffStream(this.m_hdr, entry.StreamFirstSector, entry.IsEntryMiniStream, rootDir);
                this.m_globals = new XlsWorkbookGlobals();
                this.m_stream.Seek(0, SeekOrigin.Begin);
                XlsBiffBOF fbof = this.m_stream.Read() as XlsBiffBOF;
                if ((fbof == null) || (fbof.Type != BIFFTYPE.WorkbookGlobals))
                {
                    this.fail("Error reading Workbook Globals - Stream has invalid data.");
                    return;
                }
                flag = false;
                this.m_version = fbof.Version;
                this.m_sheets = new List<XlsWorksheet>();
            }
            else
            {
                this.fail("Error: Neither stream 'Workbook' nor 'Book' was found in file.");
                return;
            }
            while ((record = this.m_stream.Read()) != null)
            {
                BIFFRECORDTYPE iD = record.ID;
                if (iD <= BIFFRECORDTYPE.COUNTRY)
                {
                    switch (iD)
                    {
                        case BIFFRECORDTYPE.FORMAT_V23:
                        {
                            XlsBiffFormatString str = (XlsBiffFormatString) record;
                            str.UseEncoding = this.m_encoding;
                            this.m_globals.Formats.Add((ushort) this.m_globals.Formats.Count, str);
                            break;
                        }
                        case BIFFRECORDTYPE.FONT:
                            goto Label_031C;

                        case BIFFRECORDTYPE.EOF:
                            goto Label_03F3;

                        case BIFFRECORDTYPE.CODEPAGE:
                            goto Label_02E4;

                        case BIFFRECORDTYPE.XF_V2:
                            goto Label_0395;

                        case BIFFRECORDTYPE.CONTINUE:
                            goto Label_03C0;

                        case BIFFRECORDTYPE.BOUNDSHEET:
                        {
                            XlsBiffBoundSheet refSheet = (XlsBiffBoundSheet) record;
                            if (refSheet.Type == XlsBiffBoundSheet.SheetType.Worksheet)
                            {
                                refSheet.IsV8 = this.isV8();
                                refSheet.UseEncoding = this.m_encoding;
                                this.m_sheets.Add(new XlsWorksheet(this.m_globals.Sheets.Count, refSheet));
                                this.m_globals.Sheets.Add(refSheet);
                            }
                            break;
                        }
                        case BIFFRECORDTYPE.COUNTRY:
                            this.m_globals.Country = record;
                            break;
                    }
                }
                else if (iD <= BIFFRECORDTYPE.EXTSST)
                {
                    switch (iD)
                    {
                        case BIFFRECORDTYPE.SST:
                            this.m_globals.SST = (XlsBiffSST) record;
                            flag = true;
                            break;

                        case BIFFRECORDTYPE.EXTSST:
                            this.m_globals.ExtSST = record;
                            flag = false;
                            break;

                        case BIFFRECORDTYPE.XF:
                            goto Label_0395;

                        case BIFFRECORDTYPE.INTERFACEHDR:
                            this.m_globals.InterfaceHdr = (XlsBiffInterfaceHdr) record;
                            break;

                        case BIFFRECORDTYPE.MMS:
                            goto Label_02C0;
                    }
                }
                else if (iD <= BIFFRECORDTYPE.FONT_V34)
                {
                    if ((iD != BIFFRECORDTYPE.PROT4REVPASSWORD) && (iD == BIFFRECORDTYPE.FONT_V34))
                    {
                        goto Label_031C;
                    }
                }
                else
                {
                    switch (iD)
                    {
                        case BIFFRECORDTYPE.XF_V3:
                        case BIFFRECORDTYPE.XF_V4:
                            goto Label_0395;

                        case BIFFRECORDTYPE.FORMAT:
                        {
                            XlsBiffFormatString str2 = (XlsBiffFormatString) record;
                            this.m_globals.Formats.Add(str2.Index, str2);
                            break;
                        }
                    }
                }
                continue;
            Label_02C0:
                this.m_globals.MMS = record;
                continue;
            Label_02E4:
                this.m_globals.CodePage = (XlsBiffSimpleValueRecord) record;
                try
                {
                    this.m_encoding = Encoding.GetEncoding(this.m_globals.CodePage.Value);
                }
                catch (ArgumentException)
                {
                }
                continue;
            Label_031C:
                this.m_globals.Fonts.Add(record);
                continue;
            Label_0395:
                this.m_globals.ExtendedFormats.Add(record);
                continue;
            Label_03C0:
                if (flag)
                {
                    XlsBiffContinue fragment = (XlsBiffContinue) record;
                    this.m_globals.SST.Append(fragment);
                }
                continue;
            Label_03F3:
                if (this.m_globals.SST != null)
                {
                    this.m_globals.SST.ReadStrings();
                }
                return;
            }
        }

        private bool readWorkSheetGlobals(XlsWorksheet sheet, out XlsBiffIndex idx, out XlsBiffRow row)
        {
            XlsBiffRecord record2;
            idx = null;
            row = null;
            this.m_stream.Seek((int) sheet.DataOffset, SeekOrigin.Begin);
            XlsBiffBOF fbof = this.m_stream.Read() as XlsBiffBOF;
            if ((fbof == null) || (fbof.Type != BIFFTYPE.Worksheet))
            {
                return false;
            }
            XlsBiffRecord record = this.m_stream.Read();
            if (record == null)
            {
                return false;
            }
            if (record is XlsBiffIndex)
            {
                idx = record as XlsBiffIndex;
            }
            else if (record is XlsBiffUncalced)
            {
                idx = this.m_stream.Read() as XlsBiffIndex;
            }
            if (idx != null)
            {
                idx.IsV8 = this.isV8();
            }
            XlsBiffDimensions dimensions = null;
            do
            {
                record2 = this.m_stream.Read();
                if (record2.ID == BIFFRECORDTYPE.DIMENSIONS)
                {
                    dimensions = (XlsBiffDimensions) record2;
                    break;
                }
            }
            while ((record2 != null) && (record2.ID != BIFFRECORDTYPE.ROW));
            if (record2.ID == BIFFRECORDTYPE.ROW)
            {
                row = (XlsBiffRow) record2;
            }
            XlsBiffRow row2 = null;
            while (row2 == null)
            {
                if (this.m_stream.Position >= this.m_stream.Size)
                {
                    break;
                }
                XlsBiffRecord record3 = this.m_stream.Read();
                if (record3 is XlsBiffEOF)
                {
                    break;
                }
                row2 = record3 as XlsBiffRow;
            }
            row = row2;
            this.m_maxCol = 0x100;
            if (dimensions != null)
            {
                dimensions.IsV8 = this.isV8();
                this.m_maxCol = dimensions.LastColumn - 1;
                sheet.Dimensions = dimensions;
            }
            this.m_maxRow = (idx == null) ? ((int) dimensions.LastRow) : ((int) idx.LastExistingRow);
            if ((idx != null) && (idx.LastExistingRow <= idx.FirstExistingRow))
            {
                return false;
            }
            if (row == null)
            {
                return false;
            }
            this.m_depth = 0;
            return true;
        }

        private bool readWorkSheetRow()
        {
            this.m_cellsValues = new object[this.m_maxCol];
            while (this.m_cellOffset < this.m_stream.Size)
            {
                XlsBiffRecord record = this.m_stream.ReadAt(this.m_cellOffset);
                this.m_cellOffset += record.Size;
                if (record is XlsBiffDbCell)
                {
                    break;
                }
                if (record is XlsBiffEOF)
                {
                    return false;
                }
                XlsBiffBlankCell cell = record as XlsBiffBlankCell;
                if ((cell != null) && (cell.ColumnIndex < this.m_maxCol))
                {
                    if (cell.RowIndex != this.m_depth)
                    {
                        this.m_cellOffset -= record.Size;
                        break;
                    }
                    this.pushCellValue(cell);
                }
            }
            this.m_depth++;
            return (this.m_depth < this.m_maxRow);
        }

        private object tryConvertOADateTime(double value, ushort XFormat)
        {
            XlsBiffFormatString str;
            ushort key = 0;
            if ((XFormat < 0) || (XFormat >= this.m_globals.ExtendedFormats.Count))
            {
                key = XFormat;
            }
            else
            {
                XlsBiffRecord record = this.m_globals.ExtendedFormats[XFormat];
                BIFFRECORDTYPE iD = record.ID;
                if (iD == BIFFRECORDTYPE.XF_V2)
                {
                    key = (ushort) (record.ReadByte(2) & 0x3f);
                }
                else if (iD == BIFFRECORDTYPE.XF_V3)
                {
                    if ((record.ReadByte(3) & 4) == 0)
                    {
                        return value;
                    }
                    key = record.ReadByte(1);
                }
                else if (iD == BIFFRECORDTYPE.XF_V4)
                {
                    if ((record.ReadByte(5) & 4) == 0)
                    {
                        return value;
                    }
                    key = record.ReadByte(1);
                }
                else
                {
                    if ((record.ReadByte(this.m_globals.Sheets[this.m_globals.Sheets.Count - 1].IsV8 ? 9 : 7) & 4) == 0)
                    {
                        return value;
                    }
                    key = record.ReadUInt16(2);
                }
            }
            switch (key)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 0x25:
                case 0x26:
                case 0x27:
                case 40:
                case 0x29:
                case 0x2a:
                case 0x2b:
                case 0x2c:
                case 0x30:
                    return value;

                case 14:
                case 15:
                case 0x10:
                case 0x11:
                case 0x12:
                case 0x13:
                case 20:
                case 0x15:
                case 0x16:
                case 0x2d:
                case 0x2e:
                case 0x2f:
                    return Helpers.ConvertFromOATime(value);

                case 0x31:
                    return value.ToString();
            }
            if (this.m_globals.Formats.TryGetValue(key, out str))
            {
                string str2 = str.Value;
                FormatReader reader = new FormatReader {
                    FormatString = str2
                };
                if (reader.IsDateFormatString())
                {
                    return Helpers.ConvertFromOATime(value);
                }
            }
            return value;
        }

        private object tryConvertOADateTime(object value, ushort XFormat)
        {
            double num;
            if (double.TryParse(value.ToString(), out num))
            {
                return this.tryConvertOADateTime(num, XFormat);
            }
            return value;
        }

        public int Depth
        {
            get
            {
                return this.m_depth;
            }
        }

        public string ExceptionMessage
        {
            get
            {
                return this.m_exceptionMessage;
            }
        }

        public int FieldCount
        {
            get
            {
                return this.m_maxCol;
            }
        }

        public bool IsClosed
        {
            get
            {
                return this.m_isClosed;
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
                return this.m_isValid;
            }
        }

        public object this[string name]
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public object this[int i]
        {
            get
            {
                return this.m_cellsValues[i];
            }
        }

        public string Name
        {
            get
            {
                if ((this.m_sheets != null) && (this.m_sheets.Count > 0))
                {
                    return this.m_sheets[this.m_SheetIndex].Name;
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
                return this.m_globals.Sheets.Count;
            }
        }
    }
}

