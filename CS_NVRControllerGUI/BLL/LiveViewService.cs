using CS_NVRController.Hickvision;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS_NVRController.BLL {
	public class LiveViewService: IDisposable {

		private NvrController nvrController_ = null;

		public LiveViewService()
		{}

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

		public event EventHandler<string> OnException;

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
				nvrController_.StartPreview(playWndHandle, NvrPreviewSettings);
				nvrController_.OnPreviewError += onPreviewError;
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
				nvrController_?.StopPreview();
			} catch (NvrSdkException ex) {
				logNvrSdkExceprtion(ex);
			} catch (Exception ex) {
				logException(ex);
			} finally {
				if(nvrController_ != null)
					nvrController_.OnPreviewError -= onPreviewError;
			}
		}

		public NvrSessionInfo SessionInfo { get; set; } = new NvrSessionInfo();

		public NvrPreviewSettings NvrPreviewSettings { get; set; } = new NvrPreviewSettings();

		public List<string> CameraChannels {
			get {
				var camChannals = new List<string>();
				nvrController_.IpChannels.ForEach(x => {
					camChannals.Add(x.Item1 + " " + x.Item2 + " [" + x.Item3 + "]");
				});
				return camChannals;
			}
		}

		public int CameraSelectedChannel {
			get { return nvrController_.SelectedChannelId; }
			set { nvrController_.SelectedChannelId = value; }
		}

		private void onPreviewError(object sender, Tuple<uint, string> args)
		{
			OnException?.Invoke(this, $"PreviewError: Code[{args.Item1}]: {args.Item2}");
		}

		private void logException(Exception ex)
		{
			Console.WriteLine($"[Debug] LiveViewService: Exception: {ex.Message}\n{ex.StackTrace}\n\n");
			OnException?.Invoke(this, $"Exception: {ex.Message}\n{ex.StackTrace}\n\n");
		}

		private void logNvrSdkExceprtion(NvrSdkException ex)
		{
			Console.WriteLine($"[Debug] LiveViewService: Exception: {ex.Message}\n{ex.StackTrace}\n\n");
			OnException?.Invoke(this, $"NvrException Code[{ex.SdkErrorCode}]: {ex.Message}\n\n");
		}
	}
}
