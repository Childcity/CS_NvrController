﻿using System.Linq;

namespace CS_NVRController.Hickvision {

	public partial class NvrCompressionSettings {

		#region RangeValues

		public static int SteamSmoothMax = 100; // Maximum Smooth

		public static int SteamSmoothMin = 1; // Clear

		#endregion

		public CompressionStreamType StreamType { get; set; } = CompressionStreamType.Auto;

		public CompressionResolution Resolution { get; set; } = CompressionResolution.Auto;

		public CompressionBitrateType BitrateType { get; set; }

		public CompressionPictureQuality PictureQuality { get; set; }

		public CompressionVideoBitrate VideoBitrate { get; set; }

		public CompressionVideoFrameRate VideoFrameRate { get; set; }

		public CompressionIntervalFrameI IntervalFrameI { get; set; }

		public CompressionIntervalBPFrame IntervalBPFrame { get; set; }

		public CompressionVideoEncoding VideoEncoding { get; set; }

		public CompressionAudioEncoding AudioEncoding { get; set; }

		public CompressionVideoEncodingComplexity VideoEncodingComplexity { get; set; }

		public bool isSvcEnable { get; set; }

		public CompressionFormatType FormatType { get; set; }

		public CompressionAudioBitrate AudioBitrate { get; set; }

		/// <summary>
		/// Can be [1 - 100]. 1-clear, 100-smooth 
		/// By default SteamSmooth = SteamSmoothMin
		/// </summary>
		public int SteamSmooth
		{
			get => SteamSmooth == 0 ? (SteamSmooth = SteamSmoothMin) : SteamSmooth; // By default set SteamSmooth to SteamSmoothMin
			set => SteamSmooth = Enumerable.Range(1, 100).Contains(value) ? value : throw new NvrExceptions.NvrBadLogicException($"SteamSmooth must be in range [1-100]. Current value is {value}");
		}

		public CompressionAudioSamplingRate AudioSamplingRate { get; set; }

		public bool isSmartCodecEnabled { get; set; }

		public CompressionAverageVideoBitrate AverageVideoBitrate { get; set; }
	}

	public partial class NvrCompressionSettings {

		public enum CompressionStreamType { Video, VideoAudio, Auto /* Same as source */ }

