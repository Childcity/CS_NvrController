using CS_NVRController.Hickvision;
using CS_NVRController.Hickvision.NvrController;
using CS_NVRController.Hickvision.NvrExceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS_NVRController.BLL {

	public class UserSessionService {

		// Singleton instance
		private static UserSessionService userSessionService_ = null;

		private UserSessionService() { }

		public static UserSessionService GetInstance()
		{
			if (userSessionService_ == null) {
				userSessionService_ = new UserSessionService();
			}
			return userSessionService_;
		}

		#region PublicProperties

		public NvrUserSession NvrUserSession { get; private set; } = null;

		public List<string> CameraChannels
		{
			get {
				var camChannals = new List<string>();
				NvrUserSession.IpChannels.ForEach(x => {
					camChannals.Add(x.Item1 + " " + x.Item2 + " [" + x.Item3 + "]");
				});
				return camChannals;
			}
		}

		public int CameraSelectedChannel
		{
			get { return NvrUserSession.SelectedChannelId; }
			set { NvrUserSession.SelectedChannelId = value; }
		}

		#endregion

		#region PublicMethods

		public async Task LoginAsync(NvrSessionInfo sessionInfo)
		{
			string sdkLogDir = System.Reflection.Assembly.GetEntryAssembly().Location + @"_NvrSdkLogs\";
			NvrUserSession = new NvrUserSession(sessionInfo, sdkLogDir, true);

			await Task.Factory.StartNew(() => {
				try {
					NvrUserSession.StartSession();
				} catch (NvrSdkException ex) {
					throw new SystemException($"StartSession failed: NvrException Code[{ex.SdkErrorCode}]: {ex.Message}", ex);
				} catch (Exception ex) {
					throw new SystemException("StartSession failed", ex);
				}
			}, TaskCreationOptions.AttachedToParent);
		}

		public void Logout()
		{
			try {
				NvrUserSession?.StopSession();
			} catch (Exception e) {
				Console.WriteLine(e.Message + "\n" + e.StackTrace);
			} finally {
				NvrUserSession?.Dispose();
				NvrUserSession = null;
			}
		}

		#endregion

	}
}