using CS_NVRController.BLL;
using CS_NVRController.DAL.Hickvision;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_NVRController {
	public partial class MainWindow: Form {

		private bool isLogedIn_ = false;

		private bool isPreviewRunningIn_ = false;

		private delegate void appendLog(string log);

		private LiveViewService liveViewService_ = new LiveViewService();

		public MainWindow() {
			InitializeComponent();

			liveViewService_.OnException += appendLogOnUiThread;

			NvrPreviewSettings previewSettings = liveViewService_.NvrPreviewSettings;
			numericUpDown1.Value = previewSettings.StreamType;
			numericUpDown2.Value = previewSettings.DisplayBufNum;
			checkBox1.Checked = previewSettings.IsPassbackRecord;
			checkBox2.Checked = previewSettings.IsBlocked;

			comboBox2.Items.AddRange(Enum.GetNames(typeof(LinkModeType)));
			comboBox2.SelectedIndex = (int)previewSettings.LinkMode;

			comboBox3.Items.AddRange(Enum.GetNames(typeof(PreviewQualityType)));
			comboBox3.SelectedIndex = (int)previewSettings.PreviewQuality;

			comboBox4.Items.AddRange(Enum.GetNames(typeof(PreviewModeType)));
			comboBox4.SelectedIndex = (int)previewSettings.PreviewMode;

			groupBox2.Enabled = isLogedIn_;
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			if (! isLogedIn_) {
				short port;
				if (! short.TryParse(textBox3.Text, out port)) {
					port = 9000;
					textBox3.Text = "" + port;
				}

				liveViewService_.SessionInfo = new NvrSessionInfo() {
					IPAddress = textBox4.Text,
					PortNumber = port,
					UserName = textBox1.Text,
					UserPassword = textBox2.Text
				};

				try {
					button1.Enabled = false;
					await liveViewService_.LoginAsync();
				} catch (Exception) {
					return;
				} finally {
					button1.Enabled = true;
				}

				comboBox1.Items.Clear();
				liveViewService_.CameraChannels.ForEach(chan => comboBox1.Items.Add(chan));
				comboBox1.SelectedIndex = liveViewService_.CameraSelectedChannel;

				isLogedIn_ = true;
				button1.Text = "Logout";
				groupBox2.Enabled = isLogedIn_;
			} else {
				button1.Enabled = false;

				{
					liveViewService_.Logout();
					isLogedIn_ = false;
					button1.Text = "Login";
					groupBox2.Enabled = isLogedIn_;

					await stopLiveViewAsync();
				}

				button1.Enabled = true;
			}
		}

		private uint parseBufferSize(string str)
		{
			string[] numsStrings = str.Replace(" ", "").Trim().Split('*');

			uint buffSize = 1;
			foreach (string numStr in numsStrings) {
				if(uint.TryParse(numStr, out uint num)) {
					buffSize *= num;
				} else {
					MessageBox.Show("PlayerBufSize is not a number!", "Warning");
					buffSize = 0;
					break;
				}
			}

			if (buffSize < 1) {
				// set and return default
				textBox5.Text = "" + new NvrPreviewSettings().PlayerBufSize;
				return uint.Parse(textBox5.Text);
			} else {
				return buffSize;
			}
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			if (! isPreviewRunningIn_) {
				liveViewService_.NvrPreviewSettings = new NvrPreviewSettings {
					LinkMode = (LinkModeType)comboBox2.SelectedIndex,
					StreamType = (uint)numericUpDown1.Value,
					DisplayBufNum = (uint)numericUpDown2.Value,
					IsBlocked = checkBox2.Checked,
					IsPassbackRecord = checkBox1.Checked,
					PlayerBufSize = parseBufferSize(textBox5.Text),
					PreviewMode = (PreviewModeType)comboBox4.SelectedIndex,
					PreviewQuality = (PreviewQualityType)comboBox3.SelectedIndex
				};

				liveViewService_.CameraSelectedChannel = comboBox1.SelectedIndex;

				try {
					liveViewService_.StartLiveView(streamWnd1.Handle);
				} catch (Exception) {
					return;
				}

				button2.Text = "Stop Live View";
				isPreviewRunningIn_ = true;
			} else {
				await stopLiveViewAsync();
			}
		}

		private async Task stopLiveViewAsync()
		{
			liveViewService_.StopLiveView();
			button2.Enabled = false;

			await Task.Delay(500);

			button2.Text = "Start Live View";
			streamWnd1.BackColor = Color.White;
			streamWnd1.BackColor = Color.DarkGray;
			button2.Enabled = true;
			isPreviewRunningIn_ = false;
		}

		private void appendLogOnUiThread(object sender, string log)
		{
			if (InvokeRequired) {
				Invoke(new appendLog((string s) => {
					logTxtBox.AppendText(log);
					logTxtBox.AppendText("\n\n");
				}), log);
			} else {
				logTxtBox.AppendText(log);
				logTxtBox.AppendText("\n\n");
			}
		}
	}
}
