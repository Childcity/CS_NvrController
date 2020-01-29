using CS_NVRController.BLL;
using CS_NVRController.Hickvision;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CS_NVRControllerGUI.GUI
{
	public partial class RecordLabelWindow: Form {

		private Size formSize;
		private BookmarkService bookmarkService_ = null;
		private ListViewItem newRecordLabelRow = null;
		private DateTime newRecordLabelDateTime;
		private delegate void addBookmark(string[] rowTxt);

		public RecordLabelWindow() => InitializeComponent();

		private void recordLabelWindow_Load(object sender, EventArgs e)
		{
			bookmarkService_ = new BookmarkService();

			formSize = Size;

			dateTimePicker1.Value = DateTime.Now.AddDays(-7);
			dateTimePicker2.Value = DateTime.Now;
			bookmarkService_.OnBookmarkFetched += onBookmarkFetched;

			labelTitleTb.TextChanged += resetLoadingBookmarks;
			dateTimePicker1.ValueChanged += resetLoadingBookmarks;
			dateTimePicker2.ValueChanged += resetLoadingBookmarks;
		}

		private void onBookmarkFetched(object sender, NvrRecordLabel bookmark)
		{
			int existedWithSameId = bookmarksLv.Items
				.Cast<ListViewItem>()
				.Select(item => item.Text.Equals(bookmark.Id))
				.Count();

			// check if bookmark is exist in list and return if true
			if (existedWithSameId > 0) {
				return;
			}

			string[] newBookMark = new string[] {
				bookmark.Id, bookmark.Time.ToString("yyyy-MM-dd HH:mm:ss"), bookmark.Title
			};

			// this callback may be called from another thread, so we should check
			// if InvokeRequired
			if (InvokeRequired) {
				Invoke(new addBookmark((string[] rowTxt) => {
					bookmarksLv.Items.Add(new ListViewItem(rowTxt));
					bookmarksLv.Invalidate();
				}), new object[] { newBookMark });
			} else {
				bookmarksLv.Items.Add(new ListViewItem(newBookMark));
				bookmarksLv.Invalidate();
			}
		}

		private async void fetchNextBtn_Click(object sender, EventArgs e)
		{
			Enabled = false;
			Cursor = Cursors.WaitCursor;

			try {
				await bookmarkService_.LoadMoreBookmarks(dateTimePicker1.Value, dateTimePicker2.Value, labelTitleTb.Text);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			Cursor = Cursors.Default;
			Enabled = true;
		}

		private void addNewBookmarkBtn_Click(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;
			bookmarksLv.Items.Add(new ListViewItem(new string[] {"", now.ToString("yyyy-MM-dd HH:mm:ss"), ""}));
			newRecordLabelDateTime = now;

			// Get Location of Cell with new bookmark and put on it TextBox 'titleTb'
			newRecordLabelRow = bookmarksLv.Items[bookmarksLv.Items.Count - 1];
			var lastEditable = newRecordLabelRow.SubItems[2];
			int cellWidth = lastEditable.Bounds.Width;
			int cellHeight = lastEditable.Bounds.Height;
			int cellLeft = 2 + bookmarksLv.Left + lastEditable.Bounds.Left;
			int cellTop = bookmarksLv.Top + lastEditable.Bounds.Top;

			// Put on Cell TextBox 'titleTb'
			titleTb.Location = new Point(cellLeft, cellTop);
			titleTb.Size = new Size(cellWidth, cellHeight);
			titleTb.Visible = true;
			titleTb.BringToFront();
			titleTb.Text = "Bookmarked at " + now.ToString("yyyy-MM-dd HH:mm:ss");
			titleTb.Select();
			titleTb.SelectAll();
		}

		private async void hideTextEditor()
		{
			var lastEditable = newRecordLabelRow?.SubItems[2];
			
			if (lastEditable == null)
				return;

			var lastEditebleIdCell = newRecordLabelRow.SubItems[0];
			newRecordLabelRow = null;

			// Hide from Cell TextBox 'titleTb' and update cell value
			titleTb.Visible = false;
			lastEditable.Text = titleTb.Text;

			try {
				var savedBookmark = await bookmarkService_.SaveNewBookmark(newRecordLabelDateTime, lastEditable.Text);
				lastEditebleIdCell.Text = savedBookmark.Id;
			} catch (Exception ex) {
				lastEditebleIdCell.Text = "Error";
				lastEditebleIdCell.BackColor = Color.Pink;
				MessageBox.Show(ex.Message);
			}
		}

		private async void deleteSelectedBookmarks()
		{
			try {
				string[] bookmarkIdsInBase64 = bookmarksLv.SelectedItems
					.Cast<ListViewItem>()
					.Select(item => item.Text)
					.ToArray();

				await bookmarkService_.DeleteBookmarks(bookmarkIdsInBase64);

				foreach (ListViewItem item in bookmarksLv.SelectedItems) {
					bookmarksLv.Items.Remove(item);
				}
				bookmarksLv.Invalidate();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void resetLoadingBookmarks(object sender, EventArgs e)
		{
			try {
				bookmarkService_.ResetLoading();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			bookmarksLv.Items.Clear();
			bookmarksLv.Invalidate();
		}

		private void bookmarksLv_MouseDown(object sender, MouseEventArgs e) => hideTextEditor();

		private void bookmarksLv_Scroll(object sender, EventArgs e) => hideTextEditor();

		private void titleTb_Leave(object sender, EventArgs e) => hideTextEditor();

		private void titleTb_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
				hideTextEditor();
		}

		private void bookmarksLv_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				deleteSelectedBookmarks();
		}

		private void recordLabelWindow_Resize(object sender, EventArgs e)
		{
			var delta = Size - formSize;
			bookmarksLv.Size += delta;
			formSize = Size;
		}

	}
}
