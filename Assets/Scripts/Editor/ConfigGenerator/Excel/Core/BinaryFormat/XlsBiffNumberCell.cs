namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffNumberCell : XlsBiffBlankCell
    {
        internal XlsBiffNumberCell(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffNumberCell(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public double Value
        {
            get
            {
                return base.ReadDouble(6);
            }
        }
    }
}

