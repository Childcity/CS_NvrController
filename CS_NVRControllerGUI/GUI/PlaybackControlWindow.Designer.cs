namespace CS_NVRControllerGUI.GUI {
	partial class PlaybackControlWindow {
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
			if (disposing) {
				try {
					playbackService_.OnStateChanged -= onStateChanged;
					playbackService_?.Dispose();
					previewWindow_?.Dispose();
				} finally { }
				components?.Dispose();
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
			this.label2 = new System.Windows.Forms.Label();
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.singleFrameBtn = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.stopBtn = new System.Windows.Forms.Button();
			this.playBtn = new System.Windows.Forms.Button();
			this.fasterCb = new System.Windows.Forms.ComboBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.playerStatusLb = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.dateTimePicker2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.dateTimePicker1);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(262, 82);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select Time";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(3, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "To:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.CalendarFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePicker2.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePicker2.Location = new System.Drawing.Point(69, 49);
			this.dateTimePicker2.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(190, 24);
			this.dateTimePicker2.TabIndex = 2;
			this.dateTimePicker2.Value = new System.DateTime(2020, 1, 6, 0, 0, 0, 0);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(3, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "From:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePicker1.Dock = System.Windows.Forms.DockStyle.Right;
			this.dateTimePicker1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePicker1.Location = new System.Drawing.Point(69, 20);
			this.dateTimePicker1.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(190, 24);
			this.dateTimePicker1.TabIndex = 0;
			this.dateTimePicker1.Value = new System.DateTime(2020, 1, 6, 0, 0, 0, 0);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Controls.Add(this.singleFrameBtn);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.stopBtn);
			this.groupBox2.Controls.Add(this.playBtn);
			this.groupBox2.Controls.Add(this.fasterCb);
			this.groupBox2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.groupBox2.Location = new System.Drawing.Point(12, 197);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(274, 98);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Control";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.panel1.Location = new System.Drawing.Point(183, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(2, 95);
			this.panel1.TabIndex = 2;
			// 
			// singleFrameBtn
			// 
			this.singleFrameBtn.BackColor = System.Drawing.Color.White;
			this.singleFrameBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.singleFrameBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.singleFrameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
			this.singleFrameBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.singleFrameBtn.Location = new System.Drawing.Point(206, 32);
			this.singleFrameBtn.Name = "singleFrameBtn";
			this.singleFrameBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.singleFrameBtn.Size = new System.Drawing.Size(40, 40);
			this.singleFrameBtn.TabIndex = 3;
			this.singleFrameBtn.Text = "⋗";
			this.singleFrameBtn.UseVisualStyleBackColor = false;
			this.singleFrameBtn.Click += new System.EventHandler(this.singleFrameBtn_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(63, 75);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 18);
			this.label3.TabIndex = 2;
			this.label3.Text = "Speed";
			// 
			// stopBtn
			// 
			this.stopBtn.BackColor = System.Drawing.Color.White;
			this.stopBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.stopBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.stopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
			this.stopBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.stopBtn.Location = new System.Drawing.Point(137, 32);
			this.stopBtn.Name = "stopBtn";
			this.stopBtn.Size = new System.Drawing.Size(40, 40);
			this.stopBtn.TabIndex = 0;
			this.stopBtn.Text = "●";
			this.stopBtn.UseVisualStyleBackColor = false;
			this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
			// 
			// playBtn
			// 
			this.playBtn.BackColor = System.Drawing.Color.White;
			this.playBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.playBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.playBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
			this.playBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.playBtn.Location = new System.Drawing.Point(8, 32);
			this.playBtn.Name = "playBtn";
			this.playBtn.Size = new System.Drawing.Size(40, 40);
			this.playBtn.TabIndex = 0;
			this.playBtn.Text = "▶";
			this.playBtn.UseVisualStyleBackColor = false;
			this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
			// 
			// fasterCb
			// 
			this.fasterCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fasterCb.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.fasterCb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
			this.fasterCb.FormattingEnabled = true;
			this.fasterCb.ItemHeight = 18;
			this.fasterCb.Location = new System.Drawing.Point(54, 40);
			this.fasterCb.Name = "fasterCb";
			this.fasterCb.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.fasterCb.Size = new System.Drawing.Size(77, 26);
			this.fasterCb.TabIndex = 1;
			this.fasterCb.SelectedIndexChanged += new System.EventHandler(this.fasterCb_SelectedIndexChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playerStatusLb});
			this.statusStrip1.Location = new System.Drawing.Point(0, 297);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.statusStrip1.Size = new System.Drawing.Size(298, 25);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "Stopped";
			// 
			// playerStatusLb
			// 
			this.playerStatusLb.Name = "playerStatusLb";
			this.playerStatusLb.Size = new System.Drawing.Size(66, 20);
			this.playerStatusLb.Text = "Stopped";
			// 
			// PlaybackControlWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.AliceBlue;
			this.ClientSize = new System.Drawing.Size(298, 322);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PlaybackControlWindow";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Camera Playback";
			this.Load += new System.EventHandler(this.playbackControlWindow_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button playBtn;
		private System.Windows.Forms.ComboBox fasterCb;
		private System.Windows.Forms.Button singleFrameBtn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button stopBtn;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel playerStatusLb;
	}
}