namespace Excel.Core.BinaryFormat
{
    using System;
    using System.Collections.Generic;

    internal class XlsWorkbookGlobals
    {
        private XlsBiffSimpleValueRecord m_Backup;
        private XlsBiffSimpleValueRecord m_CodePage;
        private XlsBiffRecord m_Country;
        private XlsBiffRecord m_DSF;
        private readonly List<XlsBiffRecord> m_ExtendedFormats = new List<XlsBiffRecord>();
        private XlsBiffRecord m_ExtSST;
        private readonly List<XlsBiffRecord> m_Fonts = new List<XlsBiffRecord>();
        private readonly Dictionary<ushort, XlsBiffFormatString> m_Formats = new Dictionary<ushort, XlsBiffFormatString>();
        private XlsBiffInterfaceHdr m_InterfaceHdr;
        private XlsBiffRecord m_MMS;
        private readonly List<XlsBiffBoundSheet> m_Sheets = new List<XlsBiffBoundSheet>();
        private XlsBiffSST m_SST;
        private readonly List<XlsBiffRecord> m_Styles = new List<XlsBiffRecord>();
        private XlsBiffRecord m_WriteAccess;

        public XlsBiffSimpleValueRecord Backup
        {
            get
            {
                return this.m_Backup;
            }
            set
            {
                this.m_Backup = value;
            }
        }

        public XlsBiffSimpleValueRecord CodePage
        {
            get
            {
                return this.m_CodePage;
            }
            set
            {
                this.m_CodePage = value;
            }
        }

        public XlsBiffRecord Country
        {
            get
            {
                return this.m_Country;
            }
            set
            {
                this.m_Country = value;
            }
        }

        public XlsBiffRecord DSF
        {
            get
            {
                return this.m_DSF;
            }
            set
            {
                this.m_DSF = value;
            }
        }

        public List<XlsBiffRecord> ExtendedFormats
        {
            get
            {
                return this.m_ExtendedFormats;
            }
        }

        public XlsBiffRecord ExtSST
        {
            get
            {
                return this.m_ExtSST;
            }
            set
            {
                this.m_ExtSST = value;
            }
        }

        public List<XlsBiffRecord> Fonts
        {
            get
            {
                return this.m_Fonts;
            }
        }

        public Dictionary<ushort, XlsBiffFormatString> Formats
        {
            get
            {
                return this.m_Formats;
            }
        }

        public XlsBiffInterfaceHdr InterfaceHdr
        {
            get
            {
                return this.m_InterfaceHdr;
            }
            set
            {
                this.m_InterfaceHdr = value;
            }
        }

        public XlsBiffRecord MMS
        {
            get
            {
                return this.m_MMS;
            }
            set
            {
                this.m_MMS = value;
            }
        }

        public List<XlsBiffBoundSheet> Sheets
        {
            get
            {
                return this.m_Sheets;
            }
        }

        public XlsBiffSST SST
        {
            get
            {
                return this.m_SST;
            }
            set
            {
                this.m_SST = value;
            }
        }

        public List<XlsBiffRecord> Styles
        {
            get
            {
                return this.m_Styles;
            }
        }

        public XlsBiffRecord WriteAccess
        {
            get
            {
                return this.m_WriteAccess;
            }
            set
            {
                this.m_WriteAccess = value;
            }
        }
    }
}

