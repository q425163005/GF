namespace Excel
{
    using System;
    using System.IO;

    public static class ExcelReaderFactory
    {
        public static IExcelDataReader CreateBinaryReader(Stream fileStream)
        {
            IExcelDataReader reader = new ExcelBinaryReader();
            reader.Initialize(fileStream);
            return reader;
        }

        public static IExcelDataReader CreateOpenXmlReader(Stream fileStream)
        {
            IExcelDataReader reader = new ExcelOpenXmlReader();
            reader.Initialize(fileStream);
            return reader;
        }
    }
}

