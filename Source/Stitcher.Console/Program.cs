using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Stitcher.Core;

namespace Stitcher.CommandLine
{
    public class Program
    {
        public static string Name
        {
            get { return "Binary File Stitcher"; }
        }

        public static void Main(string[] args)
        {
            ApplicationArguments arguments = new ApplicationArguments(args);
            ProgramScreen screen = new ProgramScreen(Console.CursorTop, Console.CursorLeft);
            ConsoleApplication app = new ConsoleApplication(arguments, screen);
            app.AppVersion = GetVersion();
            app.Run();
        }

        public static string GetVersion()
        {
            Assembly appAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(appAssembly.Location);
            return info.FileVersion;
        }    
    }

}
