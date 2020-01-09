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

			fasterCb.Items.AddRange(Enum.GetNames(typeof(PlayerSpeed)));
			fasterCb.SelectedIndex = (int)playbackService_.PreviewSpeed;
		}

		private void onStateChanged(object sender, PlayerState state)
		{
			switch (state) {
				case PlayerState.Stopped:
					playBtn.Text = "▶";
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