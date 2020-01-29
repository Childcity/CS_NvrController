namespace CS_NVRController.Hickvision {

	public class NvrCompressionSettings {

		#region RangeValues

		public static readonly int StreamSmoothUnused = 0; // Clear

		public static readonly int StreamSmoothMin = 1; // Clear

		public static readonly int StreamSmoothMax = 100; // Maximum Smooth

		#endregion RangeValues

		public NvrCompressionSettings() => AverageVideoBitrate = CompressionAverageVideoBitrate.AVB_1536Kb;

		public CompressionStreamType StreamType { get; set; } = CompressionStreamType.Auto;

		public CompressionResolution Resolution { get; set; } = CompressionResolution.Auto;

		public CompressionBitrateType BitrateType { get; set; } = CompressionBitrateType.VBR;

		public CompressionPictureQuality PictureQuality { get; set; } = CompressionPictureQuality.Auto;

		public CompressionVideoBitrate VideoBitrate { get; set; } = CompressionVideoBitrate.Auto;

		public CompressionVideoFrameRate VideoFrameRate { get; set; } = CompressionVideoFrameRate.Auto;

		/// <summary>
		///		Interval of I frame.
		/// </summary>
		public CompressionIntervalFrameI IntervalFrameI { get; set; } = CompressionIntervalFrameI.Auto;

		public CompressionIntervalBPFrame IntervalBPFrame { get; set; } = CompressionIntervalBPFrame.BBP;

		public CompressionVideoEncoding VideoEncoding { get; set; } = CompressionVideoEncoding.Auto;

		public CompressionAudioEncoding AudioEncoding { get; set; } = CompressionAudioEncoding.Auto;

		public CompressionVideoEncodingComplexity VideoEncodingComplexity { get; set; } = CompressionVideoEncodingComplexity.Auto;

		/// <summary>
		///		SVC: Scalable Video Coding, can be encoded by level.
		/// </summary>
		public bool IsSvcEnable { get; set; } = false;

		public CompressionFormatType FormatType { get; set; } = CompressionFormatType.ExposedStream;

		public CompressionAudioBitrate AudioBitrate { get; set; } = CompressionAudioBitrate.Default;

		/// <summary>
		///		Can be [1~100].
		///		If 0 - Unused parameter.
		///		By default StreamSmooth = SteamSmoothUnused.
		/// </summary>
		private int streamSmooth;

		public int StreamSmooth
		{
			get => streamSmooth; // By default SteamSmooth = SteamSmoothUnused = 0
			set => streamSmooth = (value >= 0 && value <= 100) ? value : throw new NvrExceptions.NvrBadLogicException($"SteamSmooth must be in range [0~100]. Current value is {value}");
		}

		public CompressionAudioSamplingRate AudioSamplingRate { get; set; } = CompressionAudioSamplingRate.Default;

		public bool IsSmartCodecEnabled { get; set; } = false;

		/// <summary>
		///		Depth Map Enable.
		/// </summary>
		public bool IsDepthMapEnabled { get; set; } = false;

		/// <summary>
		///		Average video bitrate (valid when isSmartCodecEnabled = true).
		/// </summary>
		public CompressionAverageVideoBitrate AverageVideoBitrate { get; set; } = CompressionAverageVideoBitrate.AVB_0Kb;

	}
}