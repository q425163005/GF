namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffBOF : XlsBiffRecord
    {
        internal XlsBiffBOF(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffBOF(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public ushort CreationID
        {
            get
            {
                if (base.RecordSize < 6)
                {
                    return 0;
                }
                return base.ReadUInt16(4);
            }
        }

        public ushort CreationYear
        {
            get
            {
                if (base.RecordSize < 8)
                {
                    return 0;
                }
                return base.ReadUInt16(6);
            }
        }

        public uint HistoryFlag
        {
            get
            {
                if (base.RecordSize < 12)
                {
                    return 0;
                }
                return base.ReadUInt32(8);
            }
        }

        public uint MinVersionToOpen
        {
            get
            {
                if (base.RecordSize < 0x10)
                {
                    return 0;
                }
                return base.ReadUInt32(12);
            }
        }

        public BIFFTYPE Type
        {
            get
            {
                return (BIFFTYPE) base.ReadUInt16(2);
            }
        }

        public ushort Version
        {
            get
            {
                return base.ReadUInt16(0);
            }
        }
    }
}

