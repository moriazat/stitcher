using System;

namespace Stitcher.Core
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(float progress)
        {
            this.Progress = progress;
        }

        public float Progress
        {
            get; 
            set;
        }
    }
}
