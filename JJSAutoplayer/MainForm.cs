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
            LoadSongs();
        }

        private void LoadSongs()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string songsDir = Path.Combine(baseDir, "songs");

            if (!Directory.Exists(songsDir))
                Directory.CreateDirectory(songsDir);

            lstSongs.Items.Clear();

            foreach (var file in Directory.GetFiles(songsDir, "*.txt"))
                lstSongs.Items.Add(Path.GetFileName(file));
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            if (lstSongs.SelectedItem == null)
            {
                MessageBox.Show("Select a song first.");
                return;
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string songsDir = Path.Combine(baseDir, "songs");
            string songFile = Path.Combine(songsDir, lstSongs.SelectedItem.ToString()!);

            if (!File.Exists(songFile))
            {
                MessageBox.Show("Song file not found.");
                return;
            }

            player.Start(songFile);
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            player.Stop();
        }
    }
}