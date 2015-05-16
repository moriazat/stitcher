using System.IO;

namespace Stitcher.Core
{
    public class FileReaderWriterFactory : IReaderWriterFactory
    {
        public BinaryReader CreateReader(string path)
        {
            FileStream fs = File.OpenRead(path);
            return new BinaryReader(fs);
        }

        public BinaryWriter CreateWriter(string path)
        {
            FileStream fs = File.OpenWrite(path);
            return new BinaryWriter(fs);
        }

        public BinaryWriter CreateWriter(string path, long fileLength)
        {
            FileStream fs = File.OpenWrite(path);
            fs.SetLength(fileLength);
            return new BinaryWriter(fs);
        }
    }
}
