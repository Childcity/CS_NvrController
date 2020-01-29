using System;

namespace CS_NVRController.Hickvision {

	public class NvrRecordLabel {

		/// <summary>
		///		Unique identificator in Base64 format.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///		Title (chould be <= 40 bytes).
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///		Data and time.
		/// </summary>
		public DateTime Time { get; set; }
	}
}