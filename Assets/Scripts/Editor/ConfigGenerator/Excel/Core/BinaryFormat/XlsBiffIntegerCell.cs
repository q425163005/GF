namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffIntegerCell : XlsBiffBlankCell
    {
        internal XlsBiffIntegerCell(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffIntegerCell(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public uint Value
        {
            get
            {
                return base.ReadUInt16(6);
            }
        }
    }
}

