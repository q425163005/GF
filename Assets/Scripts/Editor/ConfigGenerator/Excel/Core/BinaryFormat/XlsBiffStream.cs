namespace Excel.Core.BinaryFormat
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class XlsBiffStream : XlsStream
    {
        private readonly byte[] bytes;
        private int m_offset;
        private readonly int m_size;

        public XlsBiffStream(XlsHeader hdr, uint streamStart, bool isMini, XlsRootDirectory rootDir) : base(hdr, streamStart, isMini, rootDir)
        {
            this.bytes = base.ReadStream();
            this.m_size = this.bytes.Length;
            this.m_offset = 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public XlsBiffRecord Read()
        {
            XlsBiffRecord record = XlsBiffRecord.GetRecord(this.bytes, (uint) this.m_offset);
            this.m_offset += record.Size;
            if (this.m_offset > this.m_size)
            {
                return null;
            }
            return record;
        }

        public XlsBiffRecord ReadAt(int offset)
        {
            XlsBiffRecord record = XlsBiffRecord.GetRecord(this.bytes, (uint) offset);
            if ((this.m_offset + record.Size) > this.m_size)
            {
                return null;
            }
            return record;
        }

        [Obsolete("Use BIFF-specific methods for this stream")]
        public byte[] ReadStream()
        {
            return this.bytes;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Seek(int offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.m_offset = offset;
                    break;

                case SeekOrigin.Current:
                    this.m_offset += offset;
                    break;

                case SeekOrigin.End:
                    this.m_offset = this.m_size - offset;
                    break;
            }
            if (this.m_offset < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} On offset={1}", "BIFF Stream error: Moving before stream start.", offset));
            }
            if (this.m_offset > this.m_size)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} On offset={1}", "BIFF Stream error: Moving after stream end.", offset));
            }
        }

        public int Position
        {
            get
            {
                return this.m_offset;
            }
        }

        public int Size
        {
            get
            {
                return this.m_size;
            }
        }
    }
}

