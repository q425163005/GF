namespace Excel.Core.BinaryFormat
{
    using System;

    internal enum FORMULAERROR : byte
    {
        DIV0 = 7,
        NA = 0x2a,
        NAME = 0x1d,
        NULL = 0,
        NUM = 0x24,
        REF = 0x17,
        VALUE = 15
    }
}

