namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffEOF : XlsBiffRecord
    {
        internal XlsBiffEOF(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffEOF(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }
    }
}

