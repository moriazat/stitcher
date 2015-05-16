using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stitcher.CommandLine
{
    class ProgramSwitch
    {
        private string switchFormat;
        private string alternateFormat;
        private string description;

        public ProgramSwitch(string switchFormat, string alternateFormat, string description)
        {
            this.switchFormat = switchFormat;
            this.alternateFormat = alternateFormat;
            this.description = description;
        }

        public string PrimaryFormat
        {
            get { return this.switchFormat; }
        }

        public string AlternateFormat
        {
            get { return this.alternateFormat; }
        }

        public string Description
        {
            get { return this.description; }
        }
    }
}
