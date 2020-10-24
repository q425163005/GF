namespace Excel.Core.BinaryFormat
{
    using Excel.Exceptions;
    using System;
    using System.Text;

    internal class XlsDirectoryEntry
    {
        public const int Length = 0x80;
        private readonly byte[] m_bytes;
        private XlsDirectoryEntry m_child;
        private XlsHeader m_header;
        private XlsDirectoryEntry m_leftSibling;
        private XlsDirectoryEntry m_rightSibling;

        public XlsDirectoryEntry(byte[] bytes, XlsHeader header)
        {
            if (bytes.Length < 0x80)
            {
                throw new BiffRecordException("Directory Entry error: Array is too small.");
            }
            this.m_bytes = bytes;
            this.m_header = header;
        }

        public XlsDirectoryEntry Child
        {
            get
            {
                return this.m_child;
            }
            set
            {
                if (this.m_child == null)
                {
                    this.m_child = value;
                }
            }
        }

        public uint ChildSid
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x4c);
            }
        }

        public Guid ClassId
        {
            get
            {
                byte[] dst = new byte[0x10];
                Buffer.BlockCopy(this.m_bytes, 80, dst, 0, 0x10);
                return new Guid(dst);
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return DateTime.FromFileTime(BitConverter.ToInt64(this.m_bytes, 100));
            }
        }

        public DECOLOR EntryColor
        {
            get
            {
                return (DECOLOR) Buffer.GetByte(this.m_bytes, 0x43);
            }
        }

        public ushort EntryLength
        {
            get
            {
                return BitConverter.ToUInt16(this.m_bytes, 0x40);
            }
        }

        public string EntryName
        {
            get
            {
                return Encoding.Unicode.GetString(this.m_bytes, 0, this.EntryLength).TrimEnd(new char[1]);
            }
        }

        public STGTY EntryType
        {
            get
            {
                return (STGTY) Buffer.GetByte(this.m_bytes, 0x42);
            }
        }

        public bool IsEntryMiniStream
        {
            get
            {
                return (this.StreamSize < this.m_header.MiniStreamCutoff);
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                return DateTime.FromFileTime(BitConverter.ToInt64(this.m_bytes, 0x6c));
            }
        }

        public XlsDirectoryEntry LeftSibling
        {
            get
            {
                return this.m_leftSibling;
            }
            set
            {
                if (this.m_leftSibling == null)
                {
                    this.m_leftSibling = value;
                }
            }
        }

        public uint LeftSiblingSid
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x44);
            }
        }

        public uint PropType
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x7c);
            }
        }

        public XlsDirectoryEntry RightSibling
        {
            get
            {
                return this.m_rightSibling;
            }
            set
            {
                if (this.m_rightSibling == null)
                {
                    this.m_rightSibling = value;
                }
            }
        }

        public uint RightSiblingSid
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x48);
            }
        }

        public uint StreamFirstSector
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x74);
            }
        }

        public uint StreamSize
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 120);
            }
        }

        public uint UserFlags
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x60);
            }
        }
    }
}

