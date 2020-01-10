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

		private bool isPreviewRunning_ = false;

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
			UserSessionService userSessionService = UserSessionService.GetInstance();

			if (! isLogedIn_) {
				if (!short.TryParse(textBox3.Text, out short port)) {
					port = 9000;
					textBox3.Text = "" + port;
				}

				try {
					button1.Enabled = false;
					await userSessionService.LoginAsync(new NvrSessionInfo() {
						IPAddress = textBox4.Text,
						PortNumber = port,
						UserName = textBox1.Text,
						UserPassword = textBox2.Text
					});
				} catch (Exception ex) {
					appendLogOnUiThread(null, $"{ex.Message}\n\n");
					return;
				} finally {
					button1.Enabled = true;
				}

				comboBox1.Items.Clear();
				userSessionService.CameraChannels.ForEach(chan => comboBox1.Items.Add(chan));
				comboBox1.SelectedIndex = userSessionService.CameraSelectedChannel;

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
					if (isPreviewRunning_) {
						await stopLiveViewAsync();
					}

					userSessionService.Logout();
					isLogedIn_ = false;
					button1.Text = "Login";
					previewPanel.Enabled = isLogedIn_;
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
			if (! isPreviewRunning_) {
				if (previewWindow_ == null) {
					previewWindow_ = new PreviewWindow();
					previewWindow_.FormClosing += new FormClosingEventHandler(
						// Perform click will cause calling startLiveViewBtn() method, which stop preview and clean resources
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

				try {
					liveViewService_.StartLiveView(previewWindow_.GetLiveViewHandle());
				} catch (Exception) {
					return;
				}

				startLiveViewBtn.Text = "Stop Live View";
				isPreviewRunning_ = true;
				userInfoGB.Enabled = ! isPreviewRunning_;
				previewSettingsGB.Enabled = ! isPreviewRunning_;

				previewWindow_.Show(this);
			} else {
				await stopLiveViewAsync();

				isPreviewRunning_ = false;
				startLiveViewBtn.Text = "Start Live View";
				userInfoGB.Enabled = !isPreviewRunning_;
				previewSettingsGB.Enabled = !isPreviewRunning_;
				previewWindow_?.ResetLiveView();
				previewWindow_?.Dispose();
				previewWindow_ = null;
			}
		}

		private async Task stopLiveViewAsync()
		{
			liveViewService_.StopLiveView();
			await Task.Delay(500);
		}

		private async void pictureSettingsBtn_Click(object sender, EventArgs e)
		{
			NvrCompressionSettings compressionSettings;

			try {
				// Getting async settings
				compressionSettings = await liveViewService_.LoadPreviewPictureSettings();
			} catch (Exception) {
				return;
			}

			using(var pictureSettingsWindow = new PictureSettingsWindow(compressionSettings)) {
				if (DialogResult.OK == pictureSettingsWindow.ShowDialog(this)) {
					// User Clicked 'save button' => save settings 
					try {
						compressionSettings = pictureSettingsWindow.PictureSettings;
						await liveViewService_.UpdatePreviewPictureSettings(compressionSettings);
					} catch (Exception) {
						return;
					}
				}
			}
		}

		private void playbackBtn_Click(object sender, EventArgs e)
		{
			using (var playbackWindow = new PlaybackControlWindow()) {
				playbackWindow.ShowDialog(this);
			}
		}

		private void appendLogOnUiThread(object sender, string log)
		{
			if (InvokeRequired) {
				Invoke(new appendLog((string s) => {
					logTxtBox.AppendText(s);
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

		private string getAppConfiguration(string key) => ConfigurationManager.AppSettings[key];

		private void logTxtBox_DoubleClick(object sender, EventArgs e) => logTxtBox.Text = "Log...\n\n";

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
			=> UserSessionService.GetInstance().CameraSelectedChannel = comboBox1.SelectedIndex;
	}
}
