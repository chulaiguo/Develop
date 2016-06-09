using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml;

namespace JetCode.Storage
{
    public abstract class StorageBase : IDisposable
    {
        private readonly string _file;
        private readonly IsolatedStorageFile _storageFile;

        public StorageBase(string file)
        {
            this._file = file;
            this._storageFile = IsolatedStorageFile.GetMachineStoreForDomain();
        }

        public StorageBase(string file, IsolatedStorageFile storageFile)
        {
            this._file = file;
            this._storageFile = storageFile;
        }

        public bool IsFileExist()
        {
            return (this._storageFile.GetFileNames(this._file).Length != 0);
        }

        public void DeleteFile()
        {
            if (this.IsFileExist())
            {
                this._storageFile.DeleteFile(this._file);
            }
        }

        public void Dispose()
        {
            this._storageFile.Close();
            this._storageFile.Dispose();
        }

        protected FileStream GetReadFileStream()
        {
            return new IsolatedStorageFileStream(this._file, FileMode.Open, FileAccess.Read, FileShare.Read, this._storageFile);
        }

        protected FileStream GetWriteFileStream()
        {
            return new IsolatedStorageFileStream(this._file, FileMode.Create, this._storageFile);
        }

        protected StreamReader GetTextReader()
        {
            return new StreamReader(this.GetReadFileStream());
        }

        protected StreamWriter GetTextWriter()
        {
            return new StreamWriter(this.GetWriteFileStream());
        }

        protected XmlTextReader GetXmlTextReader()
        {
            return new XmlTextReader(this.GetReadFileStream());
        }

        protected XmlTextWriter GetXmlTextWriter(string file)
        {
            XmlTextWriter writer = new XmlTextWriter(this.GetWriteFileStream(), Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            return writer;
        }
    }
}