using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stitcher.Core;

namespace Stitcher.CommandLine
{
    class ConsoleApplication
    {
        private ApplicationArguments arguments;
        private FileBundle bundle;
        private ConcatenationManager manager;
        private ProgramScreen screen;
        private bool firstTime;
        private string currFile;
        private string version;

        public ConsoleApplication(ApplicationArguments args, ProgramScreen screen)
        {
            this.arguments = args;
            this.screen = screen;
            this.firstTime = true;
        }

        public string AppVersion
        {
            set { this.version = value; }
        }

        public void Run()
        {
            bool result = this.arguments.Parse();

            if (arguments.AskedForHelp)
            {
                ShowHelp();
                return ;
            }

            if (!result)
            {
                this.screen.PrintMessage(arguments.ErrorMessage);
                return;
            }

            TimeSpan duration = Stitch();

            this.screen.PrintMessage(string.Format("<{0}> Appended", currFile));
            Summarize(duration);
        }

        private void Summarize(TimeSpan duration)
        {
            this.screen.PrintMessage(string.Empty);
            this.screen.PrintMessage(
                string.Format("Stitching {0} files completed successfully.", bundle.SourceFilesCount));
            this.screen.PrintMessage(
                string.Format("Duration: {0:00}:{1:00}.{2:##} min.", duration.Minutes, duration.Seconds, duration.Milliseconds));
            this.screen.PrintFinalMessage(
                string.Format("Total bytes written: {0:#,#}", bundle.DestinationFileSize), false);
        }

        private TimeSpan Stitch()
        {
            CreateBundle();
            CreateManager();
            DateTime start = DateTime.Now;
            screen.PrintHeader("Binary File Stitcher", this.version);
            this.manager.Start();
            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            return duration;
        }

        private void CreateManager()
        {
            this.manager = new ConcatenationManager(new FileReaderWriterFactory(), this.bundle);
            this.manager.ProgressChanged += manager_ProgressChanged;
        }

        private void CreateBundle()
        {
            if (this.arguments.StitchInPlace || string.IsNullOrEmpty(arguments.DestinationFile))
                this.bundle = new FileBundle(this.arguments.SourceFiles);
            else
                this.bundle = new FileBundle(this.arguments.SourceFiles, this.arguments.DestinationFile);

            this.bundle.CurrentFileChanged += bundle_CurrentFileChanged;
        }

        private void ShowHelp()
        {
            this.screen.PrintMessage(this.arguments.HelpMessage);
        }

        private void bundle_CurrentFileChanged(object sender, CurrentFileChangedEventArgs e)
        {
            if (firstTime)
                firstTime = false;
            else
                this.screen.PrintMessage(string.Format("<{0}> Appended", currFile));

            this.screen.PrintMessage(string.Format("Appending: {0}", e.CurrentFileName));

            this.currFile = e.CurrentFileName;
        }

        private void manager_ProgressChanged(object sender, ProgressEventArgs e)
        {
            this.screen.ShowProgress(e.Progress);
        }

    }
}
