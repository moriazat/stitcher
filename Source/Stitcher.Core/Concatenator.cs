using System;
using System.IO;

namespace Stitcher.Core
{
    internal class Concatenator : IDisposable
    {
        private int bufferSize;
        private BinaryReader reader;
        private BinaryWriter writer;
        private long totalRead;

        public event EventHandler ProgressChanged;

        public Concatenator(BinaryReader br, BinaryWriter bw, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.reader = br;
            this.writer = bw;
        }

        public int BufferSize
        {
            get { return this.bufferSize; }

            set { this.bufferSize = value; }
        }

        public long TotalBytesRead
        {
            get { return this.totalRead; }
        }

        public void Concatenate()
        {
            byte[] buffer = new byte[bufferSize];
            this.totalRead = 0;
            int read;
            long length = reader.BaseStream.Length;

            while (totalRead < length)
            {
                read = this.reader.Read(buffer, 0, bufferSize);
                totalRead += read;
                this.writer.Write(buffer, 0, read);
                OnProgressChanged();
            }

            this.reader.Close();
        }

        public void Dispose()
        {
            //if (this.reader != null)
            //    this.reader.Dispose();
        }

        private void OnProgressChanged()
        {
            if (this.ProgressChanged != null)
                ProgressChanged(this, new EventArgs());
        }
    }
}
