using System;

namespace CS_NVRController.Hickvision.NvrExceptions {
	public class NvrSetPlayerSpeedException: NvrSdkException {

		public NvrSetPlayerSpeedException(uint sdkErrCode, string message)
			: base(sdkErrCode, message)
		{}

		public NvrSetPlayerSpeedException(uint sdkErrCode, string message, Exception innerException)
			: base(sdkErrCode, message, innerException)
		{}
	}
}
