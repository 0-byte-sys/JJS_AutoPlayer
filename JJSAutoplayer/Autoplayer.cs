using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace JJSAutoplayer
{
    public enum PlaybackMode
    {
        Simple,
        Advanced
    }

    public sealed class PlaybackOptions
    {
        public PlaybackMode Mode { get; init; } = PlaybackMode.Simple;
        public int StartDelayMs { get; init; } = 2000;
        public int HoldMs { get; init; } = 35;
        public int ChordSpreadMs { get; init; } = 0;
    }

    public class Autoplayer
    {
        private readonly object gate = new();
        private Thread? thread;
        private volatile bool running;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;

        public bool IsRunning => running;
        public event Action<string>? StatusChanged;
        public event Action? PlaybackFinished;

        public bool Start(string songPath, PlaybackOptions options)
        {
            lock (gate)
            {
                if (running)
                {
                    StatusChanged?.Invoke("Playback is already running.");
                    return false;
                }

                var notes = SongLoader.Load(songPath);
                if (notes.Count == 0)
                {
                    StatusChanged?.Invoke("Song is empty or has no valid notes.");
                    return false;
                }

                running = true;
                thread = new Thread(() => Play(notes, options))
                {
                    IsBackground = true,
                    Name = "JJS Autoplayer"
                };
                thread.Start();
                return true;
            }
        }

        public void Stop()
        {
            running = false;
        }

        private void Play(IReadOnlyList<SongEvent> notes, PlaybackOptions options)
        {
            try
            {
                StatusChanged?.Invoke($"Starting in {options.StartDelayMs / 1000.0:0.#} seconds. Focus Roblox now.");
                SleepInterruptibly(Math.Max(0, options.StartDelayMs));

                foreach (var note in notes)
                {
                    if (!running)
                        break;

                    if (!note.IsRest)
                        PlayNote(note.Keys, options);

                    if (note.DelayMs > 0)
                        SleepInterruptibly(note.DelayMs);
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke($"Playback error: {ex.Message}");
            }
            finally
            {
                running = false;
                PlaybackFinished?.Invoke();
            }
        }

        private void PlayNote(IReadOnlyList<byte> keys, PlaybackOptions options)
        {
            if (keys.Count == 0)
                return;

            var modeKeys = options.Mode == PlaybackMode.Simple ? new[] { keys[0] } : keys;
            var holdMs = Math.Clamp(options.HoldMs, 10, 500);
            var spreadMs = options.Mode == PlaybackMode.Advanced ? Math.Clamp(options.ChordSpreadMs, 0, 100) : 0;

            if (spreadMs > 0 && modeKeys.Count > 1)
            {
                foreach (var key in modeKeys)
                {
                    Tap(key, holdMs);
                    SleepInterruptibly(spreadMs);
                }
                return;
            }

            foreach (var key in modeKeys)
                keybd_event(key, 0, 0, 0);

            SleepInterruptibly(holdMs);

            foreach (var key in modeKeys)
                keybd_event(key, 0, KEYEVENTF_KEYUP, 0);
        }

        private void Tap(byte key, int holdMs)
        {
            keybd_event(key, 0, 0, 0);
            SleepInterruptibly(holdMs);
            keybd_event(key, 0, KEYEVENTF_KEYUP, 0);
        }

        private void SleepInterruptibly(int ms)
        {
            var remaining = ms;
            while (running && remaining > 0)
            {
                var slice = Math.Min(remaining, 25);
                Thread.Sleep(slice);
                remaining -= slice;
            }
        }
    }
}
