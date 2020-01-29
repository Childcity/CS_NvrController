using NVRCsharpDemo;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Threading;

namespace CS_NVRController.Hickvision.NvrController {

	/// <summary>
	///		Provides a mechanism for convinient previewing NVR live stream.
	/// </summary>
	public partial class NvrLivePlayer: IDisposable {

		#region Private

		private readonly bool isDebugEnabled_ = false;

		private LivePlayer livePlayer_;

		#endregion Private

		public NvrLivePlayer(NvrUserSession userSession, bool isDebugEnabled)
		{
			NvrUserSession = userSession;
			isDebugEnabled_ = isDebugEnabled;
			livePlayer_ = LivePlayer.Default();
		}

		#region IDisposable Support

		private bool isDisposed_ = false;

		protected virtual void dispose(bool disposing)
		{
			if (!isDisposed_) {
				if (disposing) {
					debugInfo("~NvrLivePlayer()");

					try {
						if(livePlayer_.RealPlayHandle != -1) {
							StopPreview();
						}
					} finally { }

					DrawOnPictureHandle = null;
				}

				isDisposed_ = true;
			}
		}

		public void Dispose()
		{
			dispose(true);
		}

		#endregion IDisposable Support

		#region PublicProperties

		/// <summary>
		///		Return current user session.
		/// </summary>
		public NvrUserSession NvrUserSession { get; private set; }

		/// <summary>
		///		Set/Get Hadler (Action), that will be called at each frame.
		///		<para><paramref name="IntPtr"/> - device context handle. Can be used to draw something on device.</para>
		/// </summary>
		public Action<IntPtr> DrawOnPictureHandle { get; set; } = null;

		#endregion PublicProperties

		#region PublicEvents

		/// <summary>
		///		Occurs when happened error while viewing a stream.
		///		<para><paramref name="uint"/> - NVR SDK error code.</para>
		///		<para><paramref name="string"/> - NVR SDK error message.</para>
		/// </summary>
		public event EventHandler<Tuple<uint, string>> OnPreviewError;

		#endregion PublicEvents

		#region PublicMethods

		/// <summary>
		///		Start Preview in realtime.
		///		<para><paramref name="playWndHandle"/> - window handle, on fitch will be drawing.</para>
		///		<para><paramref name="previewSettings"/> - preview settings.</para>
		/// </summary>
		public void StartPreview(IntPtr playWndHandle, NvrPreviewSettings previewSettings)
		{
			if (livePlayer_.RealPlayHandle != -1) {
				throw new NvrBadLogicException("Call StopPreview before calling StartPreview");
			}

			if (!NvrUserSession.IsSessionValid()) {
				throw new NvrBadLogicException("Call StartPreview when NvrUserSession is invalid");
			}

			livePlayer_ = new LivePlayer(playWndHandle, previewSettings);

			CHCNetSDK.NET_DVR_PREVIEWINFO previewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO() {
				lChannel = NvrUserSession.UserSessionState.SelectedChannelNum,
				dwStreamType = livePlayer_.PreviewSettings.StreamType,
				dwLinkMode = (uint)livePlayer_.PreviewSettings.LinkMode,
				bBlocked = livePlayer_.PreviewSettings.IsBlocked,
				dwDisplayBufNum = livePlayer_.PreviewSettings.DisplayBufNum,
				byPreviewMode = (byte)livePlayer_.PreviewSettings.PreviewMode
			};

			switch (previewSettings.PreviewHandleMode) {
				case PreviewHandleType.Direct:
					livePlayer_.RealDataCallBackFunc = null;
					previewInfo.hPlayWnd = livePlayer_.PlayWndHandlePtr;
					break;
				case PreviewHandleType.CallBack:
					livePlayer_.RealDataCallBackFunc = new CHCNetSDK.REALDATACALLBACK(realDataCallBack);// Real-time stream callback function
					previewInfo.hPlayWnd = IntPtr.Zero;
					break;
			}

			livePlayer_.RealPlayHandle = CHCNetSDK.NET_DVR_RealPlay_V40(NvrUserSession.UserSessionState.UserId, ref previewInfo, livePlayer_.RealDataCallBackFunc, IntPtr.Zero);

			if (livePlayer_.RealPlayHandle == -1) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_RealPlay_V40 failed: " + livePlayer_.RealPlayHandle);
			}

			if (previewSettings.PreviewHandleMode == PreviewHandleType.Direct) {
				livePlayer_.DrawCallBackFunc = new CHCNetSDK.DRAWFUN(drawCallBack);
				if (!CHCNetSDK.NET_DVR_RigisterDrawFun(livePlayer_.RealPlayHandle, livePlayer_.DrawCallBackFunc, 0)) {
					invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "NET_DVR_RigisterDrawFun failed");
				}
			}

