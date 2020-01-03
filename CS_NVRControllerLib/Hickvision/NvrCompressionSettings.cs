using System.Linq;

namespace CS_NVRController.Hickvision {

	public partial class NvrCompressionSettings {

		#region RangeValues

		public static readonly int SteamSmoothMax = 100; // Maximum Smooth

		public static readonly int SteamSmoothMin = 1; // Clear

		#endregion RangeValues

		public NvrCompressionSettings() {
			AverageVideoBitrate = CompressionAverageVideoBitrate.AVB_1536Kb;}

		public CompressionStreamType StreamType { get; set; } = CompressionStreamType.Auto;

		public CompressionResolution Resolution { get; set; } = CompressionResolution.Auto;

		public CompressionBitrateType BitrateType { get; set; } = CompressionBitrateType.Variable;

		public CompressionPictureQuality PictureQuality { get; set; } = CompressionPictureQuality.Auto;

		public CompressionVideoBitrate VideoBitrate { get; set; } = CompressionVideoBitrate.Auto;

		public CompressionVideoFrameRate VideoFrameRate { get; set; } = CompressionVideoFrameRate.Auto;

		public CompressionIntervalFrameI IntervalFrameI { get; set; } = CompressionIntervalFrameI.Auto;

		public CompressionIntervalBPFrame IntervalBPFrame { get; set; } = CompressionIntervalBPFrame.BBP;

		public CompressionVideoEncoding VideoEncoding { get; set; } = CompressionVideoEncoding.Auto;

		public CompressionAudioEncoding AudioEncoding { get; set; } = CompressionAudioEncoding.Auto;

		public CompressionVideoEncodingComplexity VideoEncodingComplexity { get; set; } = CompressionVideoEncodingComplexity.Auto;

		/// <summary>
		/// SVC: Scalable Video Coding, can be encoded by level
		/// </summary>
		public bool isSvcEnable { get; set; } = false;

		public CompressionFormatType FormatType { get; set; } = CompressionFormatType.ExposedStream;

		public CompressionAudioBitrate AudioBitrate { get; set; } = CompressionAudioBitrate.Default;

		/// <summary>
		/// Can be [1 fr 100]. frclear, frsmooth
		/// By default SteamSmooth = SteamSmoothMin
		/// </summary>
		public int SteamSmooth
		{
			get => (SteamSmooth == 0)
				? (SteamSmooth = SteamSmoothMin) // if SteamSmooth == 0 => set SteamSmooth to NvrCompressionSettings.SteamSmoothMin
				: SteamSmooth; // By default set SteamSmooth to SteamSmoothMin
			set => SteamSmooth = Enumerable.Range(1, 100).Contains(value) ? value : throw new NvrExceptions.NvrBadLogicException($"SteamSmooth must be in range [fr100]. Current value is {value}");
		}

		public CompressionAudioSamplingRate AudioSamplingRate { get; set; } = CompressionAudioSamplingRate.Default;

		public bool isSmartCodecEnabled { get; set; } = true;

		/// <summary>
		/// Average video bitrate (valid when isSmartCodecEnabled = true)
		/// </summary>
		public CompressionAverageVideoBitrate AverageVideoBitrate {
			get => AverageVideoBitrate;
			set => AverageVideoBitrate = (isSmartCodecEnabled == true) ? value : throw new NvrExceptions.NvrBadLogicException($"AverageVideoBitrate valid when isSmartCodecEnabled = true. Currently isSmartCodecEnabled = {isSmartCodecEnabled}");
		}
	}
}