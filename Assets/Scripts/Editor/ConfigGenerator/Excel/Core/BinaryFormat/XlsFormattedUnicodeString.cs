namespace Excel.Core.BinaryFormat
{
    using System;
    using System.Text;

    internal class XlsFormattedUnicodeString
    {
        protected byte[] m_bytes;
        protected uint m_offset;

        public XlsFormattedUnicodeString(byte[] bytes, uint offset)
        {
            this.m_bytes = bytes;
            this.m_offset = offset;
        }

        private uint ByteCount
        {
            get
            {
                return (uint) (this.CharacterCount * (this.IsMultiByte ? 2 : 1));
            }
        }

        public ushort CharacterCount
        {
            get
            {
                return BitConverter.ToUInt16(this.m_bytes, (int) this.m_offset);
            }
        }

        public uint ExtendedStringSize
        {
            get
            {
                if (!this.HasExtString)
                {
                    return 0;
                }
                return BitConverter.ToUInt16(this.m_bytes, ((int) this.m_offset) + (this.HasFormatting ? 5 : 3));
            }
        }

        public FormattedUnicodeStringFlags Flags
        {
            get
            {
                return (FormattedUnicodeStringFlags) Buffer.GetByte(this.m_bytes, ((int) this.m_offset) + 2);
            }
        }

        public ushort FormatCount
        {
            get
            {
                if (!this.HasFormatting)
                {
                    return 0;
                }
                return BitConverter.ToUInt16(this.m_bytes, ((int) this.m_offset) + 3);
            }
        }

        public bool HasExtString
        {
            get
            {
                return false;
            }
        }

        public bool HasFormatting
        {
            get
            {
                return (((byte) (this.Flags & FormattedUnicodeStringFlags.HasFormatting)) == 8);
            }
        }

        public uint HeadSize
        {
            get
            {
                return (uint) (((this.HasFormatting ? 2 : 0) + (this.HasExtString ? 4 : 0)) + 3);
            }
        }

        public bool IsMultiByte
        {
            get
            {
                return (((byte) (this.Flags & FormattedUnicodeStringFlags.MultiByte)) == 1);
            }
        }

        public uint Size
        {
            get
            {
                uint num = (uint) (((this.HasFormatting ? (2 + (this.FormatCount * 4)) : 0) + (this.HasExtString ? (4 + this.ExtendedStringSize) : 0)) + 3);
                if (!this.IsMultiByte)
                {
                    return (num + this.CharacterCount);
                }
                return (num + ((uint) (this.CharacterCount * 2)));
            }
        }

        public uint TailSize
        {
            get
            {
                return ((this.HasFormatting ? ((uint) (4 * this.FormatCount)) : 0) + (this.HasExtString ? this.ExtendedStringSize : 0));
            }
        }

        public string Value
        {
            get
            {
                if (!this.IsMultiByte)
                {
                    return Encoding.Default.GetString(this.m_bytes, (int) (this.m_offset + this.HeadSize), (int) this.ByteCount);
                }
                return Encoding.Unicode.GetString(this.m_bytes, (int) (this.m_offset + this.HeadSize), (int) this.ByteCount);
            }
        }

        [Flags]
        public enum FormattedUnicodeStringFlags : byte
        {
            HasExtendedString = 4,
            HasFormatting = 8,
            MultiByte = 1
        }
    }
}

