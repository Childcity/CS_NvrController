using NVRCsharpDemo;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CS_NVRController.Hickvision.NvrController {

	/// <summary>
	///		Provides a mechanism for convinient get set NVR Record Labls.
	/// </summary>
	public partial class NvrRecordLabels: IDisposable {

		#region Private

		private readonly bool isDebugEnabled_ = false;

		private int findHandle_ = -1;

		#endregion Private

		public NvrRecordLabels(NvrUserSession userSession, bool isDebugEnabled)
		{
			NvrUserSession = userSession;
			isDebugEnabled_ = isDebugEnabled;
		}

		#region IDisposable Support

		private bool isDisposed_ = false;

		protected virtual void dispose(bool disposing)
		{
			if (!isDisposed_) {
				if (disposing) {
					debugInfo("~NvrSettings()");

					try {
						if (findHandle_ != -1) {
							StopFindingRecordLabel();
						}
					} finally {
						findHandle_ = -1;
					}

				}

				isDisposed_ = true;
			}
		}

		public void Dispose()
		{
			dispose(true);
		}

		#endregion IDisposable Support

		#region PublicEnums

		/// <summary>
		///		Represent status of Fetched record label
		/// </summary>
		public enum FoundLabelStatus { FoundAll, NotFound, NvrFileException, Timeout, Limited };

		#endregion PublicEnums

		#region PublicProperties

		/// <summary>
		///		Return current user session
		/// </summary>
		public NvrUserSession NvrUserSession { get; private set; }

		/// <summary>
		///		Return true, if StartFindingRecordLabel was called and StopFindingRecordLabel was not called
		///		Return false, if StopFindingRecordLabel was called
		/// </summary>
		public bool IsRecordLabelFindingActive { get => findHandle_ > -1; }

		#endregion PublicEnums

		#region PublicEvents

		/// <summary>
		///		Occurs fhen next record label was fetched
		/// </summary>
		public event EventHandler<NvrRecordLabel> OnLabelFetched;

		#endregion PublicEvents

		#region PublicMethods

		/// <summary>
		///		Save record label in NVR device
		/// </summary>
		/// <param name="labelTime">date and time of record label</param>
		/// <param name="labelName">title of label</param>
		/// <returns></returns>
		public string SaveRecordLabel(DateTime labelTime, string labelName)
		{
			checkUserSessionValid();

			int playHandle = -1;
			string recordlabelIdStr = string.Empty;
			byte[] labelNameInBytes = new byte[CHCNetSDK.LABEL_NAME_LEN];

			try {
				// Get playback handle
				Encoding.ASCII.GetBytes(labelName).CopyTo(labelNameInBytes, 0);
				playHandle = startPlayBackByTime(labelTime.AddMinutes(-1), labelTime);
			} catch (Exception ex) {
				throw new NvrException("SaveRecordLabel failed", ex);
			}

			try {
				CHCNetSDK.NET_DVR_LABEL_IDENTIFY recordlabelId = new CHCNetSDK.NET_DVR_LABEL_IDENTIFY();
				CHCNetSDK.NET_DVR_RECORD_LABEL recordlabelInfo = new CHCNetSDK.NET_DVR_RECORD_LABEL() {
					struTimeLabel = new CHCNetSDK.NET_DVR_TIME() {
						dwYear = (uint)labelTime.Year,
						dwMonth = (uint)labelTime.Month,
						dwDay = (uint)labelTime.Day,
						dwHour = (uint)labelTime.Hour,
						dwMinute = (uint)labelTime.Minute,
						dwSecond = (uint)labelTime.Second,
					},
					byQuickAdd = 0,
					sLabelName = labelNameInBytes
				};
				recordlabelInfo.dwSize = (uint)Marshal.SizeOf(recordlabelInfo);

				if (!CHCNetSDK.NET_DVR_InsertRecordLabel(playHandle, ref recordlabelInfo, ref recordlabelId)) {
					throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_InsertRecordLabel failed");
				}

				byte[] labelId = recordlabelId.sLabelIdentify;
				int labelIdLength = labelId.Length - labelId.Count(by => by == 0x00); // Take length without zero in the end

				recordlabelIdStr = Convert.ToBase64String(recordlabelId.sLabelIdentify, 0, labelIdLength);
				debugInfo($"NET_DVR_InsertRecordLabel succ. Id(Base64)={recordlabelIdStr}");
			} finally {
				if (playHandle > -1) {
					try {
						stopPlayBackByTime(playHandle);
					} finally { }
				}
			}

			return recordlabelIdStr;
		}

		/// <summary>
		///		Save record label in NVR device
		/// </summary>
		/// <param name="labelIdsBase64">array of record label id in Base64 format</param>
		public void DeleteRecordLabel(string[] labelIdsBase64)
		{
			checkUserSessionValid();

			if(labelIdsBase64.Length < 1 || labelIdsBase64.Length > CHCNetSDK.MAX_DEL_LABEL_IDENTIFY) {
				throw new NvrBadLogicException("DeleteRecordLabel failed", new ArgumentException("labelIdsBase64 should be ((labelIdsBase64.Length < 1 || labelIdsBase64.Length > CHCNetSDK.MAX_DEL_LABEL_IDENTIFY))", "labelIdsBase64"));
			}

			CHCNetSDK.NET_DVR_LABEL_IDENTIFY[] idInfos = new CHCNetSDK.NET_DVR_LABEL_IDENTIFY[CHCNetSDK.MAX_DEL_LABEL_IDENTIFY];
			for (int i = 0; i < labelIdsBase64.Length; i++) {
				byte[] labelIdInBytes = new byte[CHCNetSDK.LABEL_IDENTIFY_LEN];
				Convert.FromBase64String(labelIdsBase64?[i]).CopyTo(labelIdInBytes, 0);

				idInfos[i] = new CHCNetSDK.NET_DVR_LABEL_IDENTIFY() {
					sLabelIdentify = labelIdInBytes
				};
			}

			// Call NET_DVR_FindRecordLabel()
			CHCNetSDK.NET_DVR_DEL_LABEL_PARAM labeDelInfo = new CHCNetSDK.NET_DVR_DEL_LABEL_PARAM() {
				byMode = 0x01, //means deleting by identify 
				struIndentify = idInfos,
				wLabelNum = (ushort)idInfos.Length
			};

			labeDelInfo.dwSize = (uint)Marshal.SizeOf(labeDelInfo);
			
			if (!CHCNetSDK.NET_DVR_DelRecordLabel(NvrUserSession.UserSessionState.UserId, ref labeDelInfo)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_DelRecordLabel failed");
			}

			debugInfo("NET_DVR_DelRecordLabel succ!");
		}

		/// <summary>
		///		Initialize record label search engine.
		///		If labelNameToFind is null or empty => engine will search all labels in provided time periaod
		/// </summary>
		/// <param name="startDate">start date and time of search engine</param>
		/// <param name="endDate">end date and time of search engine</param>
		/// <param name="labelNameToFind">if not <see langword="null"/>/empty => the begining of label title to be found</param>
		public void StartFindingRecordLabel(DateTime startDate, DateTime endDate, string labelNameToFind)
		{
			checkUserSessionValid();

			if (IsRecordLabelFindingActive) {
				throw new NvrBadLogicException("Called StartFindingRecordLabel before calling StopFindingRecordLabel");
			}

			byte[] labelNameInBytes = null;

			if (labelNameToFind != null && labelNameToFind?.Length > 0) {
				labelNameInBytes = new byte[40];
				Encoding.ASCII.GetBytes(labelNameToFind).CopyTo(labelNameInBytes, 0);
			}

			// Call NET_DVR_FindRecordLabel()
			CHCNetSDK.NET_DVR_FIND_LABEL labelFindInfo = new CHCNetSDK.NET_DVR_FIND_LABEL() {
				lChannel = NvrUserSession.UserSessionState.SelectedChannelNum,
				sLabelName = labelNameInBytes,
				byDrawFrame = 0,
				struStartTime = new CHCNetSDK.NET_DVR_TIME() {
					dwYear = (uint)startDate.Year,
					dwMonth = (uint)startDate.Month,
					dwDay = (uint)startDate.Day,
					dwHour = (uint)startDate.Hour,
					dwMinute = (uint)startDate.Minute,
					dwSecond = (uint)startDate.Second,
				},
				struStopTime = new CHCNetSDK.NET_DVR_TIME() {
					dwYear = (uint)endDate.Year,
					dwMonth = (uint)endDate.Month,
					dwDay = (uint)endDate.Day,
					dwHour = (uint)endDate.Hour,
					dwMinute = (uint)endDate.Minute,
					dwSecond = (uint)endDate.Second,
				}
			};

			labelFindInfo.dwSize = (uint)Marshal.SizeOf(labelFindInfo);

			findHandle_ = CHCNetSDK.NET_DVR_FindRecordLabel(NvrUserSession.UserSessionState.UserId, ref labelFindInfo);
			if (findHandle_ == -1) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_FindRecordLabel failed");
			}

			debugInfo($"NET_DVR_FindRecordLabel succ! findHandle_={findHandle_}");
		}

		/// <summary>
		///		Get next <paramref name="limit"/> items of record labels
		/// </summary>
		/// <param name="limit">max number of lebels to fetch</param>
		/// <returns></returns>
		public Tuple<FoundLabelStatus, List<NvrRecordLabel>> FetchNextLabels(int limit)
		{
			if (findHandle_ == -1) {
				throw new NvrBadLogicException("Called FetchNextLabels before calling StartFindingRecordLabel()");
			}

			var labelsList = new List<NvrRecordLabel>();

			while (limit-- > 0) {
				// Fetch Next label from NVR
				var labelInfo = findNextRecordLabel(findHandle_);

				if (labelInfo.Item1 == CHCNetSDK.NET_DVR_FILE_SUCCESS) {
					CHCNetSDK.NET_DVR_TIME labelTime = labelInfo.Item2.struTimeLabel;
					byte[] labelId = labelInfo.Item2.struLabelIdentify.sLabelIdentify;
					int labelIdLength = labelId.Length - labelId.Count(by => by == 0x00); // Take length without zero in the end
					labelsList.Add(
						new NvrRecordLabel() {
							Id = Convert.ToBase64String(labelId, 0, labelIdLength),
							Title = Encoding.ASCII.GetString(labelInfo.Item2.sLabelName),
							Time = new DateTime((int)labelTime.dwYear, (int)labelTime.dwMonth, (int)labelTime.dwDay,
												(int)labelTime.dwHour, (int)labelTime.dwMinute, (int)labelTime.dwSecond)
						});
					OnLabelFetched?.BeginInvoke(this, labelsList.Last(), null, null);

				} else if (labelInfo.Item1 == CHCNetSDK.NET_DVR_ISFINDING) {
					return Tuple.Create(FoundLabelStatus.Timeout, labelsList);

				} else if (labelInfo.Item1 == CHCNetSDK.NET_DVR_FILE_NOFIND) {
					return Tuple.Create(FoundLabelStatus.NotFound, labelsList);

				} else if (labelInfo.Item1 == CHCNetSDK.NET_DVR_FILE_EXCEPTION) {
					return Tuple.Create(FoundLabelStatus.NvrFileException, labelsList);

				} else if (labelInfo.Item1 == CHCNetSDK.NET_DVR_NOMOREFILE) {
					return Tuple.Create(FoundLabelStatus.FoundAll, labelsList);

				} else {
					debugInfo($"Record Label finding ended with status: {labelInfo.Item1}");
				}
			};

			return Tuple.Create(FoundLabelStatus.Limited, labelsList);
		}

		/// <summary>
		///		Free record label search engine
		/// </summary>
		public void StopFindingRecordLabel()
		{
			if (!CHCNetSDK.NET_DVR_StopFindLabel(findHandle_)) {
				debugInfo("NET_DVR_StopFindLabel failed");
			}

			findHandle_ = -1;
		}

		#endregion PublicMethods

		#region PrivateMethods

		private Tuple<int /*status*/, CHCNetSDK.NET_DVR_FINDLABEL_DATA> findNextRecordLabel(int findHandle)
		{
			checkUserSessionValid();

			int findStatus = -1;
			CHCNetSDK.NET_DVR_FINDLABEL_DATA labelInfo = new CHCNetSDK.NET_DVR_FINDLABEL_DATA() { };

			{
				// repeat NET_DVR_FindNextLabel until findStatus != CHCNetSDK.NET_DVR_ISFINDING
				int repeatTimes = 0;
				do {
					if (repeatTimes != 0) {
						debugInfo($"Waiting of NET_DVR_FindNextLabel. Iteration={repeatTimes}");
						System.Threading.Thread.Sleep(400); // TODO: this should be rewrited!
					}
					findStatus = CHCNetSDK.NET_DVR_FindNextLabel(findHandle, ref labelInfo);
				} while ((findStatus == CHCNetSDK.NET_DVR_ISFINDING) && ((++repeatTimes) < 10));

				//param = (CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30)Marshal.PtrToStructure(ptr, typeof(CHCNetSDK.NET_DVR_COMPRESSIONCFG_V30));
				debugInfo($"NET_DVR_FindNextLabel succ! findStatus={findStatus}");
			}

			return Tuple.Create(findStatus, labelInfo);
		}

		private int startPlayBackByTime(DateTime startDate, DateTime endDate)
		{
			CHCNetSDK.NET_DVR_VOD_PARA struVodPara = new CHCNetSDK.NET_DVR_VOD_PARA();
			struVodPara.dwSize = (uint)Marshal.SizeOf(struVodPara);
			struVodPara.struIDInfo.dwChannel = (uint)NvrUserSession.UserSessionState.SelectedChannelNum;
			struVodPara.hWnd = IntPtr.Zero;

			// Set the starting time to search video files
			struVodPara.struBeginTime.dwYear = (uint)startDate.Year;
			struVodPara.struBeginTime.dwMonth = (uint)startDate.Month;
			struVodPara.struBeginTime.dwDay = (uint)startDate.Day;
			struVodPara.struBeginTime.dwHour = (uint)startDate.Hour;
			struVodPara.struBeginTime.dwMinute = (uint)startDate.Minute;
			struVodPara.struBeginTime.dwSecond = (uint)startDate.Second;

			// Set the stopping time to search video files
			struVodPara.struEndTime.dwYear = (uint)endDate.Year;
			struVodPara.struEndTime.dwMonth = (uint)endDate.Month;
			struVodPara.struEndTime.dwDay = (uint)endDate.Day;
			struVodPara.struEndTime.dwHour = (uint)endDate.Hour;
			struVodPara.struEndTime.dwMinute = (uint)endDate.Minute;
			struVodPara.struEndTime.dwSecond = (uint)endDate.Second;

			// Playback by time
			int playHandle = CHCNetSDK.NET_DVR_PlayBackByTime_V40(NvrUserSession.UserSessionState.UserId, ref struVodPara);

			if (playHandle == -1) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackByTime_V40 failed");
			}

			uint unused = 0;
			if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(playHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 10, IntPtr.Zero, ref unused)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_PlayBackControl_V40: NET_DVR_PLAYSTART failed");
			}

			debugInfo("NET_DVR_RealPlay_V40 succ!");
			return playHandle;
		}

		private void stopPlayBackByTime(int playHandle)
		{
			if (playHandle < 0) {
				throw new NvrBadLogicException("Internal CS_NVRController exception", new ArgumentException("playHandle should be >0", "playHandle"));
			}

			//Stop playback
			if (!CHCNetSDK.NET_DVR_StopPlayBack(playHandle)) {
				throw new NvrSdkException(CHCNetSDK.NET_DVR_GetLastError(), "NET_DVR_StopPlayBack failed");
			}
		}

		private void checkUserSessionValid()
		{
			if (!NvrUserSession.IsSessionValid()) {
				throw new NvrBadLogicException("NvrUserSession is not valid");
			}
		}

		private void debugInfo(string msg)
		{
			if (isDebugEnabled_) {
				Console.WriteLine("[Debug] NvrSettings: " + msg);
			}
		}

		public static implicit operator NvrRecordLabels(NvrSettings v)
		{
			throw new NotImplementedException();
		}

		#endregion PrivateMethods

	}
	}
