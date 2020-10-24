namespace Excel.Core.BinaryFormat
{
    using Excel.Core;
    using System;
    using System.Text;

    internal class XlsBiffFormulaCell : XlsBiffNumberCell
    {
        private Encoding m_UseEncoding;

        internal XlsBiffFormulaCell(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffFormulaCell(byte[] bytes, uint offset) : base(bytes, offset)
        {
            this.m_UseEncoding = Encoding.Default;
        }

        public FormulaFlags Flags
        {
            get
            {
                return (FormulaFlags) base.ReadUInt16(14);
            }
        }

        public string Formula
        {
            get
            {
                byte[] bytes = base.ReadArray(0x10, this.FormulaLength);
                return Encoding.Default.GetString(bytes, 0, bytes.Length);
            }
        }

        public byte FormulaLength
        {
            get
            {
                return base.ReadByte(15);
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

        public object Value
        {
            get
            {
                XlsBiffFormulaString str;
                long num = base.ReadInt64(6);
                if ((num & -281474976710656L) != -281474976710656L)
                {
                    return Helpers.Int64BitsToDouble(num);
                }
                byte num2 = (byte) (num & 0xffL);
                byte num3 = (byte) ((num >> 0x10) & 0xffL);
                switch (num2)
                {
                    case 0:
                    {
                        XlsBiffRecord record = XlsBiffRecord.GetRecord(base.m_bytes, (uint) (base.Offset + base.Size));
                        if (record.ID != BIFFRECORDTYPE.SHRFMLA)
                        {
                            str = record as XlsBiffFormulaString;
                            break;
                        }
                        str = XlsBiffRecord.GetRecord(base.m_bytes, (uint) ((base.Offset + base.Size) + record.Size)) as XlsBiffFormulaString;
                        break;
                    }
                    case 1:
                        return (num3 != 0);

                    case 2:
                        return (FORMULAERROR) num3;

                    default:
                        return null;
                }
                if (str == null)
                {
                    return string.Empty;
                }
                str.UseEncoding = this.m_UseEncoding;
                return str.Value;
            }
        }

        [Flags]
        public enum FormulaFlags : ushort
        {
            AlwaysCalc = 1,
            CalcOnLoad = 2,
            SharedFormulaGroup = 8
        }
    }
}

