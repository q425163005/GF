namespace Excel.Core.OpenXmlFormat
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    internal class XlsxWorkbook
    {
        private XlsxSST _SST;
        private XlsxStyles _Styles;
        private const string A_id = "Id";
        private const string A_name = "name";
        private const string A_rid = "r:id";
        private const string A_sheetId = "sheetId";
        private const string A_target = "Target";
        private const string N_cellXfs = "cellXfs";
        private const string N_numFmts = "numFmts";
        private const string N_rel = "Relationship";
        private const string N_sheet = "sheet";
        private const string N_si = "si";
        private const string N_t = "t";
        private List<XlsxWorksheet> sheets;

        private XlsxWorkbook()
        {
        }

        public XlsxWorkbook(Stream workbookStream, Stream relsStream, Stream sharedStringsStream, Stream stylesStream)
        {
            if (workbookStream == null)
            {
                throw new ArgumentNullException();
            }
            this.ReadWorkbook(workbookStream);
            this.ReadWorkbookRels(relsStream);
            this.ReadSharedStrings(sharedStringsStream);
            this.ReadStyles(stylesStream);
        }

        private void ReadSharedStrings(Stream xmlFileStream)
        {
            if (xmlFileStream != null)
            {
                this._SST = new XlsxSST();
                using (XmlReader reader = XmlReader.Create(xmlFileStream))
                {
                    bool flag = false;
                    string item = "";
                    while (reader.Read())
                    {
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "si"))
                        {
                            if (flag)
                            {
                                this._SST.Add(item);
                            }
                            else
                            {
                                flag = true;
                            }
                            item = "";
                        }
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "t"))
                        {
                            item = item + reader.ReadElementContentAsString();
                        }
                    }
                    if (flag)
                    {
                        this._SST.Add(item);
                    }
                    xmlFileStream.Close();
                }
            }
        }

        private void ReadStyles(Stream xmlFileStream)
        {
            if (xmlFileStream != null)
            {
                this._Styles = new XlsxStyles();
                bool flag = false;
                using (XmlReader reader = XmlReader.Create(xmlFileStream))
                {
                    while (reader.Read())
                    {
                        if ((!flag && (reader.NodeType == XmlNodeType.Element)) && (reader.Name == "numFmts"))
                        {
                            while (reader.Read())
                            {
                                if ((reader.NodeType == XmlNodeType.Element) && (reader.Depth == 1))
                                {
                                    break;
                                }
                                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "numFmt"))
                                {
                                    this._Styles.NumFmts.Add(new XlsxNumFmt(int.Parse(reader.GetAttribute("numFmtId")), reader.GetAttribute("formatCode")));
                                }
                            }
                            flag = true;
                        }
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cellXfs"))
                        {
                            while (reader.Read())
                            {
                                if ((reader.NodeType == XmlNodeType.Element) && (reader.Depth == 1))
                                {
                                    break;
                                }
                                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "xf"))
                                {
                                    this._Styles.CellXfs.Add(new XlsxXf(int.Parse(reader.GetAttribute("xfId")), int.Parse(reader.GetAttribute("numFmtId")), reader.GetAttribute("applyNumberFormat")));
                                }
                            }
                            break;
                        }
                    }
                    xmlFileStream.Close();
                }
            }
        }

        private void ReadWorkbook(Stream xmlFileStream)
        {
            this.sheets = new List<XlsxWorksheet>();
            using (XmlReader reader = XmlReader.Create(xmlFileStream))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "sheet"))
                    {
                        this.sheets.Add(new XlsxWorksheet(reader.GetAttribute("name"), int.Parse(reader.GetAttribute("sheetId")), reader.GetAttribute("r:id")));
                    }
                }
                xmlFileStream.Close();
            }
        }

        private void ReadWorkbookRels(Stream xmlFileStream)
        {
            using (XmlReader reader = XmlReader.Create(xmlFileStream))
            {
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "Relationship"))
                    {
                        string attribute = reader.GetAttribute("Id");
                        for (int i = 0; i < this.sheets.Count; i++)
                        {
                            XlsxWorksheet worksheet = this.sheets[i];
                            if (worksheet.RID == attribute)
                            {
                                worksheet.Path = reader.GetAttribute("Target");
                                this.sheets[i] = worksheet;
                                continue;
                            }
                        }
                    }
                }
                xmlFileStream.Close();
            }
        }

        public List<XlsxWorksheet> Sheets
        {
            get
            {
                return this.sheets;
            }
            set
            {
                this.sheets = value;
            }
        }

        public XlsxSST SST
        {
            get
            {
                return this._SST;
            }
        }

        public XlsxStyles Styles
        {
            get
            {
                return this._Styles;
            }
        }
    }
}

