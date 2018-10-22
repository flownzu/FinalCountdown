namespace FinalCountdown
{
    partial class FinalCountdownForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxAccountInfo = new System.Windows.Forms.RichTextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkBoxUsePlaylist = new System.Windows.Forms.CheckBox();
            this.listBoxPlaylists = new System.Windows.Forms.ListBox();
            this.listBoxTracks = new System.Windows.Forms.ListBox();
            this.buttonToggle = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxAccountInfo);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Account Info";
            // 
            // richTextBoxAccountInfo
            // 
            this.richTextBoxAccountInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxAccountInfo.Location = new System.Drawing.Point(6, 19);
            this.richTextBoxAccountInfo.Name = "richTextBoxAccountInfo";
            this.richTextBoxAccountInfo.ReadOnly = true;
            this.richTextBoxAccountInfo.Size = new System.Drawing.Size(212, 43);
            this.richTextBoxAccountInfo.TabIndex = 0;
            this.richTextBoxAccountInfo.Text = "";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 492);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(560, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // checkBoxUsePlaylist
            // 
            this.checkBoxUsePlaylist.AutoSize = true;
            this.checkBoxUsePlaylist.Enabled = false;
            this.checkBoxUsePlaylist.Location = new System.Drawing.Point(242, 28);
            this.checkBoxUsePlaylist.Name = "checkBoxUsePlaylist";
            this.checkBoxUsePlaylist.Size = new System.Drawing.Size(196, 17);
            this.checkBoxUsePlaylist.TabIndex = 2;
            this.checkBoxUsePlaylist.Text = "Use Playlist to play Final Countdown";
            this.checkBoxUsePlaylist.UseVisualStyleBackColor = true;
            this.checkBoxUsePlaylist.CheckedChanged += new System.EventHandler(this.CheckBoxUsePlaylist_CheckedChanged);
            // 
            // listBoxPlaylists
            // 
            this.listBoxPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxPlaylists.FormattingEnabled = true;
            this.listBoxPlaylists.Location = new System.Drawing.Point(18, 86);
            this.listBoxPlaylists.Name = "listBoxPlaylists";
            this.listBoxPlaylists.Size = new System.Drawing.Size(212, 394);
            this.listBoxPlaylists.TabIndex = 3;
            this.listBoxPlaylists.SelectedIndexChanged += new System.EventHandler(this.ListBoxPlaylists_SelectedIndexChanged);
            this.listBoxPlaylists.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.ListBoxPlaylists_Format);
            // 
            // listBoxTracks
            // 
            this.listBoxTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxTracks.FormattingEnabled = true;
            this.listBoxTracks.Location = new System.Drawing.Point(236, 86);
            this.listBoxTracks.Name = "listBoxTracks";
            this.listBoxTracks.Size = new System.Drawing.Size(312, 394);
            this.listBoxTracks.TabIndex = 4;
            this.listBoxTracks.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.ListBoxTracks_Format);
            // 
            // buttonToggle
            // 
            this.buttonToggle.Enabled = false;
            this.buttonToggle.Location = new System.Drawing.Point(242, 51);
            this.buttonToggle.Name = "buttonToggle";
            this.buttonToggle.Size = new System.Drawing.Size(306, 23);
            this.buttonToggle.TabIndex = 5;
            this.buttonToggle.Text = "Start!";
            this.buttonToggle.UseVisualStyleBackColor = true;
            this.buttonToggle.Click += new System.EventHandler(this.ButtonToggle_Click);
            // 
            // FinalCountdownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 514);
            this.Controls.Add(this.buttonToggle);
            this.Controls.Add(this.listBoxTracks);
            this.Controls.Add(this.listBoxPlaylists);
            this.Controls.Add(this.checkBoxUsePlaylist);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.Name = "FinalCountdownForm";
            this.Text = "Final Countdown";
            this.groupBox1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxAccountInfo;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.CheckBox checkBoxUsePlaylist;
        private System.Windows.Forms.ListBox listBoxPlaylists;
        private System.Windows.Forms.ListBox listBoxTracks;
        private System.Windows.Forms.Button buttonToggle;
    }
}