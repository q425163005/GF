namespace Excel.Core.OpenXmlFormat
{
    using System;

    internal class XlsxXf
    {
        private bool _applyNumberFormat;
        private int _Id;
        private int _numFmtId;
        public const string A_applyNumberFormat = "applyNumberFormat";
        public const string A_numFmtId = "numFmtId";
        public const string A_xfId = "xfId";
        public const string N_xf = "xf";

        public XlsxXf(int id, int numFmtId, string applyNumberFormat)
        {
            this._Id = id;
            this._numFmtId = numFmtId;
            this._applyNumberFormat = (applyNumberFormat != null) && (applyNumberFormat == "1");
        }

        public bool ApplyNumberFormat
        {
            get
            {
                return this._applyNumberFormat;
            }
            set
            {
                this._applyNumberFormat = value;
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

        public int NumFmtId
        {
            get
            {
                return this._numFmtId;
            }
            set
            {
                this._numFmtId = value;
            }
        }
    }
}

