namespace Excel.Core.BinaryFormat
{
    using Excel.Core;
    using System;
    using System.Text;

    internal class XlsBiffBoundSheet : XlsBiffRecord
    {
        private bool isV8;
        private Encoding m_UseEncoding;

        internal XlsBiffBoundSheet(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffBoundSheet(byte[] bytes, uint offset) : base(bytes, offset)
        {
            this.isV8 = true;
            this.m_UseEncoding = Encoding.Default;
        }

        public bool IsV8
        {
            get
            {
                return this.isV8;
            }
            set
            {
                this.isV8 = value;
            }
        }

        public string SheetName
        {
            get
            {
                ushort count = base.ReadByte(6);
                int num2 = 8;
                if (!this.isV8)
                {
                    return Encoding.Default.GetString(base.m_bytes, (base.m_readoffset + num2) - 1, count);
                }
                if (base.ReadByte(7) == 0)
                {
                    return Encoding.Default.GetString(base.m_bytes, base.m_readoffset + num2, count);
                }
                return this.m_UseEncoding.GetString(base.m_bytes, base.m_readoffset + num2, Helpers.IsSingleByteEncoding(this.m_UseEncoding) ? count : (count * 2));
            }
        }

        public uint StartOffset
        {
            get
            {
                return base.ReadUInt32(0);
            }
        }

        public SheetType Type
        {
            get
            {
                return (SheetType) base.ReadByte(4);
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

        public SheetVisibility VisibleState
        {
            get
            {
                return (SheetVisibility) ((byte) (base.ReadByte(5) & 3));
            }
        }

        public enum SheetType : byte
        {
            Chart = 2,
            MacroSheet = 1,
            VBModule = 6,
            Worksheet = 0
        }

        public enum SheetVisibility : byte
        {
            Hidden = 1,
            VeryHidden = 2,
            Visible = 0
        }
    }
}

