﻿using CS_NVRController.BLL;
using CS_NVRController.Hickvision.NvrController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CS_NVRController.Hickvision.NvrController.NvrPlayback;

namespace CS_NVRControllerGUI.GUI {

	public partial class PlaybackControlWindow: Form {

		private PreviewWindow previewWindow_ = null;

		private PlaybackService playbackService_ = null;

		private delegate void changePlayedFrame(string txt);

		public PlaybackControlWindow()
		{
			InitializeComponent();
		}

		private void playbackControlWindow_Load(object sender, EventArgs e)
		{
			var now = DateTime.Now;
			playbackService_ = new PlaybackService {
				TimeInterval = (start: now.AddHours(-1), end: now)
			};

			dateTimePicker1.Value = playbackService_.TimeInterval.start;
			dateTimePicker2.Value = playbackService_.TimeInterval.end;

			playbackService_.OnStateChanged += onStateChanged;
			playbackService_.OnFramePlayed += onFramePlayed;

			fasterCb.Items.AddRange(Enum.GetNames(typeof(PlayerSpeed)));
			fasterCb.SelectedIndex = (int)playbackService_.PreviewSpeed;

			playedFramesLb.Text = string.Empty;
		}

		private void onStateChanged(object sender, PlayerState state)
		{
			switch (state) {
				case PlayerState.Stopped:
					playBtn.Text = "▶";
					playedFramesLb.Text = string.Empty;
					break;
				case PlayerState.Playing:
					playBtn.Text = "⏸";
					break;
				case PlayerState.Paused: // Shares the exact same action as SingleFrame
				case PlayerState.SingleFrame:
					playBtn.Text = "▶";
					break;
			}
			playerStatusLb.Text = state.ToString();
		}

		private void onFramePlayed(object sender, int frames)
		{
			// this callback may be called from another thread, so we should check
			// if InvokeRequired

			string playedFramed = $"Frame: {frames}";

			if (InvokeRequired) {
				Invoke(new changePlayedFrame((string s) => {
					playedFramesLb.Text = s;
					previewWindow_?.Invalidate();
				}), playedFramed);
			} else {
				playedFramesLb.Text = playedFramed;
				previewWindow_?.Invalidate();
			}
		}

		private void previewWindow_Paint(object sender, PaintEventArgs e)
		{
			Graphics pDc = e.Graphics;

			if (previewWindow_ == null || previewWindow_.IsDisposed 
				|| playbackService_ == null
				|| pDc == null) 
			{
				return;
			}
			
			try {
				using (Brush brush = new SolidBrush(Color.DarkRed)) {
					using (Pen pen = new Pen(brush)) {
						Rectangle rectTmp = new Rectangle(100, 100, 300, 20);

						string strText = $"Frame: {playbackService_.LastPlayedFrame}";
						Console.WriteLine(strText);
						using (Font font = new Font("Blackbody", 10, FontStyle.Italic | FontStyle.Bold)) {
							//Text
							pDc.DrawString(strText, font, brush, 6, 6);
							//Rectangle
							pDc.DrawRectangle(pen, rectTmp);
						}
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void playBtn_Click(object sender, EventArgs e)
		{
			try {
				switch (playbackService_.PreviwState) {
					case PlayerState.Stopped:
						previewWindow_ = new PreviewWindow();
						previewWindow_.FormClosing += stopBtn_Click;

						playbackService_.TimeInterval = (start: dateTimePicker1.Value, end: dateTimePicker2.Value);
						playbackService_.Play(previewWindow_.GetLiveViewHandle());
						previewWindow_.Show(this);
						break;
					case PlayerState.Playing:
						playbackService_.Pause();
						break;
					case PlayerState.Paused: // Shares the exact same action as SingleFrame
					case PlayerState.SingleFrame: 
						playbackService_.Resume();
						break;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void stopBtn_Click(object sender, EventArgs e)
		{
			try {
				playbackService_.Stop();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			previewWindow_.FormClosing -= stopBtn_Click;
			previewWindow_.Paint -= previewWindow_Paint;
			previewWindow_?.Dispose();
			previewWindow_ = null;
		}

		private void singleFrameBtn_Click(object sender, EventArgs e)
		{
			try {
				playbackService_.PauseAndNextFrame();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void fasterCb_SelectedIndexChanged(object sender, EventArgs e)
		{
			try {
				playbackService_.PreviewSpeed = (PlayerSpeed)fasterCb.SelectedIndex;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}