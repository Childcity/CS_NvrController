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

		public IntPtr GetLiveViewHandle() => streamWnd1.Handle;

		public event PaintEventHandler OnLiveViewPictureBoxPaint {
			add { streamWnd1.Paint += value; }
			remove { streamWnd1.Paint -= value; }
		}

		public void ResetLiveView()
		{
			streamWnd1.BackColor = Color.White;
			streamWnd1.BackColor = Color.Gainsboro;
			streamWnd1.Invalidate();
		}
	}
}
