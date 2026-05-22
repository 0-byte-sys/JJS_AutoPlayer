#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace JJSAutoplayer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Button btnStart = null!;
        private Button btnStop = null!;
        private Button btnReload = null!;
        private ComboBox cmbMode = null!;
        private Label lblMode = null!;
        private Label lblSongs = null!;
        private Label lblStartDelay = null!;
        private Label lblHold = null!;
        private Label lblChordSpread = null!;
        private Label lblInfo = null!;
        private Label lblStatus = null!;
        private ListBox lstSongs = null!;
        private NumericUpDown numStartDelay = null!;
        private NumericUpDown numHold = null!;
        private NumericUpDown numChordSpread = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lstSongs = new ListBox();
            btnStart = new Button();
            btnStop = new Button();
            btnReload = new Button();
            cmbMode = new ComboBox();
            lblMode = new Label();
            lblSongs = new Label();
            lblStartDelay = new Label();
            lblHold = new Label();
            lblChordSpread = new Label();
            lblInfo = new Label();
            lblStatus = new Label();
            numStartDelay = new NumericUpDown();
            numHold = new NumericUpDown();
            numChordSpread = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numStartDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numChordSpread).BeginInit();
            SuspendLayout();

            lblSongs.AutoSize = true;
            lblSongs.Location = new Point(12, 12);
            lblSongs.Text = "Songs";

            lstSongs.Location = new Point(12, 34);
            lstSongs.Size = new Size(280, 244);

            btnReload.Location = new Point(12, 288);
            btnReload.Size = new Size(88, 30);
            btnReload.Text = "Reload";
            btnReload.Click += btnReload_Click;

            btnStart.Location = new Point(108, 288);
            btnStart.Size = new Size(88, 30);
            btnStart.Text = "Start";
            btnStart.Click += btnStart_Click;

            btnStop.Enabled = false;
            btnStop.Location = new Point(204, 288);
            btnStop.Size = new Size(88, 30);
            btnStop.Text = "Stop";
            btnStop.Click += btnStop_Click;

            lblMode.AutoSize = true;
            lblMode.Location = new Point(316, 34);
            lblMode.Text = "JJS piano mode";

            cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMode.Items.AddRange(new object[] { "Simple", "Advanced" });
            cmbMode.Location = new Point(316, 56);
            cmbMode.Size = new Size(180, 23);
            cmbMode.SelectedIndexChanged += cmbMode_SelectedIndexChanged;

            lblStartDelay.AutoSize = true;
            lblStartDelay.Location = new Point(316, 96);
            lblStartDelay.Text = "Start delay (ms)";

            numStartDelay.Increment = 250;
            numStartDelay.Location = new Point(316, 118);
            numStartDelay.Maximum = 10000;
            numStartDelay.Minimum = 0;
            numStartDelay.Size = new Size(180, 23);
            numStartDelay.Value = 2000;

            lblHold.AutoSize = true;
            lblHold.Location = new Point(316, 158);
            lblHold.Text = "Key hold (ms)";

            numHold.Increment = 5;
            numHold.Location = new Point(316, 180);
            numHold.Maximum = 500;
            numHold.Minimum = 10;
            numHold.Size = new Size(180, 23);
            numHold.Value = 35;

            lblChordSpread.AutoSize = true;
            lblChordSpread.Location = new Point(316, 220);
            lblChordSpread.Text = "Chord spread (ms)";

            numChordSpread.Enabled = false;
            numChordSpread.Increment = 5;
            numChordSpread.Location = new Point(316, 242);
            numChordSpread.Maximum = 100;
            numChordSpread.Minimum = 0;
            numChordSpread.Size = new Size(180, 23);

            lblInfo.Location = new Point(12, 334);
            lblInfo.Size = new Size(484, 36);
            lblInfo.Text = "Focus the Roblox/JJS piano window after pressing Start. Simple Mode plays one key per hit; Advanced Mode supports chords.";

            lblStatus.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.Location = new Point(12, 382);
            lblStatus.Size = new Size(484, 36);
            lblStatus.Text = "Ready.";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(514, 434);
            Controls.Add(lblSongs);
            Controls.Add(lstSongs);
            Controls.Add(btnReload);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(lblMode);
            Controls.Add(cmbMode);
            Controls.Add(lblStartDelay);
            Controls.Add(numStartDelay);
            Controls.Add(lblHold);
            Controls.Add(numHold);
            Controls.Add(lblChordSpread);
            Controls.Add(numChordSpread);
            Controls.Add(lblInfo);
            Controls.Add(lblStatus);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Text = "JJS AutoPlayer";
            ((System.ComponentModel.ISupportInitialize)numStartDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHold).EndInit();
            ((System.ComponentModel.ISupportInitialize)numChordSpread).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
