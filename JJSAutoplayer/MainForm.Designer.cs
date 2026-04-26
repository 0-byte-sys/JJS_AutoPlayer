using System.Drawing;
using System.Windows.Forms;

namespace JJSAutoplayer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Button btnStart = null!;
        private Button btnStop = null!;
        private ListBox lstSongs = null!;
        private Label lblInfo = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lstSongs = new ListBox();
            this.btnStart = new Button();
            this.btnStop = new Button();
            this.lblInfo = new Label();

            this.SuspendLayout();

            this.lstSongs.Location = new Point(12, 12);
            this.lstSongs.Size = new Size(360, 260);

            this.btnStart.Text = "Start";
            this.btnStart.Location = new Point(12, 285);
            this.btnStart.Size = new Size(80, 30);
            this.btnStart.Click += new EventHandler(this.btnStart_Click);

            this.btnStop.Text = "Stop";
            this.btnStop.Location = new Point(110, 285);
            this.btnStop.Size = new Size(80, 30);
            this.btnStop.Click += new EventHandler(this.btnStop_Click);

            this.lblInfo.Text = "Click into Roblox/JJS Piano window before pressing Start.";
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new Point(12, 325);

            this.ClientSize = new Size(384, 361);
            this.Controls.Add(this.lstSongs);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "JJS Piano Autoplayer";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}