using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace JetCode
{
    public static class Compression
    {
        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(stream, CompressionMode.Compress, true))
                {
                    zip.Write(data, 0, data.Length);
                    zip.Close();
                }
                return stream.ToArray();
            }
        }

        public static byte[] Compress(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return Compress(stream.ToArray());
            }
        }

        private static MemoryStream Decompress(byte[] data)
        {
            MemoryStream stream = new MemoryStream();

            using (MemoryStream memory = new MemoryStream())
            {
                memory.Write(data, 0, data.Length);
                memory.Position = 0L;
                using (GZipStream zip = new GZipStream(memory, CompressionMode.Decompress, true))
                {
                    int num;
                    byte[] buffer = new byte[4096];
                    while ((num = zip.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        stream.Write(buffer, 0, num);
                    }
                }
            }

            stream.Flush();
            return stream;
        }

        public static byte[] DecompressToByteArray(byte[] data)
        {
            using (MemoryStream stream = Decompress(data))
            {
                return stream.ToArray();
            }
        }

        public static object DecompressToObject(byte[] data)
        {
            using (MemoryStream stream = Decompress(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0L;
                return formatter.Deserialize(stream);
            }
        }
    }
}
