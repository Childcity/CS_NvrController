using CS_NVRController.Hickvision.NvrController;
using CS_NVRController.Hickvision.NvrExceptions;
using System;

namespace CS_NVRController.BLL {
	public class PlaybackService: IDisposable {

		private NvrPlayback nvrPlayback_ = null;

		#region IDisposable Support
		private bool disposedValue = false; // Для определения избыточных вызовов

		protected virtual void dispose(bool disposing)
		{
			if (!disposedValue) {
				if (disposing) {
					try {
						nvrPlayback_?.Dispose();
					} finally { }
				}
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			dispose(true);
		}
		#endregion

		public PlaybackService()
		{
			nvrPlayback_ = new NvrPlayback(UserSessionService.GetInstance().NvrUserSession, true);
		}

		#region PublicMethods

		public void Play(IntPtr playWndHandle)
		{
			try {
				nvrPlayback_.StartPreview(playWndHandle, TimeInterval.start, TimeInterval.end);
			} catch (NvrSdkException ex) {
				throw new SystemException($"StartPreview failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
			} catch (Exception ex) {
				throw new SystemException("StartPreview failed: " + ex.Message, ex);
			}
		}

		public void Resume()
		{
			try {
				nvrPlayback_.ResumePreview();
			} catch (NvrSdkException ex) {
				throw new SystemException($"ResumePreview failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
			} catch (Exception ex) {
				throw new SystemException("ResumePreview failed: " + ex.Message, ex);
			}
		}

		public void Pause()
		{
			try {
				nvrPlayback_.PausePreview();
			} catch (NvrSdkException ex) {
				throw new SystemException($"PausePreview failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
			} catch (Exception ex) {
				throw new SystemException("PausePreview failed: " + ex.Message, ex);
			}
		}

		public void Stop()
		{
			try {
				nvrPlayback_.StopPreview();
			} catch (NvrSdkException ex) {
				throw new SystemException($"StopPreview failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
			} catch (Exception ex) {
				throw new SystemException("StopPreview failed: " + ex.Message, ex);
			}
		}

		public void PauseAndNextFrame()
		{
			try {
				nvrPlayback_.PlayNextFrame();
			} catch (NvrSdkException ex) {
				throw new SystemException($"PauseAndNextFrame failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
			} catch (Exception ex) {
				throw new SystemException("PauseAndNextFrame failed: " + ex.Message, ex);
			}
		}

		#endregion

		#region Properties

		public (DateTime start, DateTime end) TimeInterval { get; set; } = (DateTime.Now, DateTime.Now.AddDays(-1));

		public NvrPlayback.PlayerState PreviwState => nvrPlayback_.PreviewState;

		public NvrPlayback.PlayerSpeed PreviewSpeed
		{
			get => nvrPlayback_.PreviewSpeed;
			set => nvrPlayback_.PreviewSpeed = value;
		}

		#endregion

		#region PublicEvents

		public event EventHandler<NvrPlayback.PlayerState> OnStateChanged
		{
			add { nvrPlayback_.OnPreviewStateChanged += value; }
			remove { nvrPlayback_.OnPreviewStateChanged -= value; }
		}

		public event EventHandler<NvrPlayback.PlayerSpeed> OnSpeedChanged
		{
			add { nvrPlayback_.OnPreviewSpeedChanged += value; }
			remove { nvrPlayback_.OnPreviewSpeedChanged -= value; }
		}

		#endregion

		#region PrivateMethods



		#endregion
	}
}
