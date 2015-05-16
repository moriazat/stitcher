using System;
using System.IO;
using System.Diagnostics;

namespace Stitcher.Core
{
    public class ConcatenationManager
    {
        private BinaryWriter writer;
        private BinaryReader reader;
        private int bufferSize;
        private float progess;
        private Concatenator fileConcat;
        private long totalRead;
        private FileBundle bundle;
        private IReaderWriterFactory factory;

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        public ConcatenationManager(IReaderWriterFactory factory, FileBundle bundle)
        {
            this.bundle = bundle;
            this.factory = factory;
            bufferSize = 131072; // 128 KB
        }

        public int BufferSize
        {
            get { return this.bufferSize; }

            set { this.bufferSize = value; }
        }

        public float Progress
        {
            get { return this.progess; }
        }

        public long TotalBytesRead
        {
            get { return this.totalRead; }
        }

        public void Start()
        {
            this.totalRead = 0;

            this.writer = factory.CreateWriter(bundle.GetDestinationFile());

            while (!bundle.AllFilesProcessed)
            {
                reader = factory.CreateReader(bundle.GetNextSourceFile());
                fileConcat = new Concatenator(reader, this.writer, this.bufferSize);
                fileConcat.ProgressChanged += fileConcat_ProgressChanged;
                fileConcat.Concatenate();
                this.totalRead += fileConcat.TotalBytesRead;
            }
        }

        private void OnProgressChanged()
        {
            if (ProgressChanged != null)
                this.ProgressChanged(this, new ProgressEventArgs(this.progess));
        }

        private void fileConcat_ProgressChanged(object sender, EventArgs e)
        {
            long total = this.totalRead + this.fileConcat.TotalBytesRead;

            Debug.Assert((this.bundle.DestinationFileSize != 0), "Destination file size can't be 0.");

            this.progess = ((float)total / (float)bundle.DestinationFileSize) * 100;
            OnProgressChanged();
        }
    }
}
