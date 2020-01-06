using CS_NVRController.Hickvision;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace CS_NVRController.BLL {
	public class LiveViewService: IDisposable {

		private NvrController nvrController_ = null;

		#region IDisposable Support
		private bool disposedValue = false; // Для определения избыточных вызовов

		protected virtual void dispose(bool disposing)
		{
			if (!disposedValue) {
				if (disposing) {
					Logout();
				}
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			dispose(true);
		}
		#endregion

		#region PublicMethods

		public async Task LoginAsync()
		{
			string sdkLogDir = System.Reflection.Assembly.GetEntryAssembly().Location + @"_NvrSdkLogs\";
			nvrController_ = new NvrController(SessionInfo, sdkLogDir, true);

			await Task.Factory.StartNew(() => {
				try {
					nvrController_.StartSession();
				} catch (NvrSdkException ex) {
					logNvrSdkExceprtion(ex);
					throw new SystemException("NvrController: StartSession failed", ex);
				} catch (Exception ex) {
					logException(ex);
					throw new SystemException("NvrController: StartSession failed", ex);
				}
			}, TaskCreationOptions.AttachedToParent);
		}

		public void Logout()
		{
			try {
				nvrController_?.StopPreview();
				nvrController_?.StopSession();
			} catch (Exception e) {
				Console.WriteLine(e.Message + "\n" + e.StackTrace);
			} finally {
				nvrController_?.Dispose();
				nvrController_ = null;
			}
		}

		public void StartLiveView(IntPtr playWndHandle)
		{
			try {
				nvrController_.DrawOnPictureHandle += drawSomething;
				nvrController_.OnPreviewError += onPreviewError;
				nvrController_.StartPreview(playWndHandle, NvrPreviewSettings);
			} catch (NvrSdkException ex) {
				logNvrSdkExceprtion(ex);
				throw new SystemException("NvrController: StartPreview failed", ex);
			} catch (Exception ex) {
				logException(ex);
				throw new SystemException("NvrController: StartPreview failed", ex);
			}
		}

		public void StopLiveView()
		{
			try {
				nvrController_.DrawOnPictureHandle -= drawSomething;
				nvrController_.OnPreviewError -= onPreviewError;
				nvrController_.StopPreview();
			} catch (NvrSdkException ex) {
				logNvrSdkExceprtion(ex);
			} catch (Exception ex) {
				logException(ex);
			} finally {
				if (nvrController_ != null) {
					nvrController_.OnPreviewError -= onPreviewError;
					nvrController_.DrawOnPictureHandle -= drawSomething;
				}
			}
		}

		#endregion

		#region Properties

		public NvrSessionInfo SessionInfo { get; set; } = new NvrSessionInfo();

		public NvrPreviewSettings NvrPreviewSettings { get; set; } = new NvrPreviewSettings();

		public List<string> CameraChannels
		{
			get {
				var camChannals = new List<string>();
				nvrController_.IpChannels.ForEach(x => {
					camChannals.Add(x.Item1 + " " + x.Item2 + " [" + x.Item3 + "]");
				});
				return camChannals;
			}
		}

		public int CameraSelectedChannel
		{
			get { return nvrController_.SelectedChannelId; }
			set { nvrController_.SelectedChannelId = value; }
		}

		public async Task<NvrCompressionSettings> LoadPreviewPictureSettings()
		{
			return await Task<NvrCompressionSettings>.Factory.StartNew(() => {
				try {
					return nvrController_.LoadStreamCompressionSettings();
				} catch (NvrSdkException ex) {
					logNvrSdkExceprtion(ex);
					throw new SystemException("NvrController: LoadStreamCompressionSettings failed", ex);
				} catch (Exception ex) {
					logException(ex);
					throw new SystemException("Exception: LoadStreamCompressionSettings failed", ex);
				}
			}, TaskCreationOptions.AttachedToParent);
		}

		public async Task UpdatePreviewPictureSettings(NvrCompressionSettings compressionSettings)
		{
			await Task.Factory.StartNew(() => {
				try {
					nvrController_.UpdateStreamCompressionSettings(compressionSettings);
				} catch (NvrSdkException ex) {
					logNvrSdkExceprtion(ex);
					throw new SystemException("NvrController: UpdateStreamCompressionSettings failed", ex);
				} catch (Exception ex) {
					logException(ex);
					throw new SystemException("Exception: UpdateStreamCompressionSettings failed", ex);
				}
			}, TaskCreationOptions.AttachedToParent);
		}

		#endregion

		#region PublicEvents

		public event EventHandler<string> OnException;

		#endregion

		#region PrivateMethods
		private void drawSomething(IntPtr hDc)
		{
			try {
				using (Graphics pDc = Graphics.FromHdc(hDc)) {
					if (pDc == null)
						return;

					using (Brush brush = new SolidBrush(Color.DarkRed)) {
						using (Pen pen = new Pen(brush)) {
							Rectangle rectTmp = new Rectangle(5, 5, 300, 20);

							string strText = DateTime.Now.ToString() + "     Meeting Room";
							using (Font font = new Font("Blackbody", 10, FontStyle.Italic | FontStyle.Bold)) {
								//Text
								pDc.DrawString(strText, font, brush, 6, 6);
								//Rectangle
								pDc.DrawRectangle(pen, rectTmp);
							}
						}
					}
				}
			} catch (Exception ex) {
				OnException?.Invoke(this, $"Exception: {ex.Message}\n{ex.StackTrace}\n\n");
			}
		}

		private void onPreviewError(object sender, Tuple<uint, string> args)
		{
			Console.WriteLine($"[Debug] LiveViewService: PreviewError: Code[{args.Item1}]: {args.Item2}\n\n");
			OnException?.Invoke(this, $"PreviewError: Code[{args.Item1}]: {args.Item2}\n\n");
		}

		private void logException(Exception ex)
		{
			Console.WriteLine($"[Debug] LiveViewService: Exception: {ex.Message}\n{ex.StackTrace}\n\n");
			OnException?.Invoke(this, $"Exception: {ex.Message}\n\n{ex.StackTrace}\n\n");
		}

		private void logNvrSdkExceprtion(NvrSdkException ex)
		{
			Console.WriteLine($"[Debug] LiveViewService: Exception: {ex.Message}\n{ex.StackTrace}\n\n");
			OnException?.Invoke(this, $"NvrException Code[{ex.SdkErrorCode}]: {ex.Message}\n\n");
		}
		#endregion
	}
}
