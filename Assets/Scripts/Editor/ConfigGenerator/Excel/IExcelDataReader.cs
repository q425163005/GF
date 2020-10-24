namespace Excel
{
    using System;
    using System.Data;
    using System.IO;

    public interface IExcelDataReader : IDataReader, IDisposable, IDataRecord
    {
        DataSet AsDataSet();
        DataSet AsDataSet(bool convertOADateTime);
        void Initialize(Stream fileStream);

        string ExceptionMessage { get; }

        bool IsFirstRowAsColumnNames { get; set; }

        bool IsValid { get; }

        string Name { get; }

        int ResultsCount { get; }
    }
}

