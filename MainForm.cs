using System;
using System.IO;
using System.Windows.Forms;

namespace JJSAutoplayer
{
    public partial class MainForm : Form
    {
        private readonly Autoplayer player;

        public MainForm()
        {
            try
            {
                InitializeComponent();
                player = new Autoplayer();
                LoadSongs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSongs()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string songsDir = Path.Combine(baseDir, "songs");

                if (!Directory.Exists(songsDir))
                    Directory.CreateDirectory(songsDir);

                lstSongs.Items.Clear();

                var files = Directory.GetFiles(songsDir, "*.txt");
                if (files.Length == 0)
                {
                    MessageBox.Show("No song files found in the 'songs' folder.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var file in files)
                    lstSongs.Items.Add(Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading songs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            try
            {
                if (lstSongs.SelectedItem == null)
                {
                    MessageBox.Show("Select a song first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string songsDir = Path.Combine(baseDir, "songs");
                string songFile = Path.Combine(songsDir, lstSongs.SelectedItem.ToString()!);

                if (!File.Exists(songFile))
                {
                    MessageBox.Show("Song file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Click into the Roblox/JJS Piano window now! Playback will start in 2 seconds.", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
                player.Start(songFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting playback: {ex.Message}", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            try
            {
                player.Stop();
                MessageBox.Show("Playback stopped.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping playback: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}