namespace Excel.Core.BinaryFormat
{
    using System;
    using System.Text;

    internal class XlsBiffFormatString : XlsBiffRecord
    {
        private Encoding m_UseEncoding;
        private string m_value;

        internal XlsBiffFormatString(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffFormatString(byte[] bytes, uint offset) : base(bytes, offset)
        {
            this.m_UseEncoding = Encoding.Default;
        }

        public ushort Index
        {
            get
            {
                if (base.ID == BIFFRECORDTYPE.FORMAT_V23)
                {
                    return 0;
                }
                return base.ReadUInt16(0);
            }
        }

        public ushort Length
        {
            get
            {
                if (base.ID == BIFFRECORDTYPE.FORMAT_V23)
                {
                    return base.ReadByte(0);
                }
                return base.ReadUInt16(2);
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
                if (this.m_value == null)
                {
                    switch (base.ID)
                    {
                        case BIFFRECORDTYPE.FORMAT_V23:
                            this.m_value = this.m_UseEncoding.GetString(base.m_bytes, base.m_readoffset + 1, this.Length);
                            break;

                        case BIFFRECORDTYPE.FORMAT:
                        {
                            int index = base.m_readoffset + 5;
                            byte num2 = base.ReadByte(3);
                            this.m_UseEncoding = ((num2 & 1) == 1) ? Encoding.Unicode : Encoding.Default;
                            if ((num2 & 4) == 1)
                            {
                                index += 4;
                            }
                            if ((num2 & 8) == 1)
                            {
                                index += 2;
                            }
                            this.m_value = this.m_UseEncoding.IsSingleByte ? this.m_UseEncoding.GetString(base.m_bytes, index, this.Length) : this.m_UseEncoding.GetString(base.m_bytes, index, this.Length * 2);
                            break;
                        }
                    }
                }
                return this.m_value;
            }
        }
    }
}

