using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;

namespace JetCode.Storage
{
    public class SerializableObjectStorage : StorageBase
    {
        public SerializableObjectStorage(string file)
            : base(file)
        {
        }

        public SerializableObjectStorage(string file, IsolatedStorageFile storageFile)
            : base(file, storageFile)
        {
        }

        public object Read()
        {
            if (!base.IsFileExist())
                return null;

            using (FileStream stream = base.GetReadFileStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }

        public void Save(object obj)
        {
            using (FileStream stream = base.GetWriteFileStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
            }
        }
    }
}