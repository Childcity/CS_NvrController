using System;

namespace CS_NVRController.Hickvision.NvrExceptions {
	public class NvrBadLogicException: NvrException {
		public NvrBadLogicException(string message) 
			: base(message)
		{}

		public NvrBadLogicException(string message, Exception innerException) 
			: base(message, innerException)
		{}
	}
}
