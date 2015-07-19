﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Drs.Infrastructure.Extensions.Io
{
    public static class FileHelperExt
    {
        public static void ReplaceDataInFile(string alohaIniFile, string key, string newLine)
        {
            var lines = File.ReadLines(alohaIniFile).ToArray();
            var outLines = new List<string>(lines.Count());

            outLines.AddRange(lines.Select(line => line.Contains(key) ? newLine : line));

            File.WriteAllLines(alohaIniFile, outLines);
        }
    }
}
