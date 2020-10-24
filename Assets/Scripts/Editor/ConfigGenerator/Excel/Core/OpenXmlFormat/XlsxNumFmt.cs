namespace Excel.Core.OpenXmlFormat
{
    using System;

    internal class XlsxNumFmt
    {
        private string _FormatCode;
        private int _Id;
        public const string A_formatCode = "formatCode";
        public const string A_numFmtId = "numFmtId";
        public const string N_numFmt = "numFmt";

        public XlsxNumFmt(int id, string formatCode)
        {
            this._Id = id;
            this._FormatCode = formatCode;
        }

        public string FormatCode
        {
            get
            {
                return this._FormatCode;
            }
            set
            {
                this._FormatCode = value;
            }
        }

        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
            }
        }
    }
}

