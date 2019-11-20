using System;

namespace CS_NVRController.DAL.Hickvision {
	public class NvrException: Exception {
		public NvrException(string message) 
			: base(message)
		{}

		public NvrException(string message, Exception innerException) 
			: base(message, innerException)
		{}
	}
}
