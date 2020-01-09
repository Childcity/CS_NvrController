using NVRCsharpDemo;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Runtime.InteropServices;

namespace CS_NVRController.Hickvision.NvrController {

	/// <summary>
	///		Provides a mechanism for convinient previewing saved stream in NVR memory
	/// </summary>
	public partial class NvrPlayback: IDisposable {

		#region Private

		private readonly bool isDebugEnabled_ = false;

		private int playHandle_ = -1;

		private PlayerSpeed previewSpeed_ = PlayerSpeed.X1;

		#endregion Private

		public NvrPlayback(NvrUserSession userSession, bool isDebugEnabled)
		{
			NvrUserSession = userSession;
			isDebugEnabled_ = isDebugEnabled;
		}

		#region IDisposable Support

		private bool isDisposed_ = false;

		protected virtual void dispose(bool disposing)
		{
			if (!isDisposed_) {
				if (disposing) {
					debugInfo("~NvrPlayback()");

					try {
						if (playHandle_ != -1) {
							StopPreview();
						}
					} finally { }
					
				}

				isDisposed_ = true;
			}
		}

		public void Dispose()
		{
			dispose(true);
		}

		#endregion IDisposable Support

		#region PublicEnums

		public enum PlayerState { Stopped, Playing, Paused, SingleFrame }

		public enum PlayerSpeed { X1_32, X1_16, X1_4, X1_2, X1, X2, X4, X16, X32 }

		#endregion PublicEnums

		#region PublicProperties

		/// <summary>
		///		Return current user session
		/// </summary>
		public NvrUserSession NvrUserSession { get; private set; }


		/// <summary>
		///		Return current user session
		/// </summary>
		public PlayerState PreviewState { get; private set; } = PlayerState.Stopped;

