using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fclp;

namespace Stitcher.CommandLine
{
    internal class ApplicationArguments : IApplicationArguments
    {
        private List<string> source;
        private string destination;
        private bool inPlace;
        private bool help;
        private string[] args;
        private FluentCommandLineParser parser;

        public ApplicationArguments(string[] args)
        {
            this.args = args;
            source = new List<string>();
        }

        public string[] SourceFiles
        {
            get { return this.source.ToArray(); }
        }

        public bool AskedForHelp
        {
            get { return this.help; }
        }

        public string ErrorMessage
        {
            get { return "Use /? or --help for help."; }
        }

        public string HelpMessage
        {
            get {return GetHelpMessage(); }
        }

        public string DestinationFile
        {
            get { return this.destination; }
        }

        public bool StitchInPlace
        {
            get { return this.inPlace; }
        }

        public bool Parse()
        {
            FluentCommandLineParser p = CreateParser();

            var result = p.Parse(args);

            if (result.HasErrors)
                return false;

            return true;
        }

        private FluentCommandLineParser CreateParser()
        {
            this.parser = new FluentCommandLineParser();

            DefineSourceSwitch();
            DefineDestinationSwitch();
            DefineInplaceSwitch();
            DefineHelpSwitch();

            return parser;
        }

        private void DefineHelpSwitch()
        {
            parser.Setup<bool>('?', "help")
                .Callback(helpFlag => this.help = helpFlag)
                .SetDefault(false);
        }

        private void DefineInplaceSwitch()
        {
            parser.Setup<bool>("inplace")
                .Callback(inplaceFlag => this.inPlace = inplaceFlag)
                .SetDefault(false);
        }

        private void DefineDestinationSwitch()
        {
            parser.Setup<string>('d', "dest")
                .Callback(destFile => this.destination = destFile)
                .SetDefault(string.Empty);
        }

        private void DefineSourceSwitch()
        {
            parser.Setup<List<string>>('s', "source")
                .Callback(sourceFiles => this.source = sourceFiles)
                .Required();
        }

        private string GetHelpMessage()
        {
            HelpTextFormatter formatter = new HelpTextFormatter();
            formatter.ProgramName = Program.Name;
            formatter.ProgramVersion = Program.GetVersion();
            formatter.CopyrightMessage = 
                string.Format("Copyright (c) 2015 Mohammad Riazat. {0}Released under The MIT License.", Environment.NewLine);
            formatter.CommandSynopsis = "STITCH /source:files [/dest:file] [/inplace]";

            formatter.AddSwitch(
                new ProgramSwitch("/source:files", "/s:files", "It indicates the name of files to stitch."));
            formatter.AddSwitch(
                new ProgramSwitch("/dest:file", "/d:file", "It indicates the destination file name. If this switch is not used, it implies using <inplace> switch."));
            formatter.AddSwitch(
                new ProgramSwitch("/inplace", "", "When enabled the program ignores the <dest> switch, and it stitches the files to the first source file."));
            formatter.AddSwitch(
                new ProgramSwitch("/?", "--help", "Shows instructions on how to use the program."));

            return formatter.GetHelpMessage();
        }
    }
}
