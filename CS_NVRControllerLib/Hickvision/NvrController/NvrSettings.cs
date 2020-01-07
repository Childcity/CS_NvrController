using NVRCsharpDemo;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Runtime.InteropServices;

namespace CS_NVRController.Hickvision.NvrController {

	/// <summary>
	///		Provides a mechanism for convinient get and set NVR configurations.
	/// </summary>
	public class NvrSettings {

		#region Private

		private readonly bool isDebugEnabled_ = false;

		#endregion Private

		public NvrSettings(NvrUserSession userSession, bool isDebugEnabled)
		{
			NvrUserSession = userSession;
			isDebugEnabled_ = isDebugEnabled;
		}

		#region PublicProperties

		/// <summary>
		///		Return current user session
		/// </summary>
		public NvrUserSession NvrUserSession { get; private set; }

		#endregion

		#region PublicMethods

		public NvrCompressionSettings LoadStreamCompressionSettings()
		{
			var streamInf = getDvrCompressionCfgV30().struNormHighRecordPara;
			return new NvrCompressionSettings() {
				StreamType = (CompressionStreamType)streamInf.byStreamType,
				Resolution = (CompressionResolution)streamInf.byResolution,
				BitrateType = (CompressionBitrateType)streamInf.byBitrateType,
				PictureQuality = (CompressionPictureQuality)streamInf.byPicQuality,
				VideoBitrate = (CompressionVideoBitrate)streamInf.dwVideoBitrate,
				VideoFrameRate = (CompressionVideoFrameRate)streamInf.dwVideoFrameRate,
				IntervalFrameI = (CompressionIntervalFrameI)streamInf.wIntervalFrameI,
				IntervalBPFrame = (CompressionIntervalBPFrame)streamInf.byIntervalBPFrame,
				VideoEncoding = (CompressionVideoEncoding)streamInf.byVideoEncType,
				AudioEncoding = (CompressionAudioEncoding)streamInf.byAudioEncType,
				VideoEncodingComplexity = (CompressionVideoEncodingComplexity)streamInf.byVideoEncComplexity,
				IsSvcEnable = streamInf.byEnableSvc != 0,
				FormatType = (CompressionFormatType)streamInf.byFormatType,
				AudioBitrate = (CompressionAudioBitrate)streamInf.byAudioBitRate,
				StreamSmooth = streamInf.byStreamSmooth,
				AudioSamplingRate = (CompressionAudioSamplingRate)streamInf.byAudioSamplingRate,
				IsSmartCodecEnabled = streamInf.bySmartCodec != 0,
				IsDepthMapEnabled = streamInf.byDepthMapEnable != 0,
				AverageVideoBitrate = (CompressionAverageVideoBitrate)streamInf.wAverageVideoBitrate,
			};
		}

		public void UpdateStreamCompressionSettings(NvrCompressionSettings compressionSettings)
		{
			var compressionCfgInfoV30 = getDvrCompressionCfgV30();
			compressionCfgInfoV30.struNormHighRecordPara = new CHCNetSDK.NET_DVR_COMPRESSION_INFO_V30() {
				byStreamType = (byte)compressionSettings.StreamType,
				byResolution = (byte)compressionSettings.Resolution,
				byBitrateType = (byte)compressionSettings.BitrateType,
				byPicQuality = (byte)compressionSettings.PictureQuality,
				dwVideoBitrate = (uint)compressionSettings.VideoBitrate,
				dwVideoFrameRate = (uint)compressionSettings.VideoFrameRate,
				wIntervalFrameI = (ushort)compressionSettings.IntervalFrameI,
				byIntervalBPFrame = (byte)compressionSettings.IntervalBPFrame,
				byVideoEncType = (byte)compressionSettings.VideoEncoding,
				byAudioEncType = (byte)compressionSettings.AudioEncoding,
				byVideoEncComplexity = (byte)compressionSettings.VideoEncodingComplexity,
				byEnableSvc = (byte)(compressionSettings.IsSvcEnable ? 0x01 : 0x00),
				byFormatType = (byte)compressionSettings.FormatType,
				byAudioBitRate = (byte)compressionSettings.AudioBitrate,
				byStreamSmooth = (byte)compressionSettings.StreamSmooth,
				byAudioSamplingRate = (byte)compressionSettings.AudioSamplingRate,
				bySmartCodec = (byte)(compressionSettings.IsSmartCodecEnabled ? 0x01 : 0x00),
				byDepthMapEnable = (byte)(compressionSettings.IsDepthMapEnabled ? 0x01 : 0x00),
				wAverageVideoBitrate = (ushort)compressionSettings.AverageVideoBitrate
			};

			setDvrCompressionCfgV30(compressionCfgInfoV30);
		}

