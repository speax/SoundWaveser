namespace SoundWaveserWinFormTester
{
    partial class SoundWaveserWinFormTester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadAudioBTN = new System.Windows.Forms.Button();
            this.soundPnl = new System.Windows.Forms.Panel();
            this.loadHTTPAudioBTN = new System.Windows.Forms.Button();
            this.audioURLText = new System.Windows.Forms.TextBox();
            this.localFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxSave = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // loadAudioBTN
            // 
            this.loadAudioBTN.Location = new System.Drawing.Point(62, 12);
            this.loadAudioBTN.Name = "loadAudioBTN";
            this.loadAudioBTN.Size = new System.Drawing.Size(109, 41);
            this.loadAudioBTN.TabIndex = 0;
            this.loadAudioBTN.Text = "Load Local Audio";
            this.loadAudioBTN.UseVisualStyleBackColor = true;
            this.loadAudioBTN.Click += new System.EventHandler(this.loadSound_Click);
            // 
            // soundPnl
            // 
            this.soundPnl.BackColor = System.Drawing.Color.Transparent;
            this.soundPnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.soundPnl.Location = new System.Drawing.Point(62, 117);
            this.soundPnl.Name = "soundPnl";
            this.soundPnl.Size = new System.Drawing.Size(1029, 235);
            this.soundPnl.TabIndex = 1;
            this.soundPnl.Tag = "Wave";
            // 
            // loadHTTPAudioBTN
            // 
            this.loadHTTPAudioBTN.Location = new System.Drawing.Point(252, 12);
            this.loadHTTPAudioBTN.Name = "loadHTTPAudioBTN";
            this.loadHTTPAudioBTN.Size = new System.Drawing.Size(138, 41);
            this.loadHTTPAudioBTN.TabIndex = 3;
            this.loadHTTPAudioBTN.Text = "Load HTTP Audio";
            this.loadHTTPAudioBTN.UseVisualStyleBackColor = true;
            this.loadHTTPAudioBTN.Click += new System.EventHandler(this.loadHTTPAudioBTN_Click);
            // 
            // audioURLText
            // 
            this.audioURLText.Location = new System.Drawing.Point(400, 23);
            this.audioURLText.Name = "audioURLText";
            this.audioURLText.Size = new System.Drawing.Size(286, 20);
            this.audioURLText.TabIndex = 4;
            this.audioURLText.Text = "http://";
            // 
            // localFileDialog
            // 
            this.localFileDialog.FileName = "localFileDialog";
            this.localFileDialog.Filter = "WAV Files|*wav";
            // 
            // checkBoxSave
            // 
            this.checkBoxSave.AutoSize = true;
            this.checkBoxSave.Checked = true;
            this.checkBoxSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSave.Location = new System.Drawing.Point(62, 94);
            this.checkBoxSave.Name = "checkBoxSave";
            this.checkBoxSave.Size = new System.Drawing.Size(92, 17);
            this.checkBoxSave.TabIndex = 5;
            this.checkBoxSave.Text = "Save in Disk?";
            this.checkBoxSave.UseVisualStyleBackColor = true;
            // 
            // SoundWavesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 471);
            this.Controls.Add(this.checkBoxSave);
            this.Controls.Add(this.audioURLText);
            this.Controls.Add(this.loadHTTPAudioBTN);
            this.Controls.Add(this.soundPnl);
            this.Controls.Add(this.loadAudioBTN);
            this.Name = "SoundWavesForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadAudioBTN;
        private System.Windows.Forms.Panel soundPnl;
        private System.Windows.Forms.Button loadHTTPAudioBTN;
        private System.Windows.Forms.TextBox audioURLText;
        private System.Windows.Forms.OpenFileDialog localFileDialog;
        private System.Windows.Forms.CheckBox checkBoxSave;
    }
}

