namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffSimpleValueRecord : XlsBiffRecord
    {
        internal XlsBiffSimpleValueRecord(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffSimpleValueRecord(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public ushort Value
        {
            get
            {
                return base.ReadUInt16(0);
            }
        }
    }
}

