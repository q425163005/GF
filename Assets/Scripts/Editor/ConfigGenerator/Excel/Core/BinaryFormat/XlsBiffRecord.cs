namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsBiffRecord
    {
        protected byte[] m_bytes;
        protected int m_readoffset;

        protected XlsBiffRecord(byte[] bytes) : this(bytes, 0)
        {
        }

        protected XlsBiffRecord(byte[] bytes, uint offset)
        {
            if ((bytes.Length - offset) < 4L)
            {
                throw new ArgumentException("Error: Buffer size is less than minimum BIFF record size.");
            }
            this.m_bytes = bytes;
            this.m_readoffset = 4 + ((int) offset);
            if (bytes.Length < (offset + this.Size))
            {
                throw new ArgumentException("BIFF Stream error: Buffer size is less than entry length.");
            }
        }

        public static XlsBiffRecord GetRecord(byte[] bytes, uint offset)
        {
            BIFFRECORDTYPE biffrecordtype = (BIFFRECORDTYPE) BitConverter.ToUInt16(bytes, (int) offset);
            if (biffrecordtype <= BIFFRECORDTYPE.MULBLANK)
            {
                switch (biffrecordtype)
                {
                    case BIFFRECORDTYPE.BLANK_OLD:
                    case BIFFRECORDTYPE.BOOLERR_OLD:
                        goto Label_01FA;

                    case BIFFRECORDTYPE.INTEGER_OLD:
                        goto Label_021A;

                    case BIFFRECORDTYPE.NUMBER_OLD:
                        goto Label_0222;

                    case BIFFRECORDTYPE.LABEL_OLD:
                        goto Label_020A;

                    case BIFFRECORDTYPE.FORMULA_OLD:
                        goto Label_023A;

                    case BIFFRECORDTYPE.BOF_V2:
                        goto Label_01C2;

                    case BIFFRECORDTYPE.EOF:
                        return new XlsBiffEOF(bytes, offset);

                    case BIFFRECORDTYPE.FORMAT_V23:
                        goto Label_0242;

                    case BIFFRECORDTYPE.CONTINUE:
                        return new XlsBiffContinue(bytes, offset);

                    case BIFFRECORDTYPE.WINDOW1:
                        return new XlsBiffWindow1(bytes, offset);

                    case BIFFRECORDTYPE.BACKUP:
                        return new XlsBiffSimpleValueRecord(bytes, offset);

                    case BIFFRECORDTYPE.CODEPAGE:
                        return new XlsBiffSimpleValueRecord(bytes, offset);

                    case BIFFRECORDTYPE.RECORD1904:
                        return new XlsBiffSimpleValueRecord(bytes, offset);

                    case BIFFRECORDTYPE.UNCALCED:
                        return new XlsBiffUncalced(bytes, offset);

                    case BIFFRECORDTYPE.BOUNDSHEET:
                        return new XlsBiffBoundSheet(bytes, offset);

                    case BIFFRECORDTYPE.MULRK:
                        return new XlsBiffMulRKCell(bytes, offset);

                    case BIFFRECORDTYPE.MULBLANK:
                        return new XlsBiffMulBlankCell(bytes, offset);

                    case BIFFRECORDTYPE.FNGROUPCOUNT:
                        return new XlsBiffSimpleValueRecord(bytes, offset);

                    case BIFFRECORDTYPE.HIDEOBJ:
                        return new XlsBiffSimpleValueRecord(bytes, offset);
                }
                goto Label_02B2;
            }
            if (biffrecordtype <= BIFFRECORDTYPE.INDEX)
            {
                switch (biffrecordtype)
                {
                    case BIFFRECORDTYPE.RSTRING:
                    case BIFFRECORDTYPE.LABEL:
                        goto Label_020A;

                    case BIFFRECORDTYPE.DBCELL:
                        return new XlsBiffDbCell(bytes, offset);

                    case BIFFRECORDTYPE.BOOKBOOL:
                        return new XlsBiffSimpleValueRecord(bytes, offset);

                    case BIFFRECORDTYPE.INTERFACEHDR:
                        return new XlsBiffInterfaceHdr(bytes, offset);

                    case BIFFRECORDTYPE.SST:
                        return new XlsBiffSST(bytes, offset);

                    case BIFFRECORDTYPE.LABELSST:
                        return new XlsBiffLabelSSTCell(bytes, offset);

                    case BIFFRECORDTYPE.USESELFS:
                        return new XlsBiffSimpleValueRecord(bytes, offset);

                    case BIFFRECORDTYPE.DIMENSIONS:
                        return new XlsBiffDimensions(bytes, offset);

                    case BIFFRECORDTYPE.BLANK:
                    case BIFFRECORDTYPE.BOOLERR:
                        goto Label_01FA;

                    case BIFFRECORDTYPE.INTEGER:
                        goto Label_021A;

                    case BIFFRECORDTYPE.NUMBER:
                        goto Label_0222;

                    case BIFFRECORDTYPE.STRING:
                        return new XlsBiffFormulaString(bytes, offset);

                    case BIFFRECORDTYPE.ROW:
                        return new XlsBiffRow(bytes, offset);

                    case BIFFRECORDTYPE.BOF_V3:
                        goto Label_01C2;

                    case BIFFRECORDTYPE.INDEX:
                        return new XlsBiffIndex(bytes, offset);
                }
                goto Label_02B2;
            }
            if (biffrecordtype <= BIFFRECORDTYPE.FORMULA)
            {
                switch (biffrecordtype)
                {
                    case BIFFRECORDTYPE.RK:
                        return new XlsBiffRKCell(bytes, offset);

                    case BIFFRECORDTYPE.FORMULA:
                        goto Label_023A;
                }
                goto Label_02B2;
            }
            if (biffrecordtype != BIFFRECORDTYPE.BOF_V4)
            {
                if (biffrecordtype == BIFFRECORDTYPE.FORMAT)
                {
                    goto Label_0242;
                }
                if (biffrecordtype != BIFFRECORDTYPE.BOF)
                {
                    goto Label_02B2;
                }
            }
        Label_01C2:
            return new XlsBiffBOF(bytes, offset);
        Label_01FA:
            return new XlsBiffBlankCell(bytes, offset);
        Label_020A:
            return new XlsBiffLabelCell(bytes, offset);
        Label_021A:
            return new XlsBiffIntegerCell(bytes, offset);
        Label_0222:
            return new XlsBiffNumberCell(bytes, offset);
        Label_023A:
            return new XlsBiffFormulaCell(bytes, offset);
        Label_0242:
            return new XlsBiffFormatString(bytes, offset);
        Label_02B2:
            return new XlsBiffRecord(bytes, offset);
        }

        public byte[] ReadArray(int offset, int size)
        {
            byte[] dst = new byte[size];
            Buffer.BlockCopy(this.m_bytes, this.m_readoffset + offset, dst, 0, size);
            return dst;
        }

        public byte ReadByte(int offset)
        {
            return Buffer.GetByte(this.m_bytes, this.m_readoffset + offset);
        }

        public double ReadDouble(int offset)
        {
            return BitConverter.ToDouble(this.m_bytes, this.m_readoffset + offset);
        }

        public float ReadFloat(int offset)
        {
            return BitConverter.ToSingle(this.m_bytes, this.m_readoffset + offset);
        }

        public short ReadInt16(int offset)
        {
            return BitConverter.ToInt16(this.m_bytes, this.m_readoffset + offset);
        }

        public int ReadInt32(int offset)
        {
            return BitConverter.ToInt32(this.m_bytes, this.m_readoffset + offset);
        }

        public long ReadInt64(int offset)
        {
            return BitConverter.ToInt64(this.m_bytes, this.m_readoffset + offset);
        }

        public ushort ReadUInt16(int offset)
        {
            return BitConverter.ToUInt16(this.m_bytes, this.m_readoffset + offset);
        }

        public uint ReadUInt32(int offset)
        {
            return BitConverter.ToUInt32(this.m_bytes, this.m_readoffset + offset);
        }

        public ulong ReadUInt64(int offset)
        {
            return BitConverter.ToUInt64(this.m_bytes, this.m_readoffset + offset);
        }

        internal byte[] Bytes
        {
            get
            {
                return this.m_bytes;
            }
        }

        public BIFFRECORDTYPE ID
        {
            get
            {
                return (BIFFRECORDTYPE) BitConverter.ToUInt16(this.m_bytes, this.m_readoffset - 4);
            }
        }

        public bool IsCell
        {
            get
            {
                bool flag = false;
                BIFFRECORDTYPE iD = this.ID;
                if (iD <= BIFFRECORDTYPE.LABELSST)
                {
                    switch (iD)
                    {
                        case BIFFRECORDTYPE.MULRK:
                        case BIFFRECORDTYPE.MULBLANK:
                        case BIFFRECORDTYPE.LABELSST:
                            goto Label_005F;
                    }
                    return flag;
                }
                switch (iD)
                {
                    case BIFFRECORDTYPE.BLANK:
                    case BIFFRECORDTYPE.NUMBER:
                    case BIFFRECORDTYPE.BOOLERR:
                    case BIFFRECORDTYPE.RK:
                    case BIFFRECORDTYPE.FORMULA:
                        break;

                    case BIFFRECORDTYPE.INTEGER:
                    case BIFFRECORDTYPE.LABEL:
                        return flag;

                    default:
                        return flag;
                }
            Label_005F:
                return true;
            }
        }

        internal int Offset
        {
            get
            {
                return (this.m_readoffset - 4);
            }
        }

        public ushort RecordSize
        {
            get
            {
                return BitConverter.ToUInt16(this.m_bytes, this.m_readoffset - 2);
            }
        }

        public int Size
        {
            get
            {
                return (4 + this.RecordSize);
            }
        }
    }
}

