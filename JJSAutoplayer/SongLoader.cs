using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace JJSAutoplayer
{
    public sealed class SongEvent
    {
        public SongEvent(IReadOnlyList<byte> keys, int delayMs, bool isRest)
        {
            Keys = keys;
            DelayMs = delayMs;
            IsRest = isRest;
        }

        public IReadOnlyList<byte> Keys { get; }
        public int DelayMs { get; }
        public bool IsRest { get; }
    }

    public static class SongLoader
    {
        public static List<SongEvent> Load(string path)
        {
            var list = new List<SongEvent>();

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return list;

            foreach (var rawLine in File.ReadLines(path))
            {
                var line = StripComment(rawLine).Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                    continue;

                var delayMs = parts.Length > 1 ? ParseDelay(parts[1]) : 120;
                if (parts[0] is "-" or "REST" or "rest")
                {
                    list.Add(new SongEvent(Array.Empty<byte>(), delayMs, true));
                    continue;
                }

                var keys = ParseKeys(parts[0]).ToArray();
                if (keys.Length > 0)
                    list.Add(new SongEvent(keys, delayMs, false));
            }

            return list;
        }

        private static string StripComment(string line)
        {
            var cut = line.Length;
            foreach (var marker in new[] { "#", "//", ";" })
            {
                var index = line.IndexOf(marker, StringComparison.Ordinal);
                if (index >= 0)
                    cut = Math.Min(cut, index);
            }

            return line[..cut];
        }

        private static IEnumerable<byte> ParseKeys(string token)
        {
            var cleaned = token.Trim();
            if (cleaned.StartsWith("[", StringComparison.Ordinal) && cleaned.EndsWith("]", StringComparison.Ordinal))
                cleaned = cleaned[1..^1];

            foreach (var ch in cleaned.Where(char.IsLetterOrDigit))
            {
                if (TryGetVirtualKey(ch, out var key))
                    yield return key;
            }
        }

        private static bool TryGetVirtualKey(char ch, out byte key)
        {
            var upper = char.ToUpperInvariant(ch);
            if ((upper >= 'A' && upper <= 'Z') || (upper >= '0' && upper <= '9'))
            {
                key = (byte)upper;
                return true;
            }

            key = 0;
            return false;
        }

        private static int ParseDelay(string value)
        {
            value = value.Trim();
            var isMs = value.EndsWith("ms", StringComparison.OrdinalIgnoreCase);
            if (isMs)
                value = value[..^2];

            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                return 120;

            var delayMs = isMs || parsed > 10 ? parsed : parsed * 1000.0;
            return Math.Max(0, (int)Math.Round(delayMs));
        }
    }
}
