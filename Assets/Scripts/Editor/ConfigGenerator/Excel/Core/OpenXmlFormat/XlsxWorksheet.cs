namespace Excel.Core.OpenXmlFormat
{
    using System;
    using System.Runtime.CompilerServices;

    internal class XlsxWorksheet
    {
        private XlsxDimension _dimension;
        private int _id;
        private string _Name;
        private string _path;
        private string _rid;
        public const string A_r = "r";
        public const string A_ref = "ref";
        public const string A_s = "s";
        public const string A_t = "t";
        public const string N_c = "c";
        public const string N_dimension = "dimension";
        public const string N_row = "row";
        public const string N_sheetData = "sheetData";
        public const string N_v = "v";

        public XlsxWorksheet(string name, int id, string rid)
        {
            this._Name = name;
            this._id = id;
            this._rid = rid;
        }

        public int ColumnsCount
        {
            get
            {
                if (this.IsEmpty)
                {
                    return 0;
                }
                if (this._dimension != null)
                {
                    return this._dimension.LastCol;
                }
                return -1;
            }
        }

        public XlsxDimension Dimension
        {
            get
            {
                return this._dimension;
            }
            set
            {
                this._dimension = value;
            }
        }

        public int Id
        {
            get
            {
                return this._id;
            }
        }

        public bool IsEmpty { get; set; }

        public string Name
        {
            get
            {
                return this._Name;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }

        public string RID
        {
            get
            {
                return this._rid;
            }
            set
            {
                this._rid = value;
            }
        }

        public int RowsCount
        {
            get
            {
                if (this._dimension != null)
                {
                    return ((this._dimension.LastRow - this._dimension.FirstRow) + 1);
                }
                return -1;
            }
        }
    }
}

