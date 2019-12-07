using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_NVRControllerGUI.GUI {
	public partial class PreviewWindow: Form {
		public PreviewWindow()
		{
			InitializeComponent();
		}

		public IntPtr getLiveViewHandle() => streamWnd1.Handle;

		public void resetLiveView()
		{
			streamWnd1.BackColor = Color.White;
			streamWnd1.BackColor = Color.Gainsboro;
		}
	}
}
