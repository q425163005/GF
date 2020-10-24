namespace Excel.Core.BinaryFormat
{
    using System;

    internal class XlsWorksheet
    {
        private XlsBiffSimpleValueRecord m_CalcCount;
        private XlsBiffSimpleValueRecord m_CalcMode;
        private readonly uint m_dataOffset;
        private XlsBiffRecord m_Delta;
        private XlsBiffDimensions m_Dimensions;
        private readonly int m_Index;
        private XlsBiffSimpleValueRecord m_Iteration;
        private readonly string m_Name = string.Empty;
        private XlsBiffSimpleValueRecord m_RefMode;
        private XlsBiffRecord m_Window;

        public XlsWorksheet(int index, XlsBiffBoundSheet refSheet)
        {
            this.m_Index = index;
            this.m_Name = refSheet.SheetName;
            this.m_dataOffset = refSheet.StartOffset;
        }

        public XlsBiffSimpleValueRecord CalcCount
        {
            get
            {
                return this.m_CalcCount;
            }
            set
            {
                this.m_CalcCount = value;
            }
        }

        public XlsBiffSimpleValueRecord CalcMode
        {
            get
            {
                return this.m_CalcMode;
            }
            set
            {
                this.m_CalcMode = value;
            }
        }

        public uint DataOffset
        {
            get
            {
                return this.m_dataOffset;
            }
        }

        public XlsBiffRecord Delta
        {
            get
            {
                return this.m_Delta;
            }
            set
            {
                this.m_Delta = value;
            }
        }

        public XlsBiffDimensions Dimensions
        {
            get
            {
                return this.m_Dimensions;
            }
            set
            {
                this.m_Dimensions = value;
            }
        }

        public int Index
        {
            get
            {
                return this.m_Index;
            }
        }

        public XlsBiffSimpleValueRecord Iteration
        {
            get
            {
                return this.m_Iteration;
            }
            set
            {
                this.m_Iteration = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public XlsBiffSimpleValueRecord RefMode
        {
            get
            {
                return this.m_RefMode;
            }
            set
            {
                this.m_RefMode = value;
            }
        }

        public XlsBiffRecord Window
        {
            get
            {
                return this.m_Window;
            }
            set
            {
                this.m_Window = value;
            }
        }
    }
}

