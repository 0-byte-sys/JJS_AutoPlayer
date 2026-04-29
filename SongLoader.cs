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

            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("Song path cannot be null or empty.");
                    return list;
                }

                if (!File.Exists(path))
                {
                    Console.WriteLine($"Song file not found: {path}");
                    return list;
                }

                foreach (var rawLine in File.ReadAllLines(path))
                {
                    try
                    {
                        var line = rawLine.Trim();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length < 1 || parts.Length > 2)
                            continue;

                        string key = parts[0];
                        if (string.IsNullOrWhiteSpace(key))
                            continue;

                        int delayMs = 0;
                        if (parts.Length == 2)
                        {
                            if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double seconds))
                                continue;

                            delayMs = (int)(seconds * 1000.0);
                            if (delayMs < 0)
                                delayMs = 0;
                        }

                        list.Add((key, delayMs));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line '{rawLine}': {ex.Message}");
                    }
                }

                if (list.Count == 0)
                {
                    Console.WriteLine("Warning: Song file loaded but contains no valid notes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading song file: {ex.Message}");
            }

            return list;
        }
    }
}