namespace Excel.Core
{
    using ICSharpCode.SharpZipLib.Zip;
    using System;
    using System.Collections;
    using System.IO;

    public class ZipWorker : IDisposable
    {
        private string _exceptionMessage;
        private string _format = "xml";
        private bool _isCleaned;
        private bool _isValid;
        private string _tempEnv = Path.GetTempPath();
        private string _tempPath;
        private string _xlPath;
        private byte[] buffer;
        private bool disposed;
        private const string FILE_rels = "workbook.{0}.rels";
        private const string FILE_sharedStrings = "sharedStrings.{0}";
        private const string FILE_sheet = "sheet{0}.{1}";
        private const string FILE_styles = "styles.{0}";
        private const string FILE_workbook = "workbook.{0}";
        private const string FOLDER_rels = "_rels";
        private const string FOLDER_worksheets = "worksheets";
        private const string FOLDER_xl = "xl";
        private const string TMP = "TMP_Z";

        private bool CheckFolderTree()
        {
            this._xlPath = Path.Combine(this._tempPath, "xl");
            return (((Directory.Exists(this._xlPath) && Directory.Exists(Path.Combine(this._xlPath, "worksheets"))) && File.Exists(Path.Combine(this._xlPath, "workbook.{0}"))) && File.Exists(Path.Combine(this._xlPath, "styles.{0}")));
        }

        private void CleanFromTemp(bool catchIoError)
        {
            if (!string.IsNullOrEmpty(this._tempPath))
            {
                this._isCleaned = true;
                try
                {
                    if (Directory.Exists(this._tempPath))
                    {
                        Directory.Delete(this._tempPath, true);
                    }
                }
                catch (IOException)
                {
                    if (!catchIoError)
                    {
                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing && !this._isCleaned)
                {
                    this.CleanFromTemp(false);
                }
                this.buffer = null;
                this.disposed = true;
            }
        }

        public bool Extract(Stream fileStream)
        {
            if (fileStream == null)
            {
                return false;
            }
            this.CleanFromTemp(false);
            this.NewTempPath();
            this._isValid = true;
            ZipFile zipFile = null;
            try
            {
                zipFile = new ZipFile(fileStream);
                IEnumerator enumerator = zipFile.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ZipEntry current = (ZipEntry) enumerator.Current;
                    this.ExtractZipEntry(zipFile, current);
                }
            }
            catch (Exception exception)
            {
                this._isValid = false;
                this._exceptionMessage = exception.Message;
                this.CleanFromTemp(true);
            }
            finally
            {
                fileStream.Close();
                if (zipFile != null)
                {
                    zipFile.Close();
                }
            }
            if (!this._isValid)
            {
                return false;
            }
            return this.CheckFolderTree();
        }

        private void ExtractZipEntry(ZipFile zipFile, ZipEntry entry)
        {
            if (entry.IsCompressionMethodSupported() && !string.IsNullOrEmpty(entry.Name))
            {
                string path = Path.Combine(this._tempPath, entry.Name);
                string str2 = entry.IsDirectory ? path : Path.GetDirectoryName(Path.GetFullPath(path));
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }
                if (entry.IsFile)
                {
                    using (FileStream stream = File.Create(path))
                    {
                        if (this.buffer == null)
                        {
                            this.buffer = new byte[0x1000];
                        }
                        using (Stream stream2 = zipFile.GetInputStream(entry))
                        {
                            int num;
                            while ((num = stream2.Read(this.buffer, 0, this.buffer.Length)) > 0)
                            {
                                stream.Write(this.buffer, 0, num);
                            }
                        }
                        stream.Flush();
                    }
                }
            }
        }

        ~ZipWorker()
        {
            this.Dispose(false);
        }

        public Stream GetSharedStringsStream()
        {
            return GetStream(Path.Combine(this._xlPath, string.Format("sharedStrings.{0}", this._format)));
        }

        private static Stream GetStream(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.Open(filePath, FileMode.Open, FileAccess.Read);
            }
            return null;
        }

        public Stream GetStylesStream()
        {
            return GetStream(Path.Combine(this._xlPath, string.Format("styles.{0}", this._format)));
        }

        public Stream GetWorkbookRelsStream()
        {
            return GetStream(Path.Combine(this._xlPath, Path.Combine("_rels", string.Format("workbook.{0}.rels", this._format))));
        }

        public Stream GetWorkbookStream()
        {
            return GetStream(Path.Combine(this._xlPath, string.Format("workbook.{0}", this._format)));
        }

        public Stream GetWorksheetStream(int sheetId)
        {
            return GetStream(Path.Combine(Path.Combine(this._xlPath, "worksheets"), string.Format("sheet{0}.{1}", sheetId, this._format)));
        }

        public Stream GetWorksheetStream(string sheetPath)
        {
            return GetStream(Path.Combine(this._xlPath, sheetPath));
        }

        private void NewTempPath()
        {
            this._tempPath = Path.Combine(this._tempEnv, "TMP_Z" + DateTime.Now.ToFileTimeUtc().ToString());
            this._isCleaned = false;
            Directory.CreateDirectory(this._tempPath);
        }

        public string ExceptionMessage
        {
            get
            {
                return this._exceptionMessage;
            }
        }

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
        }

        public string TempPath
        {
            get
            {
                return this._tempPath;
            }
        }
    }
}