		public enum CompressionResolution {
			CIF_528x384_528x320 = 0, CIF_352x288or352x240 = 1, QCIF_176x144or176x120 = 2, CIF4_704x576or704x480_OR_D1_720x576or720x486 = 3,
			CIF2_704x288or704x240 = 4, QVGA_320x240 = 6, QQVGA_160x120 = 7, p384x288_ = 12, p576x576 = 13, VGA_640x480 = 16, UXGA_1600x1200 = 17, SVGA_800x600 = 18,
			HD720P_1280x720 = 19, XVGA_1280x960 = 20, HD900P_1600x900 = 21, p1360x1024 = 22, p1536x1536 = 23, p1920x1920 = 24, p1920x1080p = 27, p2560x1920 = 28,
			p1600x304 = 29, p2048x1536 = 30, p2448x2048 = 31, з2448x1200 = 32, p2448x800 = 33, XGA_1024x768 = 34, SXGA_1280x1024 = 35, WD1_960x576or960x480 = 36,
			p1920x1080_1080i = 37, WXGA_1440x900 = 38, HD_F_1920x1080or1280x720 = 39, HD_H_1920x540or1280x360 = 40, HD_Q_960x540or630x360 = 41, p2336x1744 = 42,
			p1920x1456 = 43, p2592x2048 = 44, p3296x2472 = 45, p1376x768 = 46, p1366x768 = 47, p1360x768 = 48, WSXGA_Plus = 49, p720x720 = 50, p1280x1280 = 51, p2048x768 = 52,
			pp2048x2048 = 53, p2560x2048 = 54, p3072x2048 = 55, p2304x1296 = 56, WXGA_1280x800 = 57, p1600x600 = 58, p1600x900 = 59, p2752x2208 = 60, p384x288 = 61, p4000x3000 = 62,
			p4096x2160 = 63, p3840x2160 = 64, p4000x2250 = 65, p3072x1728 = 66, p2592x1944 = 67, p2464x1520 = 68, p1280x1920 = 69, p2560x1440 = 70, p1024x1024 = 71, p160x128 = 72,
			p324x240 = 73, p324x256 = 74, p336x256 = 75, p640x512 = 76, p2720x2048 = 77, p384x256 = 78, p384x216 = 79, p320x256 = 80, p320x180 = 81, p320x192 = 82, p512x384 = 83,
			pp325x256 = 84, p256x192 = 85, p640x360 = 86, p1776x1340 = 87, p1936x1092 = 88, p2080x784 = 89, p2144x604 = 90, p1920x1200 = 91, p4064x3040 = 92, p3040x3040 = 93,
			p3072x2304 = 94, p3072x1152 = 95, p2560x2560 = 96, p2688x1536 = 97, p2688x1520 = 98, p3072x3072 = 99, p3392x2008 = 100, p4000x3080 = 101, p960x720 = 102,
			p1024x1536 = 103, p704x1056 = 104, p352x528 = 105, p2048x1530 = 106, p2560x1600 = 107, p2800x2100 = 108, p4088x4088 = 109, p4000x3072 = 110,
			p960x1080_1080p_lite = 111, p640x720_720p_half = 112, p640x960 = 113, p320x480 = 114, p3840x2400 = 115, p3840x1680 = 116, p2560x1120 = 117, p704x320 = 118,
			p1200x1920 = 119, p480x768 = 120, p768x480 = 121, p320x512 = 122, p512x320 = 123, p4096x1800 = 124, p1280x560 = 125, p2400x3840 = 126, p480x272 = 127, p512x272 = 128,
			p2592x2592 = 129, p1792x2880 = 130, p1600x2560 = 131, p2720x1192 = 132, p1920x1536or2048x1536 = 133, p2560x1944 = 134, p4096x1200 = 137, p3840x1080 = 138,
			p2720x800 = 139, p512x232 = 140, p704x200 = 141, p512x152 = 142, p2048x896 = 143, p2048x600 = 144, p1280x376 = 145, p8208x3072 = 150, p4096x1536 = 151, p6912x2800 = 152,
			p3456x1400 = 153, p480x720 = 154, p800x450 = 155, p480x270 = 156, p2560x1536 = 157, p3264x2448 = 160, p288x320 = 161, p144x176 = 162, p480x640 = 163, p240x320 = 164,
			p120x160 = 165, p576x720 = 166, p720x1280 = 167, p576x960 = 168, p2944x1656 = 169, p432x240 = 170, p2160x3840 = 171, p1080x1920 = 172, p7008x1080 = 173, p3504x540 = 174,
			p1752x270 = 175, p876x135 = 176, p4096x1440 = 177, p4096x1080 = 178, p1536x864 = 179, p180x240 = 180, p360x480 = 181, p540x720 = 182, p720x960 = 183, p960x1280 = 184,
			p1080x1440 = 185, p3200x1800 = 186, p1752x272 = 187, p872x136 = 188, p1280x1440 = 189,
			Auto = 0xff
		}

		public enum CompressionPictureQuality { }

		public enum CompressionBitrateType { }

		public enum CompressionVideoBitrate { }

		public enum CompressionVideoFrameRate { }

		public enum CompressionIntervalFrameI { }

		public enum CompressionIntervalBPFrame { }

		public enum CompressionVideoEncoding { }

		public enum CompressionAudioEncoding { }

		public enum CompressionVideoEncodingComplexity { }

		public enum CompressionEnableSvc { }

		public enum CompressionFormatType { }

		public enum CompressionAudioBitrate { }

		public enum CompressionSteamSmooth { }

		public enum CompressionAudioSamplingRate { }

		public enum CompressionSmartCodec { }

		public enum CompressionAverageVideoBitrate { }
	}
}