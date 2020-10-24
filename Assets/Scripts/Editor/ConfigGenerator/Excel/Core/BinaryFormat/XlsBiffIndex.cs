namespace Excel.Core.BinaryFormat
{
    using System;
    using System.Collections.Generic;

    internal class XlsBiffIndex : XlsBiffRecord
    {
        private bool isV8;

        internal XlsBiffIndex(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffIndex(byte[] bytes, uint offset) : base(bytes, offset)
        {
            this.isV8 = true;
        }

        public uint[] DbCellAddresses
        {
            get
            {
                int recordSize = base.RecordSize;
                int num2 = this.isV8 ? 0x10 : 12;
                if (recordSize <= num2)
                {
                    return new uint[0];
                }
                List<uint> list = new List<uint>((recordSize - num2) / 4);
                for (int i = num2; i < recordSize; i += 4)
                {
                    list.Add(base.ReadUInt32(i));
                }
                return list.ToArray();
            }
        }

        public uint FirstExistingRow
        {
            get
            {
                if (!this.isV8)
                {
                    return base.ReadUInt16(4);
                }
                return base.ReadUInt32(4);
            }
        }

        public bool IsV8
        {
            get
            {
                return this.isV8;
            }
            set
            {
                this.isV8 = value;
            }
        }

        public uint LastExistingRow
        {
            get
            {
                if (!this.isV8)
                {
                    return base.ReadUInt16(6);
                }
                return base.ReadUInt32(8);
            }
        }
    }
}

