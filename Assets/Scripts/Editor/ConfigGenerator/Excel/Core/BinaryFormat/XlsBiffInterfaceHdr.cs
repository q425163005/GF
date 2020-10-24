namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffInterfaceHdr : XlsBiffRecord
    {
        internal XlsBiffInterfaceHdr(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffInterfaceHdr(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public ushort CodePage
        {
            get
            {
                return base.ReadUInt16(0);
            }
        }
    }
}

