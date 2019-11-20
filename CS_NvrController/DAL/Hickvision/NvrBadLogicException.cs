using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_NVRController.DAL.Hickvision {
	public class NvrBadLogicException: NvrException {
		public NvrBadLogicException(string message) 
			: base(message)
		{}

		public NvrBadLogicException(string message, Exception innerException) 
			: base(message, innerException)
		{}
	}
}
