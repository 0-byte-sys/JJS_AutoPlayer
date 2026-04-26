using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace JJSAutoplayer
{
    public static class SongLoader
    {
        public static List<(string key, int delayMs)> Load(string path)
        {
            var list = new List<(string, int)>();

            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) continue;

                string key = parts[0];
                if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double seconds))
                    continue;

                int delayMs = (int)(seconds * 1000.0);
                if (delayMs < 0) delayMs = 0;

                list.Add((key, delayMs));
            }

            return list;
        }
    }
}