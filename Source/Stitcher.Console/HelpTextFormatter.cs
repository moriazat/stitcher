﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Stitcher.CommandLine
{
    class HelpTextFormatter
    {
        private string copyright;
        private string name;
        private string version;
        private string synopsis;
        private int middlePadSize;
        private List<ProgramSwitch> switches;
        private const string LEFT_PAD = "   ";
        private const int SCREEN_WIDTH = 80;
        private const char MIDDLE_PADDING_CHAR = '.';

        public HelpTextFormatter()
        {
            this.switches = new List<ProgramSwitch>();
        }

        public string CopyrightMessage
        {
            get { return this.copyright; }
            set { this.copyright = value; }
        }

        public string ProgramName
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string ProgramVersion
        {
            get { return this.version; }
            set { this.version = value; }
        }

        public string CommandSynopsis
        {
            get { return this.synopsis; }
            set { this.synopsis = value; }
        }

        public void AddSwitch(ProgramSwitch pSwitch)
        {
            this.switches.Add(pSwitch);
        }

        public string GetHelpMessage()
        {
            StringBuilder sb = new StringBuilder();

            AddProgramInfo(sb);
            AddCopyrightMessage(sb);
            AddSynopsis(sb);
            AddSwitches(sb);

            return sb.ToString();
        }

        private void AddSynopsis(StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(this.synopsis))
            {
                sb.AppendFormat("{0}{1}", LEFT_PAD, this.synopsis);
                sb.AppendLine();
                sb.AppendLine();
            }
        }

        private void AddCopyrightMessage(StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(this.copyright))
            {
                sb.AppendLine(this.copyright);
                sb.AppendLine();
            }
        }

        private void AddProgramInfo(StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(this.name) ||
                !string.IsNullOrEmpty(this.version))
            {
                sb.AppendFormat("{0}\t{1}", this.name, this.version);
                sb.AppendLine();
            }
        }

        private void AddSwitches(StringBuilder sb)
        {
            this.middlePadSize =  GetMaxSwitchLength() + 6;
            string s;

            foreach (ProgramSwitch ps in switches)
            {
                s = FormatSwitch(ps);
                sb.AppendLine(s);
            }
        }

        private int GetMaxSwitchLength()
        {
            int max = 0;

            foreach (ProgramSwitch ps in switches)
            {
                if (ps.PrimaryFormat.Length > max)
                    max = ps.PrimaryFormat.Length;

                if (ps.AlternateFormat.Length > max)
                    max = ps.AlternateFormat.Length;
            }

            return max;
        }

        private string FormatSwitch(ProgramSwitch ps)
        {
            int maxLinesCount;
            int maxAllowedRightColSize = SCREEN_WIDTH - this.middlePadSize;
            string[] descLines = GetDescriptionLines(ps.Description, maxAllowedRightColSize);

            if (descLines.Length > 1)
                maxLinesCount = descLines.Length;
            else if (!string.IsNullOrEmpty(ps.AlternateFormat))
                maxLinesCount = 2;
            else
                maxLinesCount = 1;

            string[] leftCol = CreateLeftColumn(maxLinesCount, ps);
            string[] rightCol = CreateRightColumn(maxLinesCount, descLines);

            string table = CreateTable(leftCol, rightCol);

            return table;
        }

        private string CreateLeftCell(string content)
        {
            string col = LEFT_PAD + content;
            col = col.PadRight(this.middlePadSize);
            return col;
        }

        private string[] GetDescriptionLines(string desc, int maxWidth)
        {
            List<string> lines = new List<string>();

            if (maxWidth >= desc.Length)
            {
                lines.Add(desc);
            }
            else
            {
                int firstCharInLine = 0;
                int lastCharInLine;
                int lineLength;
                int start = maxWidth - 1;

                while (firstCharInLine < desc.Length)
                {
                    lastCharInLine = desc.LastIndexOf(' ', start, maxWidth);
                    lineLength = lastCharInLine - firstCharInLine;

                    if ((lastCharInLine < desc.Length - 1) && lastCharInLine != -1)
                        lines.Add(desc.Substring(firstCharInLine, lineLength));
                    firstCharInLine = lastCharInLine + 1;

                    if (maxWidth > (desc.Length - firstCharInLine - 1))
                    {
                        lines.Add(desc.Substring(firstCharInLine, (desc.Length - firstCharInLine - 1)));
                        break;
                    }
                }
            }

            return lines.ToArray();
        }

        private string[] CreateLeftColumn(int cellsCount, ProgramSwitch ps)
        {
            List<string> column = new List<string>();
            string emptyPadding = string.Empty.PadLeft(this.middlePadSize);

            column.Add(CreateLeftCell(ps.PrimaryFormat));

            if (cellsCount> 1)
            {
                Debug.Assert(ps.AlternateFormat != null, "ProgramSwitch.AlternateFormat shouldn't be null.");
                column.Add(CreateLeftCell(ps.AlternateFormat));
                if (cellsCount > 2)
                    for (int i = 0; i < cellsCount - 2; i++)
                        column.Add(emptyPadding);
            }

            return column.ToArray();
        }

        private string[] CreateRightColumn(int cellsCount, string[] currCells)
        {
            List<string> column = new List<string>(currCells);

            if (cellsCount > currCells.Length)
            {
                for (int i = 0; i < cellsCount - column.Count; i++)
                    column.Add(string.Empty);
            }

            return column.ToArray();
        }

        private string CreateTable(string[] leftColumn, string[] rightColumn)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < leftColumn.Length; i++)
            {
                sb.Append(leftColumn[i]);
                sb.AppendLine(rightColumn[i]);
            }

            return sb.ToString();
        }
    }
}
