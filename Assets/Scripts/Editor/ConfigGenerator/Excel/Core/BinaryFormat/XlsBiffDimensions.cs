namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffDimensions : XlsBiffRecord
    {
        private bool isV8;

        internal XlsBiffDimensions(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffDimensions(byte[] bytes, uint offset) : base(bytes, offset)
        {
            this.isV8 = true;
        }

        public ushort FirstColumn
        {
            get
            {
                if (!this.isV8)
                {
                    return base.ReadUInt16(4);
                }
                return base.ReadUInt16(8);
            }
        }

        public uint FirstRow
        {
            get
            {
                if (!this.isV8)
                {
                    return base.ReadUInt16(0);
                }
                return base.ReadUInt32(0);
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

        public ushort LastColumn
        {
            get
            {
                if (!this.isV8)
                {
                    return base.ReadUInt16(6);
                }
                return (ushort) ((base.ReadUInt16(9) >> 8) + 1);
            }
        }

        public uint LastRow
        {
            get
            {
                if (!this.isV8)
                {
                    return base.ReadUInt16(2);
                }
                return base.ReadUInt32(4);
            }
        }
    }
}

