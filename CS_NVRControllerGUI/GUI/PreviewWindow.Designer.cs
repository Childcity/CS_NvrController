namespace CS_NVRControllerGUI.GUI {
	partial class PreviewWindow {
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
			this.streamWnd1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.streamWnd1)).BeginInit();
			this.SuspendLayout();
			// 
			// streamWnd1
			// 
			this.streamWnd1.BackColor = System.Drawing.Color.Gainsboro;
			this.streamWnd1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.streamWnd1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.streamWnd1.Location = new System.Drawing.Point(0, 0);
			this.streamWnd1.Name = "streamWnd1";
			this.streamWnd1.Size = new System.Drawing.Size(726, 397);
			this.streamWnd1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.streamWnd1.TabIndex = 1;
			this.streamWnd1.TabStop = false;
			// 
			// PreviewWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(726, 397);
			this.Controls.Add(this.streamWnd1);
			this.Name = "PreviewWindow";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowIcon = false;
			this.Text = "Camera Preview";
			((System.ComponentModel.ISupportInitialize)(this.streamWnd1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox streamWnd1;
	}
}