namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffMulBlankCell : XlsBiffBlankCell
    {
        internal XlsBiffMulBlankCell(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffMulBlankCell(byte[] bytes, uint offset) : base(bytes, offset)
        {
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

