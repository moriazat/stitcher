using System.IO;

namespace Stitcher.Core
{
    public interface IReaderWriterFactory
    {
        BinaryReader CreateReader(string path);

        BinaryWriter CreateWriter(string path);

        BinaryWriter CreateWriter(string path, long fileLength);
    }
}
