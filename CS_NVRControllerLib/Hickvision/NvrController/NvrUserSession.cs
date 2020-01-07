using CS_NVRController.Hickvision.NvrExceptions;
using NVRCsharpDemo;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CS_NVRController.Hickvision.NvrController {

	/// <summary>
	///		Provides a mechanism for convinient management NVR session.
	/// </summary>
	public partial class NvrUserSession: IDisposable {

		#region Private

		private readonly bool isDebugEnabled_ = false;

		private bool isSdkInit_ = false;

		private string sdkLogDir_;

		private UserSession userSession_;

		#endregion Private

		public NvrUserSession(NvrSessionInfo sessionInfo, string sdkLogDir, bool isDebugEnabled)
		{
			sdkLogDir_ = sdkLogDir;
			isDebugEnabled_ = isDebugEnabled;
			userSession_ = new UserSession(sessionInfo);
			debugInfo("NvrSdkLogsDir: '" + sdkLogDir_ + "'");
		}

		#region IDisposable Support

		private bool isDisposed_ = false;

		protected virtual void dispose(bool disposing)
		{
			if (!isDisposed_) {
				if (disposing) {
					debugInfo("~NvrUserSession()");

					try {
						if (isLoggedIn()) {
							StopSession();
						}
					} finally { }

					bool sdkCleanup = CHCNetSDK.NET_DVR_Cleanup();
					debugInfo("Is NET_DVR_Cleanup() was Ok = " + sdkCleanup);
					isSdkInit_ = false;
				}

				isDisposed_ = true;
			}
		}

		public void Dispose()
		{
			dispose(true);
		}

		#endregion IDisposable Support

		#region Enums

		public enum ChannelStatus { Unknown, Online, Offline, Idle }

		public enum ChannelTransmissionProtocol { Unknown = -1, Tcp, Udp, Rtsp, Auto = 255 }

		#endregion Enums

		#region PublicProperties

		/// <summary>
		///		Return true if [sdk init] and [session started] and [this object isn't disposed]
		/// </summary>
		/// <returns></returns>
		public bool IsSessionValid() => isSdkInit_ && isLoggedIn() && (!isDisposed_);

		public NvrSessionInfo SessionInfo => userSession_.SessionInfo;

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
			get => userSession_.SelectedChannelId;
			set => userSession_.SelectedChannelId = value;
		}

		#endregion

		#region PublicMethods

		/// <summary>
		/// Start session with NVR/DVR device, initialize sdk and logs.
		/// </summary>
		public void StartSession()
		{
			initSdk();
			loginUser();
		}

		/// <summary>
		/// Stop session with NVR/DVR device.
		/// </summary>
		public void StopSession() => logoutUser();

		#endregion

		#region InternalProperties

		internal UserSession UserSessionState => userSession_;

		#endregion

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
			if (!isLoggedIn()) {
				debugInfo("User not login!");
				return;
			}

			//Logout the device

			if (!CHCNetSDK.NET_DVR_Logout(userSession_.UserId)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_Logout failed");
			}

			debugInfo("NET_DVR_Logout succ!");

			userSession_ = new UserSession(userSession_.SessionInfo);
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
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_GetDVRConfig failed");
				}

				debugInfo("NET_DVR_GetDVRConfig: NET_DVR_GET_IPPARACFG_V40 succ!");

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

		private void debugInfo(string msg)
		{
			if (isDebugEnabled_) {
				Console.WriteLine("[Debug] NvrUserSession: " + msg);
			}
		}

		private bool isLoggedIn() => userSession_.UserId != -1;

		#endregion PrivateMethods

	}

	public partial class NvrUserSession {

		internal struct UserSession {

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

	}
}