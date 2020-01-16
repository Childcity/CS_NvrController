using CS_NVRController.Hickvision;
using CS_NVRController.Hickvision.NvrController;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS_NVRController.BLL {

	public class BookmarkService: IDisposable {
		private static readonly int limit = 20;
		private NvrRecordLabels nvrRecordLabels_ = null;

		#region IDisposable Support

		private bool disposedValue = false; // Для определения избыточных вызовов

		protected virtual void dispose(bool disposing)
		{
			if (!disposedValue) {
				if (disposing) {
					try {
						nvrRecordLabels_?.Dispose();
					} finally {
						nvrRecordLabels_ = null;
					}
				}
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			dispose(true);
		}

		#endregion IDisposable Support

		public BookmarkService()
		{
			nvrRecordLabels_ = new NvrRecordLabels(UserSessionService.GetInstance().NvrUserSession, true);
		}
		
		#region PublicEvents

		public event EventHandler<NvrRecordLabel> OnBookmarkFetched
		{
			add { nvrRecordLabels_.OnLabelFetched += value; }
			remove { nvrRecordLabels_.OnLabelFetched -= value; }
		}

		#endregion PublicEvents

		#region PublicMethods

		public async Task LoadMoreBookmarks(DateTime startDate, DateTime endDate, string labelNameToFind)
		{
			await Task.Factory.StartNew(() => {
				Tuple<NvrRecordLabels.FoundLabelStatus, List<NvrRecordLabel>> labels;
				try {
					if ((!nvrRecordLabels_?.IsRecordLabelFindingActive).Value) {
						nvrRecordLabels_?.StartFindingRecordLabel(startDate, endDate, labelNameToFind);
					}

					labels = nvrRecordLabels_?.FetchNextLabels(limit);
				} catch (NvrSdkException ex) {
					nvrRecordLabels_?.StopFindingRecordLabel();
					throw new SystemException($"LoadBookmarks failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
				} catch (Exception ex) {
					nvrRecordLabels_?.StopFindingRecordLabel();
					throw new SystemException("LoadBookmarks failed: " + ex.Message, ex);
				}

				switch (labels.Item1) {
					case NvrRecordLabels.FoundLabelStatus.FoundAll:
						nvrRecordLabels_?.StopFindingRecordLabel();
						break;
					case NvrRecordLabels.FoundLabelStatus.NotFound:
						nvrRecordLabels_?.StopFindingRecordLabel();
						throw new SystemException("Bookmarks Not found!");
					case NvrRecordLabels.FoundLabelStatus.NvrFileException:
						nvrRecordLabels_?.StopFindingRecordLabel();
						throw new SystemException("Bookmarks can't be loaded, because NVR have problems with Files!");
					case NvrRecordLabels.FoundLabelStatus.Timeout:
						throw new SystemException("To long loading! Try again later!");
					case NvrRecordLabels.FoundLabelStatus.Limited:
						// Do nothing.
						break;
				}

			}, TaskCreationOptions.LongRunning);
		}

		public void ResetLoading()
		{
			try {
				nvrRecordLabels_?.StopFindingRecordLabel();
			} catch (NvrSdkException ex) {
				throw new SystemException($"RestLoading failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
			} catch (Exception ex) {
				throw new SystemException("RestLoading failed: " + ex.Message, ex);
			}
		}

		public async Task<NvrRecordLabel> SaveNewBookmark(DateTime time, string name)
		{
			return await Task.Factory.StartNew(() => {
				try {
					return new NvrRecordLabel() {
						Id = nvrRecordLabels_.SaveRecordLabel(time, name),
						Time = time,
						Title = name
					};
				} catch (NvrSdkException ex) {
					if (ex.SdkErrorCode == 87) { // Maximum number of supported record labels
						throw new SystemException($"SaveNewBookmark failed!\nExceeds maximum number of supported record labels\n\nNvrException Code [{ex.SdkErrorCode}]", ex);
					}
					throw new SystemException($"SaveNewBookmark failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
				} catch (Exception ex) {
					throw new SystemException("SaveNewBookmark failed: " + ex.Message + "\n\n" + ex.InnerException.Message, ex);
				}
			}, TaskCreationOptions.AttachedToParent);
		}

		public async Task DeleteBookmarks(string[] labelIdsBase64)
		{
			await Task.Factory.StartNew(() => {
				try {
					nvrRecordLabels_?.DeleteRecordLabel(labelIdsBase64);
				} catch (NvrSdkException ex) {
					throw new SystemException($"DeleteBookmarks failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
				} catch (Exception ex) {
					throw new SystemException("DeleteBookmarks failed: " + ex.Message, ex);
				}
			}, TaskCreationOptions.AttachedToParent);
		}

		#endregion PublicMethods

	}
}