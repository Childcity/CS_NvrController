using System;

namespace CS_NVRController.Hickvision.NvrExceptions {
	public class NvrSdkException: NvrException {

		//public NvrSdkException(string message)
		//	: base(message)
		//{
		//	SdkErrorCode = uint.MaxValue;
		//}

		public NvrSdkException(uint sdkErrCode, string message)
			: base(message)
		{
			SdkErrorCode = sdkErrCode;
		}

		public NvrSdkException(uint sdkErrCode, string message, Exception innerException)
			: base(message, innerException)
		{
			SdkErrorCode = sdkErrCode;
		}

		public uint SdkErrorCode { get; private set; }
	}
}
