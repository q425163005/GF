namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffLabelSSTCell : XlsBiffBlankCell
    {
        internal XlsBiffLabelSSTCell(byte[] bytes) : this(bytes, 0)
        {
        }

        internal XlsBiffLabelSSTCell(byte[] bytes, uint offset) : base(bytes, offset)
        {
        }

        public string Text(XlsBiffSST sst)
        {
            return sst.GetString(this.SSTIndex);
        }

        public uint SSTIndex
        {
            get
            {
                return base.ReadUInt32(6);
            }
        }
    }
}

