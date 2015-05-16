using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Stitcher.Core
{
    public class FileBundle
    {
        private bool inPlace;
        private string[] sources;
        private string destination;
        private int currentFileIndex;
        private long destinationSize;

        public event EventHandler<CurrentFileChangedEventArgs> CurrentFileChanged;

        public FileBundle(string[] sourceFiles) 
        {
            this.sources = sourceFiles; 
            this.inPlace = true;
            currentFileIndex = 0;
            CalculateDestinationFileSize();
        }

        public FileBundle(string[] sourceFiles, string destinationFile)
        {
            this.sources = sourceFiles;
            this.inPlace = false;
            this.destination = destinationFile;
            currentFileIndex = -1;
            CalculateDestinationFileSize();
        }

        public bool InPlaceConcatenation
        {
            get { return this.inPlace; }
        }

        public int SourceFilesCount
        {
            get { return this.sources.Length; }
        }

        public bool AllFilesProcessed
        {
            get
            {
                if (currentFileIndex == sources.Length - 1)
                    return true;
                else
                    return false;
            }
        }

        public long DestinationFileSize
        {
            get { return this.destinationSize; }
        }

        public string GetDestinationFile()
        {
            if (inPlace)
                return sources[0];
            else
                return destination;
        }

        public string GetNextSourceFile()
        {
            currentFileIndex++;
            OnCurrentFileChanged();
            return this.sources[this.currentFileIndex];
        }

        private void CalculateDestinationFileSize()
        {
            FileInfo info;

            foreach (string s in this.sources)
            {
                info = new FileInfo(s);
                this.destinationSize += info.Length;
            }
        }

        private void OnCurrentFileChanged()
        {
            if (CurrentFileChanged != null)
            {
                string fileName = Path.GetFileName(this.sources[this.currentFileIndex]);
                this.CurrentFileChanged(this, new CurrentFileChangedEventArgs(fileName));
            }
        }
    }
}
