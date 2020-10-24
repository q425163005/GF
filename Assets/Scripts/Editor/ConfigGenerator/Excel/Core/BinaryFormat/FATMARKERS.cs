namespace Excel.Core.BinaryFormat
{
    using System;

    internal enum FATMARKERS : uint
    {
        FAT_DifSector = 0xfffffffc,
        FAT_EndOfChain = 0xfffffffe,
        FAT_FatSector = 0xfffffffd,
        FAT_FreeSpace = 0xffffffff
    }
}