		public PlayerSpeed PreviewSpeed
		{
			get => previewSpeed_;
			set {
				if (PreviewState == PlayerState.Stopped 
					|| PreviewState == PlayerState.SingleFrame) {
					previewSpeed_ = value; // Value speed will be set after StartPreview called
					return;
				}

				// In loop we trying to set needed speed step by step
				debugInfo($"Cur speed       : {PreviewSpeed}");
				debugInfo($"New speed to set: {value}");
				int delta = value - previewSpeed_;
				if (delta > 0) {
					debugInfo("Speed will be Upped to: " + (PreviewSpeed + delta));
					for (int i = 0; i < delta; i++) {
						uint unused = 0;
						if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle_, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref unused)) {
							uint errCode = CHCNetSDK.NET_DVR_GetLastError();
							if (errCode == 502) {
								throw new NvrSetPlayerSpeedException(CHCNetSDK.NET_DVR_GetLastError(), $"Preview speed can't be set to: {value}");
							} else {
								throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYFAST failed");
							}
						}
						previewSpeed_++;
						debugInfo($"  --Cur speed       : {PreviewSpeed}");
					}
				} else if (delta < 0) {
					debugInfo("Speed will be Down to: " + (PreviewSpeed + delta));
					for (int i = 0; i < -delta; i++) {
						uint unused = 0;
						if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle_, CHCNetSDK.NET_DVR_PLAYSLOW, IntPtr.Zero, 0, IntPtr.Zero, ref unused)) {
							uint errCode = CHCNetSDK.NET_DVR_GetLastError();
							if (errCode == 502) {
								throw new NvrSetPlayerSpeedException(CHCNetSDK.NET_DVR_GetLastError(), $"Preview speed can't be set to: {value}");
							} else {
								throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYFAST failed");
							}
						}
						previewSpeed_--;
						debugInfo($"  --Cur speed       : {PreviewSpeed}");
					}
				}
			}
		}

		#endregion PublicProperties

		#region PublicEvents

		/// <summary>
		///		Occurs when PreviewState is changed
		/// </summary>
		/// <param name="PlayerState">
		///		Current preview state
		/// </param>
		public event EventHandler<PlayerState> OnPreviewStateChanged;

		/// <summary>
		///		Occurs when PreviewSpeed is changed
		/// </summary>
		/// <param name="PlayerSpeed">
		///		Current preview speed
		/// </param>
		public event EventHandler<PlayerSpeed> OnPreviewSpeedChanged;

		#endregion PublicEvents

		#region PublicMethods

		/// <summary>
		///		Start back preview
		/// </summary>
		public void StartPreview(IntPtr playWndHandle, DateTime startDate, DateTime endDate)
		{
			if (playHandle_ != -1) {
				throw new NvrBadLogicException("Call StartPreview before calling StartPreview");
			}

			if (!NvrUserSession.IsSessionValid()) {
				throw new NvrBadLogicException("Call StartPreview when NvrUserSession is invalid");
			}

			CHCNetSDK.NET_DVR_VOD_PARA struVodPara = new CHCNetSDK.NET_DVR_VOD_PARA();
			struVodPara.dwSize = (uint)Marshal.SizeOf(struVodPara);
			struVodPara.struIDInfo.dwChannel = (uint)NvrUserSession.UserSessionState.SelectedChannelNum;
			struVodPara.hWnd = playWndHandle;

			// Set the starting time to search video files
			struVodPara.struBeginTime.dwYear = (uint)startDate.Year;
			struVodPara.struBeginTime.dwMonth = (uint)startDate.Month;
			struVodPara.struBeginTime.dwDay = (uint)startDate.Day;
			struVodPara.struBeginTime.dwHour = (uint)startDate.Hour;
			struVodPara.struBeginTime.dwMinute = (uint)startDate.Minute;
			struVodPara.struBeginTime.dwSecond = (uint)startDate.Second;

			// Set the stopping time to search video files
			struVodPara.struEndTime.dwYear = (uint)endDate.Year;
			struVodPara.struEndTime.dwMonth = (uint)endDate.Month;
			struVodPara.struEndTime.dwDay = (uint)endDate.Day;
			struVodPara.struEndTime.dwHour = (uint)endDate.Hour;
			struVodPara.struEndTime.dwMinute = (uint)endDate.Minute;
			struVodPara.struEndTime.dwSecond = (uint)endDate.Second;

			// Playback by time
			playHandle_ = CHCNetSDK.NET_DVR_PlayBackByTime_V40(NvrUserSession.UserSessionState.UserId, ref struVodPara);

			if (playHandle_ == -1) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackByTime_V40 failed: " + playHandle_);
			}

			uint unused = 0;
			if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle_, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 10, IntPtr.Zero, ref unused)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYSTART failed");
			}

			changeState(PlayerState.Playing);
			setCorrectSpeed();

			debugInfo("NET_DVR_RealPlay_V40 succ!");
		}

		/// <summary>
		///		Resume back preview
		/// </summary>
		public void ResumePreview()
		{
			if (PreviewState == PlayerState.SingleFrame) {
				uint unused = 0;
				if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle_, CHCNetSDK.NET_DVR_PLAYNORMAL, IntPtr.Zero, 0, IntPtr.Zero, ref unused)) {
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYNORMAL failed");
				}

				changeState(PlayerState.Playing);
				setCorrectSpeed(); 
				debugInfo("NET_DVR_RealPlay_V40: NET_DVR_PLAYNORMAL succ!");

			} else {
				if (PreviewState != PlayerState.Paused) {
					throw new NvrBadLogicException("Call ResumePreview but preview isn't paused");
				}

				uint unused = 0;
				if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle_, CHCNetSDK.NET_DVR_PLAYRESTART, IntPtr.Zero, 0, IntPtr.Zero, ref unused)) {
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYRESTART failed");
				}

				changeState(PlayerState.Playing);
				debugInfo("NET_DVR_RealPlay_V40: NET_DVR_PLAYRESTART succ!");
			}
		}

		/// <summary>
		///		Pause back preview
		/// </summary>
		public void PausePreview()
		{
			if (PreviewState != PlayerState.Playing) {
				throw new NvrBadLogicException("Call PausePreview but preview isn't started");
			}

			uint unused = 0;
			if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle_, CHCNetSDK.NET_DVR_PLAYPAUSE, IntPtr.Zero, 0, IntPtr.Zero, ref unused)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYPAUSE failed");
			}

			changeState(PlayerState.Paused);
			debugInfo("NET_DVR_RealPlay_V40: NET_DVR_PLAYPAUSE succ!");
		}

		/// <summary>
		///		Stop back preview
		/// </summary>
		public void StopPreview()
		{
			if (playHandle_ == -1) {
				debugInfo("Preview not started!");
				return;
			}

			//Stop playback
			if (!CHCNetSDK.NET_DVR_StopPlayBack(playHandle_)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_StopPlayBack failed");
			}

			playHandle_ = -1;
			changeState(PlayerState.Stopped);
			debugInfo("NET_DVR_StopPlayBack succ!");
		}

		/// <summary>
		///		Set PreviewState to SingleFrame and play next frame.
		///		PreviewSpeed will be set to X1 automaticaly 
		/// </summary>
		public void PlayNextFrame()
		{
			if (PreviewState == PlayerState.Stopped) {
				throw new NvrBadLogicException("Call PlayNextFrame but preview isn't started");
			}

			uint unused = 0;
			if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle_, CHCNetSDK.NET_DVR_PLAYFRAME, IntPtr.Zero, 0, IntPtr.Zero, ref unused)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYPAUSE failed");
			}

			changeState(PlayerState.SingleFrame);
			debugInfo("NET_DVR_RealPlay_V40: NET_DVR_PLAYFRAME succ!");
		}

		#endregion PublicMethods

		#region PrivateMethods

		private void changeState(PlayerState newState)
		{
			PreviewState = newState;
			OnPreviewStateChanged?.Invoke(this, newState);
		}

		private void changeSpeed(PlayerSpeed newSpeed)
		{
			previewSpeed_ = newSpeed;
			OnPreviewSpeedChanged?.Invoke(this, newSpeed);
		}

		// In states PlayerState.Stopped and PlayerState.SingleFrame speed automaticaly set to X1, 
		// but our users think, that speed wasn't changed. 
		// So we should set speed to last value.
		private void setCorrectSpeed()
		{
			if (previewSpeed_ != PlayerSpeed.X1) {
				// This will set Nvr preview speed to correct
				PlayerSpeed newPreviewSpeed = previewSpeed_;
				previewSpeed_ = PlayerSpeed.X1;
				PreviewSpeed = newPreviewSpeed;
			}
		}

		private void debugInfo(string msg)
		{
			if (isDebugEnabled_) {
				Console.WriteLine("[Debug] NvrPlayback: " + msg);
			}
		}

		#endregion PrivateMethods

	}

}