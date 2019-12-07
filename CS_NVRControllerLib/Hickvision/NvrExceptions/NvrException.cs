using System;

namespace CS_NVRController.Hickvision.NvrExceptions {
	public class NvrException: Exception {
		public NvrException(string message) 
			: base(message)
		{}

		public NvrException(string message, Exception innerException) 
			: base(message, innerException)
		{}
	}
}
