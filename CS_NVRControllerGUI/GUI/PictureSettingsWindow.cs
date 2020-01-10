using CS_NVRController.Hickvision;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CS_NVRControllerGUI.GUI {

	public partial class PictureSettingsWindow: Form {

		public PictureSettingsWindow(NvrCompressionSettings pictureSettings)
		{
			InitializeComponent();
			PictureSettings = pictureSettings;
		}

		public NvrCompressionSettings PictureSettings { get; private set; } = null;

		private void pictureSettingsWindow_Load(object sender, EventArgs e)
		{
			// Remove focus from droDown list, that is default
			ActiveControl = label1;

			// Get all Properties of enum 'NvrCompressionSettings'
			var props = typeof(NvrCompressionSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			for (int i = 0; i < props.Length; i++) {
				// Set text of label of settings
				((Label)tableLayoutPanel1.GetControlFromPosition(0, i)).Text
					= Regex.Replace(props[i].Name, "([a-z])([A-Z])", "$1 $2"); // add space before upper case

				// Fill dropdown list for current setting
				var dropDownList = (ComboBox)tableLayoutPanel1.GetControlFromPosition(1, i);
				if (props[i].PropertyType == typeof(bool)) {
					dropDownList.Items.AddRange(new object[] { "True", "False" });
				} else if (props[i].PropertyType == typeof(int)) {
					dropDownList.Items.AddRange(Enumerable.Range(0, 101).Select(el => (object)el).ToArray());
				} else { //this is enum
					dropDownList.Items.AddRange(Enum.GetNames(props[i].PropertyType));
				}

				// Set value of current setting
				dropDownList.SelectedText = props[i].GetValue(PictureSettings).ToString();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// Get all Properties of enum 'NvrCompressionSettings'
			var props = typeof(NvrCompressionSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			for (int i = 0; i < props.Length; i++) {
				// Get selected setting value
				string selectedText = ((ComboBox)tableLayoutPanel1.GetControlFromPosition(1, i)).Text;

				try {
					if (props[i].PropertyType == typeof(bool)) {
						props[i].SetValue(PictureSettings, selectedText == "True" ? true : false);
					} else if (props[i].PropertyType == typeof(int)) {
						props[i].SetValue(PictureSettings, int.Parse(selectedText));
					} else { //this is enum
						var val = Enum.Parse(props[i].PropertyType, selectedText);
						props[i].SetValue(PictureSettings, val);
					}
				} catch (Exception ex) {
					MessageBox.Show($"Settings '{props[i].Name}' is not correct!\n" +
						$"Current value: {((ComboBox)tableLayoutPanel1.GetControlFromPosition(1, i)).Text}\n\n" +
						$"Exception: {ex.Message}");
					return;
				}
			}
		}
	}
}