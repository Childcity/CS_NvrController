﻿namespace CS_NVRController {
	partial class MainWindow {
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
				liveViewService_.OnException -= appendLogOnUiThread;
				liveViewService_?.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent() {
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			this.streamWnd1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.logTxtBox = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.userInfoGB = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.previewSettingsGB = new System.Windows.Forms.GroupBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.button2 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.comboBox4 = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.previewPanel = new System.Windows.Forms.Panel();
			label4 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.streamWnd1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.userInfoGB.SuspendLayout();
			this.previewSettingsGB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.previewPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label4.Location = new System.Drawing.Point(6, 21);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(80, 17);
			label4.TabIndex = 7;
			label4.Text = "IpAddress";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label3.Location = new System.Drawing.Point(6, 49);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(38, 17);
			label3.TabIndex = 5;
			label3.Text = "Port";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label2.Location = new System.Drawing.Point(6, 104);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(53, 17);
			label2.TabIndex = 4;
			label2.Text = "Passw";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			label1.Location = new System.Drawing.Point(6, 77);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(83, 17);
			label1.TabIndex = 3;
			label1.Text = "UserName";
			// 
			// streamWnd1
			// 
			this.streamWnd1.BackColor = System.Drawing.Color.Gainsboro;
			this.streamWnd1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.streamWnd1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.streamWnd1.Location = new System.Drawing.Point(303, 3);
			this.streamWnd1.Name = "streamWnd1";
			this.streamWnd1.Size = new System.Drawing.Size(556, 507);
			this.streamWnd1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.streamWnd1.TabIndex = 0;
			this.streamWnd1.TabStop = false;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.logTxtBox, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.streamWnd1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(862, 734);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// logTxtBox
			// 
			this.logTxtBox.AcceptsReturn = true;
			this.logTxtBox.BackColor = System.Drawing.Color.Gainsboro;
			this.logTxtBox.CausesValidation = false;
			this.tableLayoutPanel1.SetColumnSpan(this.logTxtBox, 2);
			this.logTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logTxtBox.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)), true);
			this.logTxtBox.Location = new System.Drawing.Point(3, 516);
			this.logTxtBox.Multiline = true;
			this.logTxtBox.Name = "logTxtBox";
			this.logTxtBox.ReadOnly = true;
			this.logTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logTxtBox.Size = new System.Drawing.Size(856, 215);
			this.logTxtBox.TabIndex = 2;
			this.logTxtBox.TabStop = false;
			this.logTxtBox.Text = "Log...\r\n";
			this.logTxtBox.WordWrap = false;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel2.Controls.Add(this.userInfoGB);
			this.flowLayoutPanel2.Controls.Add(this.previewPanel);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(294, 507);
			this.flowLayoutPanel2.TabIndex = 3;
			// 
			// userInfoGB
			// 
			this.userInfoGB.Controls.Add(this.button1);
			this.userInfoGB.Controls.Add(label4);
			this.userInfoGB.Controls.Add(this.textBox4);
			this.userInfoGB.Controls.Add(label3);
			this.userInfoGB.Controls.Add(label2);
			this.userInfoGB.Controls.Add(label1);
			this.userInfoGB.Controls.Add(this.textBox3);
			this.userInfoGB.Controls.Add(this.textBox2);
			this.userInfoGB.Controls.Add(this.textBox1);
			this.userInfoGB.Dock = System.Windows.Forms.DockStyle.Top;
			this.userInfoGB.Location = new System.Drawing.Point(3, 3);
			this.userInfoGB.Name = "userInfoGB";
			this.userInfoGB.Size = new System.Drawing.Size(291, 141);
			this.userInfoGB.TabIndex = 0;
			this.userInfoGB.TabStop = false;
			this.userInfoGB.Text = "User Info";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Gainsboro;
			this.button1.Location = new System.Drawing.Point(214, 21);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(61, 105);
			this.button1.TabIndex = 8;
			this.button1.Text = "Login";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(95, 21);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(113, 22);
			this.textBox4.TabIndex = 6;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(95, 49);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(113, 22);
			this.textBox3.TabIndex = 2;
			this.textBox3.Text = "9000";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(95, 104);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(113, 22);
			this.textBox2.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(95, 77);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(113, 22);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "user";
			// 
			// previewSettingsGB
			// 
			this.previewSettingsGB.Controls.Add(this.checkBox2);
			this.previewSettingsGB.Controls.Add(this.numericUpDown2);
			this.previewSettingsGB.Controls.Add(this.checkBox1);
			this.previewSettingsGB.Controls.Add(this.label11);
			this.previewSettingsGB.Controls.Add(this.comboBox4);
			this.previewSettingsGB.Controls.Add(this.label10);
			this.previewSettingsGB.Controls.Add(this.label9);
			this.previewSettingsGB.Controls.Add(this.label8);
			this.previewSettingsGB.Controls.Add(this.textBox5);
			this.previewSettingsGB.Controls.Add(this.label7);
			this.previewSettingsGB.Controls.Add(this.comboBox3);
			this.previewSettingsGB.Controls.Add(this.label6);
			this.previewSettingsGB.Controls.Add(this.comboBox2);
			this.previewSettingsGB.Controls.Add(this.numericUpDown1);
			this.previewSettingsGB.Controls.Add(this.comboBox1);
			this.previewSettingsGB.Controls.Add(this.label5);
			this.previewSettingsGB.Dock = System.Windows.Forms.DockStyle.Top;
			this.previewSettingsGB.Location = new System.Drawing.Point(0, 0);
			this.previewSettingsGB.Name = "previewSettingsGB";
			this.previewSettingsGB.Size = new System.Drawing.Size(291, 294);
			this.previewSettingsGB.TabIndex = 1;
			this.previewSettingsGB.TabStop = false;
			this.previewSettingsGB.Text = "Preview Settings";
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(3, 264);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(90, 21);
			this.checkBox2.TabIndex = 17;
			this.checkBox2.Text = "IsBlocked";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.numericUpDown2.Location = new System.Drawing.Point(242, 137);
			this.numericUpDown2.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(41, 18);
			this.numericUpDown2.TabIndex = 16;
			this.numericUpDown2.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Gainsboro;
			this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.button2.Location = new System.Drawing.Point(0, 294);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(291, 32);
			this.button2.TabIndex = 15;
			this.button2.Text = "Start Live View";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(3, 237);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(147, 21);
			this.checkBox1.TabIndex = 14;
			this.checkBox1.Text = "IsPassbackRecord";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(3, 136);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 17);
			this.label11.TabIndex = 12;
			this.label11.Text = "DisplayBufNum";
			// 
			// comboBox4
			// 
			this.comboBox4.DisplayMember = "1";
			this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboBox4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.comboBox4.Location = new System.Drawing.Point(145, 201);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new System.Drawing.Size(138, 24);
			this.comboBox4.TabIndex = 11;
			this.comboBox4.Tag = "";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(4, 204);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(92, 17);
			this.label10.TabIndex = 10;
			this.label10.Text = "PreviewMode";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(-127, 234);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(74, 17);
			this.label9.TabIndex = 9;
			this.label9.Text = "ImgQuality";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(4, 174);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(74, 17);
			this.label8.TabIndex = 9;
			this.label8.Text = "ImgQuality";
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(145, 100);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(138, 22);
			this.textBox5.TabIndex = 8;
			this.textBox5.Text = "2*1024*1024";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(3, 105);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(96, 17);
			this.label7.TabIndex = 7;
			this.label7.Text = "PlayerBufSize";
			// 
			// comboBox3
			// 
			this.comboBox3.DisplayMember = "1";
			this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboBox3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.comboBox3.Location = new System.Drawing.Point(145, 171);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(138, 24);
			this.comboBox3.TabIndex = 5;
			this.comboBox3.Tag = "";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(142, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 17);
			this.label6.TabIndex = 6;
			this.label6.Text = "LinkMode";
			// 
			// comboBox2
			// 
			this.comboBox2.DisplayMember = "1";
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.comboBox2.Location = new System.Drawing.Point(229, 72);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(54, 24);
			this.comboBox2.TabIndex = 5;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.numericUpDown1.Location = new System.Drawing.Point(95, 70);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(41, 18);
			this.numericUpDown1.TabIndex = 4;
			// 
			// comboBox1
			// 
			this.comboBox1.DisplayMember = "1";
			this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.comboBox1.Location = new System.Drawing.Point(3, 21);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(167, 24);
			this.comboBox1.TabIndex = 3;
			this.comboBox1.Text = "Select Channel";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(4, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(85, 17);
			this.label5.TabIndex = 2;
			this.label5.Text = "StreamType";
			// 
			// previewPanel
			// 
			this.previewPanel.Controls.Add(this.button2);
			this.previewPanel.Controls.Add(this.previewSettingsGB);
			this.previewPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.previewPanel.Location = new System.Drawing.Point(3, 150);
			this.previewPanel.Name = "previewPanel";
			this.previewPanel.Size = new System.Drawing.Size(291, 326);
			this.previewPanel.TabIndex = 16;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(862, 734);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "MainWindow";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CS NVRController";
			((System.ComponentModel.ISupportInitialize)(this.streamWnd1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.userInfoGB.ResumeLayout(false);
			this.userInfoGB.PerformLayout();
			this.previewSettingsGB.ResumeLayout(false);
			this.previewSettingsGB.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.previewPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox streamWnd1;
		private System.Windows.Forms.TextBox logTxtBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.GroupBox userInfoGB;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox previewSettingsGB;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.ComboBox comboBox4;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.Panel previewPanel;
	}
}
