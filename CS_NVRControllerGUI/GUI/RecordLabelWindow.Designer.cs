namespace CS_NVRControllerGUI.GUI {
	partial class RecordLabelWindow {
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
			if (disposing && (components != null)) {
				if (bookmarkService_ != null) {
					bookmarkService_.OnBookmarkFetched -= onBookmarkFetched;
					bookmarkService_.Dispose();
				}

				labelTitleTb.TextChanged -= resetLoadingBookmarks;
				dateTimePicker1.ValueChanged -= resetLoadingBookmarks;
				dateTimePicker2.ValueChanged -= resetLoadingBookmarks;

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
			System.Windows.Forms.Panel panel1;
			System.Windows.Forms.TextBox textBox2;
			System.Windows.Forms.Panel panel3;
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelTitleTb = new System.Windows.Forms.TextBox();
			this.bookmarksLv = new System.Windows.Forms.ListView();
			this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.fetchNextBtn = new System.Windows.Forms.Button();
			this.addNewBookmarkBtn = new System.Windows.Forms.Button();
			this.titleTb = new System.Windows.Forms.TextBox();
			panel1 = new System.Windows.Forms.Panel();
			textBox2 = new System.Windows.Forms.TextBox();
			panel3 = new System.Windows.Forms.Panel();
			panel1.SuspendLayout();
			panel3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			panel1.Controls.Add(this.dateTimePicker2);
			panel1.Controls.Add(this.dateTimePicker1);
			panel1.Controls.Add(textBox2);
			panel1.Dock = System.Windows.Forms.DockStyle.Right;
			panel1.Location = new System.Drawing.Point(314, 20);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(194, 83);
			panel1.TabIndex = 2;
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.CalendarFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePicker2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dateTimePicker2.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePicker2.Location = new System.Drawing.Point(0, 42);
			this.dateTimePicker2.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(194, 24);
			this.dateTimePicker2.TabIndex = 2;
			this.dateTimePicker2.Value = new System.DateTime(2020, 1, 6, 0, 0, 0, 0);
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePicker1.Dock = System.Windows.Forms.DockStyle.Top;
			this.dateTimePicker1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePicker1.Location = new System.Drawing.Point(0, 0);
			this.dateTimePicker1.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(194, 24);
			this.dateTimePicker1.TabIndex = 0;
			this.dateTimePicker1.Value = new System.DateTime(2020, 1, 6, 0, 0, 0, 0);
			// 
			// textBox2
			// 
			textBox2.BackColor = System.Drawing.Color.AliceBlue;
			textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			textBox2.Cursor = System.Windows.Forms.Cursors.Arrow;
			textBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
			textBox2.Enabled = false;
			textBox2.Location = new System.Drawing.Point(0, 66);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(194, 17);
			textBox2.TabIndex = 6;
			// 
			// panel3
			// 
			panel3.Controls.Add(this.label3);
			panel3.Controls.Add(this.label2);
			panel3.Controls.Add(this.labelTitleTb);
			panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			panel3.Location = new System.Drawing.Point(0, 35);
			panel3.Name = "panel3";
			panel3.Size = new System.Drawing.Size(246, 48);
			panel3.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(0, 26);
			this.label3.Name = "label3";
			this.label3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 5);
			this.label3.Size = new System.Drawing.Size(49, 22);
			this.label3.TabIndex = 6;
			this.label3.Text = "Find:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 5);
			this.label2.Size = new System.Drawing.Size(37, 22);
			this.label2.TabIndex = 3;
			this.label2.Text = "To:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelTitleTb
			// 
			this.labelTitleTb.Location = new System.Drawing.Point(60, 21);
			this.labelTitleTb.Name = "labelTitleTb";
			this.labelTitleTb.Size = new System.Drawing.Size(183, 24);
			this.labelTitleTb.TabIndex = 5;
			// 
			// bookmarksLv
			// 
			this.bookmarksLv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.bookmarksLv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id,
            this.Time,
            this.Title});
			this.bookmarksLv.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bookmarksLv.GridLines = true;
			this.bookmarksLv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.bookmarksLv.HideSelection = false;
			this.bookmarksLv.LabelEdit = true;
			this.bookmarksLv.Location = new System.Drawing.Point(0, 109);
			this.bookmarksLv.Name = "bookmarksLv";
			this.bookmarksLv.Size = new System.Drawing.Size(559, 250);
			this.bookmarksLv.TabIndex = 0;
			this.bookmarksLv.UseCompatibleStateImageBehavior = false;
			this.bookmarksLv.View = System.Windows.Forms.View.Details;
			this.bookmarksLv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.bookmarksLv_KeyUp);
			this.bookmarksLv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bookmarksLv_MouseDown);
			// 
			// Id
			// 
			this.Id.Text = "Id";
			this.Id.Width = 92;
			// 
			// Time
			// 
			this.Time.Text = "Time";
			this.Time.Width = 134;
			// 
			// Title
			// 
			this.Title.Text = "Title";
			this.Title.Width = 292;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(panel1);
			this.groupBox1.Controls.Add(this.panel2);
			this.groupBox1.Controls.Add(this.fetchNextBtn);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(559, 106);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Find Bookmarks";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(panel3);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(3, 20);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(246, 83);
			this.panel2.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.label1.Size = new System.Drawing.Size(54, 22);
			this.label1.TabIndex = 1;
			this.label1.Text = "From:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// fetchNextBtn
			// 
			this.fetchNextBtn.BackColor = System.Drawing.Color.White;
			this.fetchNextBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.fetchNextBtn.Dock = System.Windows.Forms.DockStyle.Right;
			this.fetchNextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.fetchNextBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
			this.fetchNextBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.fetchNextBtn.Location = new System.Drawing.Point(508, 20);
			this.fetchNextBtn.Name = "fetchNextBtn";
			this.fetchNextBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.fetchNextBtn.Size = new System.Drawing.Size(48, 83);
			this.fetchNextBtn.TabIndex = 4;
			this.fetchNextBtn.Text = "⋗";
			this.fetchNextBtn.UseVisualStyleBackColor = false;
			this.fetchNextBtn.Click += new System.EventHandler(this.fetchNextBtn_Click);
			// 
			// addNewBookmarkBtn
			// 
			this.addNewBookmarkBtn.BackColor = System.Drawing.Color.White;
			this.addNewBookmarkBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.addNewBookmarkBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.addNewBookmarkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.addNewBookmarkBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
			this.addNewBookmarkBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.addNewBookmarkBtn.Location = new System.Drawing.Point(0, 359);
			this.addNewBookmarkBtn.Name = "addNewBookmarkBtn";
			this.addNewBookmarkBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.addNewBookmarkBtn.Size = new System.Drawing.Size(559, 40);
			this.addNewBookmarkBtn.TabIndex = 5;
			this.addNewBookmarkBtn.Text = "🖊";
			this.addNewBookmarkBtn.UseVisualStyleBackColor = false;
			this.addNewBookmarkBtn.Click += new System.EventHandler(this.addNewBookmarkBtn_Click);
			// 
			// titleTb
			// 
			this.titleTb.Location = new System.Drawing.Point(224, 134);
			this.titleTb.Name = "titleTb";
			this.titleTb.Size = new System.Drawing.Size(323, 22);
			this.titleTb.TabIndex = 6;
			this.titleTb.Visible = false;
			this.titleTb.KeyUp += new System.Windows.Forms.KeyEventHandler(this.titleTb_KeyUp);
			this.titleTb.Leave += new System.EventHandler(this.titleTb_Leave);
			// 
			// RecordLabelWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.AliceBlue;
			this.ClientSize = new System.Drawing.Size(559, 399);
			this.Controls.Add(this.titleTb);
			this.Controls.Add(this.bookmarksLv);
			this.Controls.Add(this.addNewBookmarkBtn);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(509, 446);
			this.Name = "RecordLabelWindow";
			this.ShowIcon = false;
			this.Text = "Bookmarks";
			this.Load += new System.EventHandler(this.recordLabelWindow_Load);
			this.Resize += new System.EventHandler(this.recordLabelWindow_Resize);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView bookmarksLv;
		private System.Windows.Forms.ColumnHeader Id;
		private System.Windows.Forms.ColumnHeader Time;
		private System.Windows.Forms.ColumnHeader Title;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button addNewBookmarkBtn;
		private System.Windows.Forms.Button fetchNextBtn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox labelTitleTb;
		private System.Windows.Forms.TextBox titleTb;
	}
}