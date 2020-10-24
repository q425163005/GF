namespace Excel.Core.BinaryFormat
{
    using System;

    internal enum BIFFTYPE : ushort
    {
        Chart = 0x20,
        v4MacroSheet = 0x40,
        v4WorkbookGlobals = 0x100,
        VBModule = 6,
        WorkbookGlobals = 5,
        Worksheet = 0x10
    }
}

