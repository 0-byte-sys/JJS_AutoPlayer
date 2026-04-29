using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace JJSAutoplayer
{
    public class Autoplayer
    {
        private Thread? thread;
        private bool running;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;

        public void Start(string songPath)
        {
            if (running)
                return;

            try
            {
                if (string.IsNullOrWhiteSpace(songPath))
                {
                    Console.WriteLine("Song path cannot be null or empty.");
                    return;
                }

                var notes = SongLoader.Load(songPath);
                if (notes == null || notes.Count == 0)
                {
                    Console.WriteLine("Failed to load song or song is empty.");
                    return;
                }

                running = true;
                thread = new Thread(() => Play(notes))
                {
                    IsBackground = true
                };
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting playback: {ex.Message}");
                running = false;
            }
        }

        public void Stop()
        {
            running = false;
            try
            {
                if (thread != null && thread.IsAlive)
                {
                    thread.Join(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping playback: {ex.Message}");
            }
        }

        private void Play(List<(string key, int delayMs)> notes)
        {
            Thread.Sleep(2000);

            foreach (var (key, delayMs) in notes)
            {
                if (!running)
                    break;

                try
                {
                    if (key.StartsWith("[") && key.EndsWith("]") && key.Length > 2)
                    {
                        string inner = key.Substring(1, key.Length - 2);
                        var chars = inner.ToCharArray();

                        foreach (var ch in chars)
                        {
                            if (!running)
                                break;

                            byte vk = (byte)char.ToUpperInvariant(ch);
                            keybd_event(vk, 0, 0, 0);
                        }

                        Thread.Sleep(40);

                        foreach (var ch in chars)
                        {
                            byte vk = (byte)char.ToUpperInvariant(ch);
                            keybd_event(vk, 0, KEYEVENTF_KEYUP, 0);
                        }
                    }
                    else if (key.Length == 1)
                    {
                        byte vk = (byte)char.ToUpperInvariant(key[0]);
                        keybd_event(vk, 0, 0, 0);
                        Thread.Sleep(30);
                        keybd_event(vk, 0, KEYEVENTF_KEYUP, 0);
                    }

                    if (delayMs > 0)
                        Thread.Sleep(delayMs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error playing note '{key}': {ex.Message}");
                }
            }

            running = false;
        }
    }
}