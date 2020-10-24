namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffRow : XlsBiffRecord
    {
        internal XlsBiffRow(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffRow(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public ushort FirstDefinedColumn
        {
            get
            {
                return base.ReadUInt16(2);
            }
        }

        public ushort Flags
        {
            get
            {
                return base.ReadUInt16(12);
            }
        }

        public ushort LastDefinedColumn
        {
            get
            {
                return base.ReadUInt16(4);
            }
        }

        public uint RowHeight
        {
            get
            {
                return base.ReadUInt16(6);
            }
        }

        public ushort RowIndex
        {
            get
            {
                return base.ReadUInt16(0);
            }
        }

        public ushort XFormat
        {
            get
            {
                return base.ReadUInt16(14);
            }
        }
    }
}