		#endregion PublicMethods

		#region PrivateMethods

		private CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30 getDvrCompressionCfgV30()
		{
			checkUserSessionValid();

			IntPtr ptrCompressionCfgInfoV30 = IntPtr.Zero;
			CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30 compressionCfgInfoV30 = new CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30();

			try {
				int compressionCfgInfoV30Size = Marshal.SizeOf(compressionCfgInfoV30);
				ptrCompressionCfgInfoV30 = Marshal.AllocHGlobal(compressionCfgInfoV30Size);
				Marshal.StructureToPtr(compressionCfgInfoV30, ptrCompressionCfgInfoV30, false);

				uint dwReturn = 0;
				if (!CHCNetSDK.NET_DVR_GetDVRConfig(NvrUserSession.UserSessionState.UserId,
													CHCNetSDK.NET_DVR_GET_COMPRESSCFG_V30,
													NvrUserSession.UserSessionState.SelectedChannelNum,
													ptrCompressionCfgInfoV30,
													(uint)compressionCfgInfoV30Size,
													ref dwReturn)) {
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_GetDVRConfig: NET_DVR_GET_COMPRESSCFG_V30 failed");
				}

				compressionCfgInfoV30 = (CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30)Marshal.PtrToStructure(ptrCompressionCfgInfoV30, typeof(CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30));
				debugInfo($"NET_DVR_GetDVRConfig: NET_DVR_GET_COMPRESSCFG_V30 succ! return={dwReturn}");
			} finally {
				if (ptrCompressionCfgInfoV30 != IntPtr.Zero) {
					Marshal.FreeHGlobal(ptrCompressionCfgInfoV30);
				}
			}

			return compressionCfgInfoV30;
		}

		private void setDvrCompressionCfgV30(CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30 compressionCfgInfoV30)
		{
			checkUserSessionValid();

			IntPtr ptrCompressionCfgInfoV30 = IntPtr.Zero;

			try {
				int compressionCfgInfoV30Size = Marshal.SizeOf(compressionCfgInfoV30);
				ptrCompressionCfgInfoV30 = Marshal.AllocHGlobal(compressionCfgInfoV30Size);
				Marshal.StructureToPtr(compressionCfgInfoV30, ptrCompressionCfgInfoV30, false);

				if (!CHCNetSDK.NET_DVR_SetDVRConfig(NvrUserSession.UserSessionState.UserId,
													CHCNetSDK.NET_DVR_SET_COMPRESSCFG_V30,
													NvrUserSession.UserSessionState.SelectedChannelNum,
													ptrCompressionCfgInfoV30,
													(uint)compressionCfgInfoV30Size)) {
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_SetDVRConfig: NET_DVR_SET_COMPRESSCFG_V30 failed");
				}

				debugInfo($"NET_DVR_SetDVRConfig: NET_DVR_SET_COMPRESSCFG_V30 succ!");
			} finally {
				if (ptrCompressionCfgInfoV30 != IntPtr.Zero) {
					Marshal.FreeHGlobal(ptrCompressionCfgInfoV30);
				}
			}
		}

		private void checkUserSessionValid()
		{
			if (!NvrUserSession.IsSessionValid()) {
				throw new NvrBadLogicException("NvrUserSession is not valid");
			}
		}

		private void debugInfo(string msg)
		{
			if (isDebugEnabled_) {
				Console.WriteLine("[Debug] NvrSettings: " + msg);
			}
		}

		#endregion PrivateMethods

		}
	}
