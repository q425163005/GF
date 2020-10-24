namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffContinue : XlsBiffRecord
    {
        internal XlsBiffContinue(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffContinue(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }
    }
}

