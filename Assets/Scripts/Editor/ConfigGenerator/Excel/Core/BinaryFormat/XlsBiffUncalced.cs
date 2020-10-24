namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffUncalced : XlsBiffRecord
    {
        internal XlsBiffUncalced(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffUncalced(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }
    }
}

