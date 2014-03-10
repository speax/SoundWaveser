using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundWaveserWinFormTester
{
    public partial class SoundWaveserWinFormTester : Form
    {
        public int SoundPnlWidth { get { return soundPnl.Width; } }
        public int SoundPnlHeight { get { return soundPnl.Height; } }

        public SoundWaveserWinFormTester()
        {
            InitializeComponent();
        }

        private void loadSound_Click(object sender, EventArgs e)
        {
            localFileDialog.ShowDialog();
            if (string.IsNullOrEmpty(localFileDialog.FileName))
                throw new Exception("WAV File not found!");

            GenerateWavesImage(localFileDialog.FileName);
        }

        private void loadHTTPAudioBTN_Click(object sender, EventArgs e)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(audioURLText.Text);
                WebResponse webResp = webRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            GenerateWavesImage(audioURLText.Text);
        }

        private void GenerateWavesImage(string URI)
        {
            wndr.me.SoundWaveser.Waveser wa = new wndr.me.SoundWaveser.Waveser();
            var bm = wa.GenerateWavesBitmap(URI, soundPnl.Width, soundPnl.Height);
            SaveWavesImage(bm);
            AddToWin(bm);
        }


        private void SaveWavesImage(Bitmap bm)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string imageFilePath = currentDirectory + "\\waveser_" + DateTime.Now.Ticks + ".png";
            bm.Save(imageFilePath);

            if (checkBoxSave.Checked)
            {
                var result = MessageBox.Show("File Saved in: " + imageFilePath + "\n\n\n Open in Folder?", "File Saved!", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                    Process.Start(currentDirectory);
            }
        }

        private void AddToWin(Bitmap bm)
        {
            soundPnl.Controls.Clear();
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(soundPnl.Width, soundPnl.Height);
            pictureBox.Image = bm;

            soundPnl.Controls.Add(pictureBox);
        }
    }
}