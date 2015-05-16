using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Stitcher.Core
{
    public class CurrentFileChangedEventArgs : EventArgs
    {
        private string path;

        public CurrentFileChangedEventArgs(string filePath)
        {
            this.path = filePath;
        }

        public string CurrentFileName
        {
            get
            {
                return Path.GetFileName(this.path);
            }
        }

        public string CurrentFilePath
        {
            get
            {
                return this.path;
            }
        }
    }
}
