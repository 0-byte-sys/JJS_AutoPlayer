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
            InitializeComponent();
            player = new Autoplayer();
            player.StatusChanged += SetStatus;
            player.PlaybackFinished += PlaybackFinished;
            cmbMode.SelectedIndex = 0;
            LoadSongs();
        }

        private void LoadSongs()
        {
            var songsDir = GetSongsDirectory();
            Directory.CreateDirectory(songsDir);

            lstSongs.Items.Clear();
            foreach (var file in Directory.GetFiles(songsDir, "*.txt"))
                lstSongs.Items.Add(Path.GetFileName(file));

            if (lstSongs.Items.Count > 0)
                lstSongs.SelectedIndex = 0;

            SetStatus(lstSongs.Items.Count == 0
                ? "No songs found. Add .txt files to the songs folder."
                : $"Loaded {lstSongs.Items.Count} song file(s).");
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            if (lstSongs.SelectedItem == null)
            {
                MessageBox.Show("Select a song first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var songFile = Path.Combine(GetSongsDirectory(), lstSongs.SelectedItem.ToString()!);
            if (!File.Exists(songFile))
            {
                MessageBox.Show("Song file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadSongs();
                return;
            }

            var options = new PlaybackOptions
            {
                Mode = cmbMode.SelectedIndex == 1 ? PlaybackMode.Advanced : PlaybackMode.Simple,
                StartDelayMs = (int)numStartDelay.Value,
                HoldMs = (int)numHold.Value,
                ChordSpreadMs = (int)numChordSpread.Value
            };

            if (player.Start(songFile, options))
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            player.Stop();
            SetStatus("Stopping playback.");
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnReload_Click(object? sender, EventArgs e)
        {
            LoadSongs();
        }

        private void cmbMode_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var advanced = cmbMode.SelectedIndex == 1;
            numChordSpread.Enabled = advanced;
            SetStatus(advanced
                ? "Advanced Mode: chords play together or with chord spread."
                : "Simple Mode: chords only play their first key.");
        }

        private static string GetSongsDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "songs");
        }

        private void SetStatus(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(SetStatus), message);
                return;
            }

            lblStatus.Text = message;
        }

        private void PlaybackFinished()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(PlaybackFinished));
                return;
            }

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            SetStatus("Playback finished.");
        }
    }
}
