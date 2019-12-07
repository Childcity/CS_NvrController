﻿using CS_NVRController.Hickvision.NvrExceptions;
using NVRCsharpDemo;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace CS_NVRController.Hickvision {

	/// <summary>
	///		Provides a mechanism for convinient interection with Hickvision SDK.
	/// </summary>

	public partial class NvrController: IDisposable {

		#region Private

		private bool isDebugEnabled_ = false;

		private UserSession userSession_;

		private LivePlayer livePlayer_;

		private string sdkLogDir_;

		private bool isSdkInit_ = false;

		private CHCNetSDK.REALDATACALLBACK realDataCallBackFunc = null;

		#endregion Private

		public NvrController(NvrSessionInfo sessionInfo, string sdkLogDir, bool isDebugEnabled)
		{
			sdkLogDir_ = sdkLogDir;
			userSession_ = new UserSession(sessionInfo);
			livePlayer_ = new LivePlayer(IntPtr.Zero, new NvrPreviewSettings());
			debugInfo("NvrSdkLogsDir: '" + sdkLogDir_ + "'");
		}

		#region IDisposable Support

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue) {
				if (disposing) {
					debugInfo("~NvrController()");

					try {
						StopPreview();
					} finally { }

					try {
						StopSession();
					} finally { }

					bool sdkCleanup = CHCNetSDK.NET_DVR_Cleanup();
					debugInfo("Is NET_DVR_Cleanup() was Ok = " + sdkCleanup);
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion IDisposable Support

		#region Enums

		public enum ChannelStatus { Unknown, Online, Offline, Idle }

		public enum ChannelTransmissionProtocol { Unknown = -1, Tcp, Udp, Rtsp, Auto = 255 }

		#endregion Enums

		#region Properties

		public NvrSessionInfo SessionInfo
		{
			get { return userSession_.SessionInfo; }
		}

		/// <summary>
		///		Return List of cameras analog channels:
		///			int - id of channel
		///			ChannelStatus - channel status
		/// </summary>
		public List<Tuple<int, ChannelStatus>> AnalogChannels
		{
			get {
				var channels = new List<Tuple<int, ChannelStatus>>();

				for (int i = 0; i < userSession_.AnalogChanTotalNum; i++) {
					ChannelStatus status = (userSession_.IpParaCfgV40.byAnalogChanEnable[i] == 1) ? ChannelStatus.Online : ChannelStatus.Offline;
					channels.Add(Tuple.Create(i + 1, status));
				}

				return channels;
			}
		}

		/// <summary>
		///		Return List of cameras digital channels:
		///			int - id of channel
		///			ChannelStatus - channel status
		///			ChannelTransmissionProtocol - channel transmission protocol
		/// </summary>
		public List<Tuple<int, ChannelStatus, ChannelTransmissionProtocol>> IpChannels { get; private set; } = new List<Tuple<int, ChannelStatus, ChannelTransmissionProtocol>>();

		/// <summary>
		/// Set/Get Selected by user camera id
		/// </summary>
		public int SelectedChannelId
		{
			get { return userSession_.SelectedChannelId; }
			set { userSession_.SelectedChannelId = value; }
		}

		#endregion Properties

		#region PublicEvents

		/// <summary>
		///		
		/// </summary>
		/// <param name="Tuple.uint">
		///		NVR SDK error code
		/// </param>
		/// <param name="Tuple.string">
		///     NVR SDK error message
		/// </param>
		public event EventHandler<Tuple<uint, string>> OnPreviewError;

		#endregion PublicEvents

		#region PublicMethods

		public void StartSession()
		{
			initSdk();
			loginUser();
		}

		public void StopSession() => logoutUser();

		public void StartPreview(IntPtr playWndHandle, NvrPreviewSettings previewSettings)
		{
			if (livePlayer_.RealPlayHandle != -1) {
				throw new NvrBadLogicException("Call StopPreview before calling StartPreview");
			}

			livePlayer_ = new LivePlayer(playWndHandle, previewSettings);

			CHCNetSDK.NET_DVR_PREVIEWINFO previewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO() {
				hPlayWnd = IntPtr.Zero,
				lChannel = userSession_.SelectedChannelNum,
				dwStreamType = livePlayer_.PreviewSettings.StreamType,
				dwLinkMode = (uint)livePlayer_.PreviewSettings.LinkMode,
				bBlocked = livePlayer_.PreviewSettings.IsBlocked,
				dwDisplayBufNum = livePlayer_.PreviewSettings.DisplayBufNum,
				byPreviewMode = (byte)livePlayer_.PreviewSettings.PreviewMode
			};

			realDataCallBackFunc = new CHCNetSDK.REALDATACALLBACK(realDataCallBack);// Real-time stream callback function
			livePlayer_.RealPlayHandle = CHCNetSDK.NET_DVR_RealPlay_V40(userSession_.UserId, ref previewInfo, realDataCallBackFunc, IntPtr.Zero);

			if (livePlayer_.RealPlayHandle == -1) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_Init failed: " + livePlayer_.RealPlayHandle);
			}

			debugInfo("NET_DVR_RealPlay_V40 succ!");
		}

		public void StopPreview()
		{
			if (livePlayer_.RealPlayHandle == -1) {
				debugInfo("Preview not started!");
				return;
			}

			// Stop live view
			bool isStopRealPlayOk = CHCNetSDK.NET_DVR_StopRealPlay(livePlayer_.RealPlayHandle);

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

				livePlayer_.RealPlayPort = -1;
			}

			livePlayer_.RealPlayHandle = -1;

			if (isStopRealPlayOk) {
				debugInfo("NET_DVR_StopRealPlay succ!");
			} else {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_StopRealPlay failed");
			}
		}

		#endregion PublicMethods

		#region PrivateMethods

		private void initSdk()
		{
			isSdkInit_ = CHCNetSDK.NET_DVR_Init();

			if (!isSdkInit_) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_Init failed");
			}

			//To save the SDK log
			if (!CHCNetSDK.NET_DVR_SetLogToFile(3, sdkLogDir_, true)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_SetLogToFile error!");
			}

			if (!CHCNetSDK.NET_DVR_SetConnectTime(3000, 1)) { // Can be in range[300,75000]
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_SetConnectTime error!");
			}

			if (!CHCNetSDK.NET_DVR_SetReconnect(10000, 1)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_SetReconnect error!");
			}
		}

		private void loginUser()
		{
			userSession_.UserId = CHCNetSDK.NET_DVR_Login_V30(
				userSession_.SessionInfo.IPAddress,
				userSession_.SessionInfo.PortNumber,
				userSession_.SessionInfo.UserName,
				userSession_.SessionInfo.UserPassword,
				ref userSession_.DeviceInfo
			);

			if (userSession_.UserId < 0) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_Login_V30 failed");
			}

			debugInfo("NET_DVR_Login_V30 succ!");

			if (userSession_.DigitChanTotalNum > 0) {
				getIPChannelInfo();
				userSession_.SelectedChannelId = 0;
			} else {
				debugInfo("This device has no IP channel!");

				for (int i = 0; i < userSession_.AnalogChanTotalNum; i++) {
					userSession_.ChannelNum[i] = i + (int)userSession_.DeviceInfo.byStartChan;
				}
			}
		}

		private void logoutUser()
		{
			if (userSession_.UserId == -1) {
				debugInfo("User not login!");
				return;
			}

			//Logout the device

			try {
				StopPreview();
			} finally {
				if (!CHCNetSDK.NET_DVR_Logout(userSession_.UserId)) {
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_Logout failed");
				}

				debugInfo("NET_DVR_Logout succ!");

				userSession_ = new UserSession(userSession_.SessionInfo);
			}
		}

		private void getIPChannelInfo()
		{
			IntPtr ptrIpParaCfgV40 = IntPtr.Zero;

			try {
				int ipParaCfgV40Size = Marshal.SizeOf(userSession_.IpParaCfgV40);

				ptrIpParaCfgV40 = Marshal.AllocHGlobal(ipParaCfgV40Size);
				Marshal.StructureToPtr(userSession_.IpParaCfgV40, ptrIpParaCfgV40, false);

				uint dwReturn = 0;
				int iGroupNo = 0;

				if (!CHCNetSDK.NET_DVR_GetDVRConfig(userSession_.UserId, CHCNetSDK.NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, (uint)ipParaCfgV40Size, ref dwReturn)) {
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_GET_IPPARACFG_V40 failed");
				}

				debugInfo("NET_DVR_GET_IPPARACFG_V40 succ!");

				userSession_.IpParaCfgV40 = (CHCNetSDK.NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(CHCNetSDK.NET_DVR_IPPARACFG_V40));

				for (int i = 0; i < userSession_.AnalogChanTotalNum; i++) {
					userSession_.ChannelNum[i] = i + (int)userSession_.DeviceInfo.byStartChan;
				}

				byte byStreamType = 0;
				uint digitalChanNum = 64;

				if (userSession_.DigitChanTotalNum < 64) {
					digitalChanNum = userSession_.DigitChanTotalNum; //If the IP channel of the device is less than 64 channels, obtain the actual number of channels.
				}

				IpChannels.Clear();

				for (int i = 0; i < digitalChanNum; i++) {
					userSession_.ChannelNum[i + userSession_.AnalogChanTotalNum] = i + (int)userSession_.IpParaCfgV40.dwStartDChan;
					byStreamType = userSession_.IpParaCfgV40.struStreamMode[i].byGetStreamType;

					//Currently NVR supports only the mode: get stream from device directly
					int streamUnionSize = Marshal.SizeOf(userSession_.IpParaCfgV40.struStreamMode[i].uGetStream);
					if (byStreamType == 0) {
						CHCNetSDK.NET_DVR_IPCHANINFO chanInfo;
						IntPtr ptrChanInfo = IntPtr.Zero;
						try {
							ptrChanInfo = Marshal.AllocHGlobal(streamUnionSize);
							Marshal.StructureToPtr(userSession_.IpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfo, false);
							chanInfo = (CHCNetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure(ptrChanInfo, typeof(CHCNetSDK.NET_DVR_IPCHANINFO));

							//Add the IP channel to list
							IpChannels.Add(Tuple.Create(i + 1, getIPChannelStatus(chanInfo.byEnable, chanInfo.byIPID), ChannelTransmissionProtocol.Unknown));
							userSession_.IPDeviceID[i] = chanInfo.byIPID + chanInfo.byIPIDHigh * 256 - iGroupNo * 64 - 1;
						} finally {
							if (ptrChanInfo != IntPtr.Zero) {
								Marshal.FreeHGlobal(ptrChanInfo);
							}
						}
					} else if (byStreamType == 6) {
						CHCNetSDK.NET_DVR_IPCHANINFO_V40 chanInfoV40;
						IntPtr ptrChanInfoV40 = IntPtr.Zero;

						try {
							ptrChanInfoV40 = Marshal.AllocHGlobal(streamUnionSize);
							Marshal.StructureToPtr(userSession_.IpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfoV40, false);
							chanInfoV40 = (CHCNetSDK.NET_DVR_IPCHANINFO_V40)Marshal.PtrToStructure(ptrChanInfoV40, typeof(CHCNetSDK.NET_DVR_IPCHANINFO_V40));

							//Add the IP channel to list
							IpChannels.Add(Tuple.Create(i + 1, getIPChannelStatus(chanInfoV40.byEnable, chanInfoV40.wIPID), (ChannelTransmissionProtocol)chanInfoV40.byTransProtocol));
							userSession_.IPDeviceID[i] = chanInfoV40.wIPID - iGroupNo * 64 - 1;
						} finally {
							if (ptrChanInfoV40 != IntPtr.Zero) {
								Marshal.FreeHGlobal(ptrChanInfoV40);
							}
						}
					}
				}
			} finally {
				if (ptrIpParaCfgV40 != IntPtr.Zero) {
					Marshal.FreeHGlobal(ptrIpParaCfgV40);
				}
			}
		}

		private ChannelStatus getIPChannelStatus(byte byOnline, int byIPID)
		{
			if (byIPID == 0) {
				return ChannelStatus.Idle;
			} else {
				if (byOnline == 0) {
					return ChannelStatus.Offline;
				} else {
					return ChannelStatus.Online;
				}
			}
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
				Console.WriteLine("[Debug] NvrController: " + msg);
			}
		}

		#endregion PrivateMethods

	}

	public partial class NvrController {

		#region PrivateClasses

		private struct UserSession {

			public UserSession(NvrSessionInfo sessionInfo)
			{
				SessionInfo = sessionInfo;
				DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
				IpParaCfgV40 = new CHCNetSDK.NET_DVR_IPPARACFG_V40();
				UserId = SelectedChannelId = -1;
				IPDeviceID = new int[96];
				ChannelNum = new int[96];

				for (int i = 0; i < 96; i++) {
					IPDeviceID[i] = ChannelNum[i] = -1;
				}
			}

			public NvrSessionInfo SessionInfo { get; }

			public int UserId { get; set; }

			public int SelectedChannelId { get; set; }

			public int[] IPDeviceID { get; set; }

			public int[] ChannelNum { get; set; }

			public int SelectedChannelNum { get { return ChannelNum[SelectedChannelId]; } }

			public CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;

			public CHCNetSDK.NET_DVR_IPPARACFG_V40 IpParaCfgV40;

			//The number of analog channels. And the max number of IP channels equals to the value of dwDChanNum got by calling NET_DVR_GetDVRConfig(command: NET_DVR_GET_IPPARACFG_V40)
			public uint AnalogChanTotalNum
			{
				get { return (uint)DeviceInfo.byChanNum; }
			}

			//The maximum number of digital channels
			public uint DigitChanTotalNum
			{
				get { return (uint)DeviceInfo.byIPChanNum + 256 * (uint)DeviceInfo.byHighDChanNum; }
			}
		}

		private struct LivePlayer {

			public NvrPreviewSettings PreviewSettings { get; }

			public IntPtr PlayWndHandlePtr { get; }

			public int RealPlayHandle { get; set; }

			public int RealPlayPort;

			public LivePlayer(IntPtr playWndHandle, NvrPreviewSettings previewSettings)
			{
				PreviewSettings = previewSettings ?? throw new ArgumentNullException(nameof(previewSettings));
				PlayWndHandlePtr = playWndHandle;
				RealPlayHandle = RealPlayPort = -1;
			}
		}

		#endregion PrivateClasses

	}
}