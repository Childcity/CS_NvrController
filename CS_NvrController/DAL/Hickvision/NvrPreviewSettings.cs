namespace CS_NVRController.DAL.Hickvision {
	public class NvrPreviewSettings {

		/*
		 * General settings
		 */

		// 0-main stream, 1-sub stream, 2-stream 3, 3- virtual stream, and so on 
		public uint StreamType { get; set; } = 0;

		// Tcp, Udp, Multicast, Rtp, RtpRtsp, RstpHttp, HrUdp
		public LinkModeType LinkMode { get; set; } = LinkModeType.Tcp;

		// false - non-blocking stream getting, 
		// true - blocking stream getting. 
		// if it is block, there will be 5s timeout return when connect failed in the SDK, 
		// it is not suitable for polling stream gettitng. 
		public bool IsBlocked { get; set; } = true;

		// 0-disable video passback, 1-enable video passback. back tracking when 
		// ANR disconnected- devices send the data automaticly after the network recovery between client and devices.
		// (need devices support) 
		public bool IsPassbackRecord { get; set; } = false;

		// Preview mode: Normal or Dalayed
		public PreviewModeType PreviewMode { get; set; } = PreviewModeType.Normal;

		// The max buffer frames of player SDK,value range: 1, 6 (default, self-adaptive mode), 15. It is 1 when setting to 0. 
		// (Play library display buffer maximum frame number)
		public uint DisplayBufNum { get; set; } = 15;

		/*
		 * Player settings
		 */

		// Player buffer size (this buffer will be used for realtime video streaming)
		public uint PlayerBufSize { get; set; } = 2 * 1024 * 1024;

		// Picture quality (if device doesn't support this, set to PreviewQualityType.NotSet)
		public PreviewQualityType PreviewQuality { get; set; } = PreviewQualityType.NotSet;

	}
	
	public enum LinkModeType { Tcp, Udp, Multicast, Rtp, RtpRtsp, RstpHttp, HrUdp }

	public enum PreviewModeType { Normal, Dalayed }

	public enum PreviewQualityType { Low, High, NotSet }
}
