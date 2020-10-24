namespace Excel.Core.BinaryFormat
{
    using Excel.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class XlsHeader
    {
        private readonly byte[] m_bytes = new byte[0x200];
        private XlsFat m_fat;
        private readonly Stream m_file;
        private XlsFat m_minifat;

        private XlsHeader(Stream file)
        {
            this.m_file = file;
        }

        public XlsFat GetMiniFAT(XlsRootDirectory rootDir)
        {
            if (this.m_minifat == null)
            {
                int miniSectorSize = this.MiniSectorSize;
                List<uint> sectors = new List<uint>(this.MiniFatSectorCount);
                uint item = BitConverter.ToUInt32(this.m_bytes, 60);
                sectors.Add(item);
                this.m_minifat = new XlsFat(this, sectors, this.MiniSectorSize, true, rootDir);
            }
            return this.m_minifat;
        }

        public static XlsHeader ReadHeader(Stream file)
        {
            XlsHeader header = new XlsHeader(file);
            lock (file)
            {
                file.Seek(0L, SeekOrigin.Begin);
                file.Read(header.m_bytes, 0, 0x200);
            }
            if (!header.IsSignatureValid)
            {
                throw new HeaderException("Error: Invalid file signature.");
            }
            if (header.ByteOrder != 0xfffe)
            {
                throw new FormatException("Error: Invalid byte order specified in header.");
            }
            return header;
        }

        public ushort ByteOrder
        {
            get
            {
                return BitConverter.ToUInt16(this.m_bytes, 0x1c);
            }
        }

        public Guid ClassId
        {
            get
            {
                byte[] dst = new byte[0x10];
                Buffer.BlockCopy(this.m_bytes, 8, dst, 0, 0x10);
                return new Guid(dst);
            }
        }

        public uint DifFirstSector
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x44);
            }
        }

        public int DifSectorCount
        {
            get
            {
                return BitConverter.ToInt32(this.m_bytes, 0x48);
            }
        }

        public ushort DllVersion
        {
            get
            {
                return BitConverter.ToUInt16(this.m_bytes, 0x1a);
            }
        }

        public XlsFat FAT
        {
            get
            {
                uint num;
                if (this.m_fat != null)
                {
                    return this.m_fat;
                }
                int sectorSize = this.SectorSize;
                List<uint> sectors = new List<uint>(this.FatSectorCount);
                for (int i = 0x4c; i < sectorSize; i += 4)
                {
                    num = BitConverter.ToUInt32(this.m_bytes, i);
                    if (num == uint.MaxValue)
                    {
                        goto Label_0129;
                    }
                    sectors.Add(num);
                }
                int difSectorCount = this.DifSectorCount;
                if (difSectorCount != 0)
                {
                    lock (this.m_file)
                    {
                        uint difFirstSector = this.DifFirstSector;
                        byte[] buffer = new byte[sectorSize];
                        uint num6 = 0;
                        while (difSectorCount > 0)
                        {
                            sectors.Capacity += 0x80;
                            if ((num6 == 0) || ((difFirstSector - num6) != 1))
                            {
                                this.m_file.Seek((difFirstSector + 1) * sectorSize, SeekOrigin.Begin);
                            }
                            num6 = difFirstSector;
                            this.m_file.Read(buffer, 0, sectorSize);
                            for (int j = 0; j < 0x1fc; j += 4)
                            {
                                num = BitConverter.ToUInt32(buffer, j);
                                if (num == uint.MaxValue)
                                {
                                    goto Label_0129;
                                }
                                sectors.Add(num);
                            }
                            num = BitConverter.ToUInt32(buffer, 0x1fc);
                            if (num == uint.MaxValue)
                            {
                                goto Label_0129;
                            }
                            if (difSectorCount-- > 1)
                            {
                                difFirstSector = num;
                            }
                            else
                            {
                                sectors.Add(num);
                            }
                        }
                    }
                }
            Label_0129:
                this.m_fat = new XlsFat(this, sectors, this.SectorSize, false, null);
                return this.m_fat;
            }
        }

        public int FatSectorCount
        {
            get
            {
                return BitConverter.ToInt32(this.m_bytes, 0x2c);
            }
        }

        public Stream FileStream
        {
            get
            {
                return this.m_file;
            }
        }

        public bool IsSignatureValid
        {
            get
            {
                return (this.Signature == 16220472316735377360L);
            }
        }

        public uint MiniFatFirstSector
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 60);
            }
        }

        public int MiniFatSectorCount
        {
            get
            {
                return BitConverter.ToInt32(this.m_bytes, 0x40);
            }
        }

        public int MiniSectorSize
        {
            get
            {
                return (((int) 1) << BitConverter.ToUInt16(this.m_bytes, 0x20));
            }
        }

        public uint MiniStreamCutoff
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x38);
            }
        }

        public uint RootDirectoryEntryStart
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x30);
            }
        }

        public int SectorSize
        {
            get
            {
                return (((int) 1) << BitConverter.ToUInt16(this.m_bytes, 30));
            }
        }

        public ulong Signature
        {
            get
            {
                return BitConverter.ToUInt64(this.m_bytes, 0);
            }
        }

        public uint TransactionSignature
        {
            get
            {
                return BitConverter.ToUInt32(this.m_bytes, 0x34);
            }
        }

        public ushort Version
        {
            get
            {
                return BitConverter.ToUInt16(this.m_bytes, 0x18);
            }
        }
    }
}