			debugInfo("NET_DVR_RealPlay_V40 succ!");
		}

		/// <summary>
		///		Stop Preview.
		/// </summary>
		public void StopPreview()
		{
			if (livePlayer_.RealPlayHandle == -1) {
				debugInfo("Preview not started!");
				return;
			}

			{
				// Set to null Draw CallBack Function
				CHCNetSDK.NET_DVR_RigisterDrawFun(livePlayer_.RealPlayHandle, null, 0);
			}

			bool isStopRealPlayOk;

			{
				// Stop live view
				isStopRealPlayOk = CHCNetSDK.NET_DVR_StopRealPlay(livePlayer_.RealPlayHandle);

				if (livePlayer_.RealPlayPort >= 0) {
					if (!PlayCtrl.PlayM4_Stop(livePlayer_.RealPlayPort)) {
						invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_Stop failed");
					}

					if (!PlayCtrl.PlayM4_CloseStream(livePlayer_.RealPlayPort)) {
						invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_CloseStream failed");
					}

					if (!PlayCtrl.PlayM4_FreePort(livePlayer_.RealPlayPort)) {
						invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_FreePort failed");
					}
				}
			}

			livePlayer_ = LivePlayer.Default();

			if (isStopRealPlayOk) {
				debugInfo("NET_DVR_StopRealPlay succ!");
			} else {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_StopRealPlay failed");
			}
		}

		#endregion PublicMethods

		#region PrivateMethods

		private void drawCallBack(int lRealHandle, IntPtr hDc, uint dwUser)
		{
			DrawOnPictureHandle?.Invoke(hDc);
		}

		private void realDataCallBack(int realHandle, uint dataType, IntPtr buffer, uint bufferSize, IntPtr pUser)
		{
			if (bufferSize <= 0) {
				return;
			}

			if (dataType == CHCNetSDK.NET_DVR_SYSHEAD) {
				if (livePlayer_.RealPlayPort >= 0) {
					return; // The same code stream does not need to call the open stream interface multiple times.
				}

				// Get the port to play
				if (!PlayCtrl.PlayM4_GetPort(ref livePlayer_.RealPlayPort)) {
					invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_GetPort failed");
					return;
				}

				// Set the stream mode: real-time stream mode
				if (!PlayCtrl.PlayM4_SetStreamOpenMode(livePlayer_.RealPlayPort, PlayCtrl.STREAME_REALTIME)) {
					invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_SetStreamOpenMode 'STREAME_REALTIME' failed");
					return;
				}

				// Open stream
				if (!PlayCtrl.PlayM4_OpenStream(livePlayer_.RealPlayPort, buffer, bufferSize, livePlayer_.PreviewSettings.PlayerBufSize)) {
					invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_OpenStream failed");
					return;
				}

				// Set the display buffer number
				if (!PlayCtrl.PlayM4_SetDisplayBuf(livePlayer_.RealPlayPort, livePlayer_.PreviewSettings.DisplayBufNum)) {
					invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_SetDisplayBuf failed");
					return;
				}

				// Set the display mode
				if (!PlayCtrl.PlayM4_SetOverlayMode(livePlayer_.RealPlayPort, 0, 0/* COLORREF(0)*/)) {
					invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_SetOverlayMode failed");
					return;
				}

				if (livePlayer_.PreviewSettings.PreviewQuality != PreviewQualityType.NotSet) {
					if (!PlayCtrl.PlayM4_SetPicQuality(livePlayer_.RealPlayPort, (int)livePlayer_.PreviewSettings.PreviewQuality)) {
						invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_SetPicQuality failed");
						return;
					}
				}

				// Set the decoding callback function to obtain the decoded audio and video raw data.
				//m_fDisplayFun = new PlayCtrl.DECCBFUN(DecCallbackFUN);
				//if (!PlayCtrl.PlayM4_SetDecCallBackEx(livePlayer_.RealPlayPort, m_fDisplayFun, IntPtr.Zero, 0)) {
				//	invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_SetDecCallBackEx failed");
				//}

				// Start to play
				if (!PlayCtrl.PlayM4_Play(livePlayer_.RealPlayPort, livePlayer_.PlayWndHandlePtr)) {
					invokeOnPreviewErrorEvent(PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort), "PlayM4_Play failed");
					return;
				}
			} else if (dataType == CHCNetSDK.NET_DVR_AUDIOSTREAMDATA) {
				debugInfo("NET_DVR_AUDIOSTREAMDATA");
			} else if (dataType == 112 /*NET_DVR_PRIVATE_DATA*/) {
				// NET_DVR_PRIVATE_DATA - Private data, including intelligent information
			} else {
				// dataType == CHCNetSDK.NET_DVR_STREAMDATA or else
				if (livePlayer_.RealPlayPort != -1) {
					for (int i = 0; i < 999; i++) {
						// Input the stream data to decode
						if (!PlayCtrl.PlayM4_InputData(livePlayer_.RealPlayPort, buffer, bufferSize)) {
							debugInfo("RealDataCallBack: PlayM4_InputData failed, error=" + PlayCtrl.PlayM4_GetLastError(livePlayer_.RealPlayPort));
							Thread.Sleep(50);
						} else {
							break;
						}
					}
				}
			}
		}

		private void invokeOnPreviewErrorEvent(uint sdkErrCode, string message)
		{
			OnPreviewError?.BeginInvoke(this, Tuple.Create(sdkErrCode, message), null, null);
		}

		private void debugInfo(string msg)
		{
			if (isDebugEnabled_) {
				Console.WriteLine("[Debug] NvrLivePlayer: " + msg);
			}
		}

		#endregion PrivateMethods

	}

	public partial class NvrLivePlayer {

		private struct LivePlayer {

			public NvrPreviewSettings PreviewSettings { get; }

			public IntPtr PlayWndHandlePtr { get; }

			public int RealPlayHandle { get; set; }

			public CHCNetSDK.REALDATACALLBACK RealDataCallBackFunc { get; set; }

			public CHCNetSDK.DRAWFUN DrawCallBackFunc { get; set; }

			public int RealPlayPort;

			public LivePlayer(IntPtr playWndHandle, NvrPreviewSettings previewSettings)
			{
				PreviewSettings = previewSettings ?? throw new ArgumentNullException(nameof(previewSettings));
				PlayWndHandlePtr = playWndHandle;
				RealPlayHandle = RealPlayPort = -1;
				RealDataCallBackFunc = null;
				DrawCallBackFunc = null;
			}

			public static LivePlayer Default() => new LivePlayer(IntPtr.Zero, new NvrPreviewSettings());
		}

	}
}