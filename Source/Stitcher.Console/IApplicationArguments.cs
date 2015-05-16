using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stitcher.CommandLine
{
    internal interface IApplicationArguments
    {
        string ErrorMessage { get; }

        bool AskedForHelp { get; }

        string HelpMessage { get; }

        bool Parse();
    }
}
