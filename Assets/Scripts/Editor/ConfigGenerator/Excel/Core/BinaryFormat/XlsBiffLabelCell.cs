namespace Excel.Core.BinaryFormat
{
    using Excel.Core;
    using System;
    using System.Text;

    internal class XlsBiffLabelCell : XlsBiffBlankCell
    {
        private Encoding m_UseEncoding;

        internal XlsBiffLabelCell(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffLabelCell(byte[] bytes, uint offset) : base(bytes, offset)
        {
            this.m_UseEncoding = Encoding.Default;
        }

        public byte Length
        {
            get
            {
                return base.ReadByte(6);
            }
        }

        public Encoding UseEncoding
        {
            get
            {
                return this.m_UseEncoding;
            }
            set
            {
                this.m_UseEncoding = value;
            }
        }

        public string Value
        {
            get
            {
                byte[] bytes = base.ReadArray(8, this.Length * (Helpers.IsSingleByteEncoding(this.m_UseEncoding) ? 1 : 2));
                return this.m_UseEncoding.GetString(bytes, 0, bytes.Length);
            }
        }
    }
}

