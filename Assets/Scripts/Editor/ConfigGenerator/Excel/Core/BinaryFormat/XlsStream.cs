namespace Excel.Core.BinaryFormat
{
    using System;
    using System.IO;

    internal class XlsStream
    {
        protected XlsFat m_fat;
        protected Stream m_fileStream;
        protected XlsHeader m_hdr;
        protected bool m_isMini;
        protected XlsFat m_minifat;
        protected XlsRootDirectory m_rootDir;
        protected uint m_startSector;

        public XlsStream(XlsHeader hdr, uint startSector, bool isMini, XlsRootDirectory rootDir)
        {
            this.m_fileStream = hdr.FileStream;
            this.m_fat = hdr.FAT;
            this.m_hdr = hdr;
            this.m_startSector = startSector;
            this.m_isMini = isMini;
            this.m_rootDir = rootDir;
            this.CalculateMiniFat(rootDir);
        }

        public void CalculateMiniFat(XlsRootDirectory rootDir)
        {
            this.m_minifat = this.m_hdr.GetMiniFAT(rootDir);
        }

        public byte[] ReadStream()
        {
            uint startSector = this.m_startSector;
            uint num2 = 0;
            int count = this.m_isMini ? this.m_hdr.MiniSectorSize : this.m_hdr.SectorSize;
            XlsFat fat = this.m_isMini ? this.m_minifat : this.m_fat;
            long num4 = 0L;
            if (this.m_isMini && (this.m_rootDir != null))
            {
                num4 = (this.m_rootDir.RootEntry.StreamFirstSector + 1) * this.m_hdr.SectorSize;
            }
            byte[] buffer = new byte[count];
            using (MemoryStream stream = new MemoryStream(count * 8))
            {
                lock (this.m_fileStream)
                {
                    do
                    {
                        if ((num2 == 0) || ((startSector - num2) != 1))
                        {
                            uint num5 = this.m_isMini ? startSector : (startSector + 1);
                            this.m_fileStream.Seek((num5 * count) + num4, SeekOrigin.Begin);
                        }
                        num2 = startSector;
                        this.m_fileStream.Read(buffer, 0, count);
                        stream.Write(buffer, 0, count);
                    }
                    while ((startSector = fat.GetNextSector(startSector)) != 0xfffffffe);
                }
                return stream.ToArray();
            }
        }

        public uint BaseOffset
        {
            get
            {
                return (uint) ((this.m_startSector + 1) * this.m_hdr.SectorSize);
            }
        }

        public uint BaseSector
        {
            get
            {
                return this.m_startSector;
            }
        }
    }
}

