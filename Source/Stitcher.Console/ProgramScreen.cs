using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stitcher.CommandLine
{
    class ProgramScreen
    {
        private int initialRow;
        private int initialCol;
        private bool colorReversed;

        public ProgramScreen(int currentRow, int currentColumn)
        {
            this.initialCol = currentColumn;
            this.initialRow = currentRow;
            this.colorReversed = false;
            Console.CursorVisible = false;
        }

        public void PrintMessage(string text)
        {           
            if (colorReversed)
            {
                ReverseColors();
                Console.CursorLeft = 0;
                Console.WriteLine(text);
                ReverseColors();
                return;
            }

            Console.CursorLeft = 0;
            Console.WriteLine(text);
        }

        public void PrintHeader(string appName, string appVersion)
        {
            string name = string.Format("{0}   {1}", appName, appVersion);
            Console.WriteLine(name);
            Console.WriteLine("".PadRight(name.Length, '-'));
        }

        public void ShowProgress(float progress)
        {
            if (!this.colorReversed)
            {
                ReverseColors();
                this.colorReversed = true;
            }

            Console.CursorLeft = 0;
            Console.Write("{0:F2}%", progress);
        }

        public void PrintFinalMessage(string message, bool waitForKeyPressed)
        {
            Console.CursorLeft = 0;
            if (colorReversed)
                ReverseColors();
            Console.WriteLine(message);

            if (waitForKeyPressed)
                Console.ReadKey();

            Console.CursorVisible = true;
        }

        private void ReverseColors()
        {
            ConsoleColor temp = Console.BackgroundColor;
            Console.BackgroundColor = Console.ForegroundColor;
            Console.ForegroundColor = temp;
        }
    }
}
