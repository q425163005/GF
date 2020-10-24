namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffMulRKCell : XlsBiffBlankCell
    {
        internal XlsBiffMulRKCell(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffMulRKCell(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public double GetValue(ushort ColumnIdx)
        {
            int offset = 6 + (6 * (ColumnIdx - base.ColumnIndex));
            if (offset > base.RecordSize)
            {
                return 0.0;
            }
            return XlsBiffRKCell.NumFromRK(base.ReadUInt32(offset));
        }

        public ushort GetXF(ushort ColumnIdx)
        {
            int offset = 4 + (6 * (ColumnIdx - base.ColumnIndex));
            if (offset > (base.RecordSize - 2))
            {
                return 0;
            }
            return base.ReadUInt16(offset);
        }

        public ushort LastColumnIndex
        {
            get
            {
                return base.ReadUInt16(base.RecordSize - 2);
            }
        }
    }
}

