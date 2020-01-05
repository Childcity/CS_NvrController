using CS_NVRController.BLL;
using CS_NVRController.Hickvision;
using CS_NVRControllerGUI.GUI;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_NVRController {
	public partial class MainWindow: Form {

		private PreviewWindow previewWindow_ = null;

		private bool isLogedIn_ = false;

		private bool isPreviewRunningIn_ = false;

		private delegate void appendLog(string log);

		private LiveViewService liveViewService_ = new LiveViewService();

		public MainWindow() {
			InitializeComponent();

			textBox4.Text = getAppConfiguration("NvrIp");
			textBox3.Text = getAppConfiguration("NvrPort");
			textBox1.Text = getAppConfiguration("NvrUserName");
			textBox2.Text = getAppConfiguration("NvrUserPassword");

			liveViewService_.OnException += appendLogOnUiThread;

			NvrPreviewSettings previewSettings = liveViewService_.NvrPreviewSettings;
			numericUpDown1.Value = previewSettings.StreamType;
			numericUpDown2.Value = previewSettings.DisplayBufNum;
			checkBox1.Checked = previewSettings.IsPassbackRecord;
			checkBox2.Checked = previewSettings.IsBlocked;

			comboBox2.Items.AddRange(Enum.GetNames(typeof(PreviewLinkModeType)));
			comboBox2.SelectedIndex = (int)previewSettings.LinkMode;

			comboBox3.Items.AddRange(Enum.GetNames(typeof(PreviewQualityType)));
			comboBox3.SelectedIndex = (int)previewSettings.PreviewQuality;

			comboBox4.Items.AddRange(Enum.GetNames(typeof(PreviewModeType)));
			comboBox4.SelectedIndex = (int)previewSettings.PreviewMode;

			comboBox5.Items.AddRange(Enum.GetNames(typeof(PreviewHandleType)));
			comboBox5.SelectedIndexChanged += (object sender, EventArgs e) => {
				if(comboBox5.SelectedIndex == (int)PreviewHandleType.Direct) {
					subtleSettingsGB.Enabled = false;
				} else {
					subtleSettingsGB.Enabled = true;
				}
			};
			comboBox5.SelectedIndex = (int)previewSettings.PreviewHandleMode;

			previewPanel.Enabled = isLogedIn_;
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			if (! isLogedIn_) {
				if (!short.TryParse(textBox3.Text, out short port)) {
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
				previewPanel.Enabled = isLogedIn_;
				updateAppConfiguration("NvrIp", textBox4.Text);
				updateAppConfiguration("NvrPort", textBox3.Text);
				updateAppConfiguration("NvrUserName", textBox1.Text);
				updateAppConfiguration("NvrUserPassword", textBox2.Text);
			} else {
				button1.Enabled = false;

				{
					liveViewService_.Logout();
					isLogedIn_ = false;
					button1.Text = "Login";
					previewPanel.Enabled = isLogedIn_;

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

		private async void startLiveViewBtn_Click(object sender, EventArgs e)
		{
			if (! isPreviewRunningIn_) {
				if (previewWindow_ == null) {
					previewWindow_ = new PreviewWindow();
					previewWindow_.FormClosing += new FormClosingEventHandler(
						(object s, FormClosingEventArgs args) => startLiveViewBtn.PerformClick()
					);
				}

				liveViewService_.NvrPreviewSettings = new NvrPreviewSettings {
					PreviewHandleMode = (PreviewHandleType)comboBox5.SelectedIndex,
					LinkMode = (PreviewLinkModeType)comboBox2.SelectedIndex,
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
					liveViewService_.StartLiveView(previewWindow_.getLiveViewHandle());
				} catch (Exception) {
					return;
				}

				startLiveViewBtn.Text = "Stop Live View";
				isPreviewRunningIn_ = true;
				userInfoGB.Enabled = ! isPreviewRunningIn_;
				previewSettingsGB.Enabled = ! isPreviewRunningIn_;

				previewWindow_.Show(this);
			} else {
				await stopLiveViewAsync();

				isPreviewRunningIn_ = false;
				startLiveViewBtn.Text = "Start Live View";
				userInfoGB.Enabled = !isPreviewRunningIn_;
				previewSettingsGB.Enabled = !isPreviewRunningIn_;
				previewWindow_.resetLiveView();
				previewWindow_?.Dispose();
				previewWindow_ = null;
			}
		}

		private async Task stopLiveViewAsync()
		{
			liveViewService_.StopLiveView();
			await Task.Delay(500);
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

		private void updateAppConfiguration(string key, string value)
		{
			// Open App.Config of executable
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			// Add an Application Setting
			config.AppSettings.Settings.Remove(key);
			config.AppSettings.Settings.Add(key, value);

			// Save the configuration file.
			config.Save(ConfigurationSaveMode.Modified);
			
			// Force a reload of a changed section.
			ConfigurationManager.RefreshSection("appSettings");
		}

		private string getAppConfiguration(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
