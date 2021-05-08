using Android.App;
using System;
using System.Collections.Generic;

namespace Dyme.Services
{
	public class SimpleServiceAdvancedOptions
	{
		public string ChannelName { get; set; }
		public string ChannelId { get; set; }
		public string ChannelDescription { get; set; }
		public string ServiceName { get; set; }
		public string PackageName { get; set; }
		public bool Silent { get; set; } = false;
		public SimpleServiceAdvancedOptions(string name)
		{
			var packageName = PackageName ?? Application.Context.PackageName;
			ChannelName = $"Channel_{name}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelId = $"ChannelId_{name}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelDescription = packageName;
			ServiceName = $"Service_{name}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			PackageName = packageName;
		}

		public SimpleServiceAdvancedOptions()
		{
			var packageName = PackageName ?? Application.Context.PackageName;
			ChannelName = $"Channel_{packageName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelId = $"ChannelId_{packageName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelDescription = packageName;
			ServiceName = $"Service_{packageName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			PackageName = packageName;
		}
	}

	public class SimpleServiceOptions
	{
		public string NotificationTitle { get; set; }
		public string NotificationText { get; set; }
		public IconEnum NotificationIcon { get; set; } = IconEnum.IcDialogInfo;

		public Dictionary<string, string> ActionNamesAndTitles { get; set; }
		public SimpleServiceAdvancedOptions Advanced { get; set; }

		public SimpleServiceOptions()
		{
			NotificationTitle = Application.Context.PackageName;
			ActionNamesAndTitles = new Dictionary<string, string>();
			Advanced = new SimpleServiceAdvancedOptions();
		}
		public SimpleServiceOptions(string name)
		{
			NotificationTitle = name;
			ActionNamesAndTitles = new Dictionary<string, string>();
			Advanced = new SimpleServiceAdvancedOptions(name);
		}

		public void AddActionButton(string key, string friendlyTitle = null)
		{
			ActionNamesAndTitles.Add(key, friendlyTitle ?? key);
		}
	}

	public enum IconEnum
	{
		AlertDarkFrame = 17301504,
		PresenceAudioBusy = 17301680,
		PresenceAudioOnline = 17301681,
		PresenceAway = 17301607,
		PresenceBusy = 17301608,
		PresenceInvisible = 17301609,
		PresenceOffline = 17301610,
		PresenceOnline = 17301611,
		PresenceVideoAway = 17301676,
		PresenceAudioAway = 17301679,
		PresenceVideoBusy = 17301677,
		ProgressHorizontal = 17301612,
		ProgressIndeterminateHorizontal = 17301613,
		RadiobuttonOffBackground = 17301614,
		RadiobuttonOnBackground = 17301615,
		ScreenBackgroundDark = 17301656,
		ScreenBackgroundDarkTransparent = 17301673,
		ScreenBackgroundLight = 17301657,
		ScreenBackgroundLightTransparent = 17301674,
		PresenceVideoOnline = 17301678,
		SpinnerBackground = 17301616,
		PictureFrame = 17301606,
		MenuFullFrame = 17301604,
		IcMenuSortAlphabetically = 17301660,
		IcMenuSortBySize = 17301661,
		IcMenuToday = 17301588,
		IcMenuUpload = 17301589,
		IcMenuUploadYouTube = 17301590,
		IcMenuView = 17301591,
		IcMenuWeek = 17301592,
		IcMenuZoom = 17301593,
		MenuitemBackground = 17301605,
		IcNotificationClearAll = 17301594,
		IcPartialSecure = 17301596,
		IcPopupDiskFull = 17301597,
		IcPopupReminder = 17301598,
		IcPopupSync = 17301599,
		IcSearchCategoryDefault = 17301600,
		IcSecure = 17301601,
		ListSelectorBackground = 17301602,
		MenuFrame = 17301603,
		IcNotificationOverlay = 17301595,
		IcMenuSlideshow = 17301587,
		SpinnerDropDownBackground = 17301617,
		StarBigOn = 17301618,
		StatSysUploadDone = 17301641,
		StatSysVpPhoneCall = 17301671,
		StatSysVpPhoneCallOnHold = 17301672,
		StatSysWarning = 17301642,
		StatusBarItemAppBackground = 17301643,
		StatusBarItemBackground = 17301644,
		SymActionCall = 17301645,
		SymActionChat = 17301646,
		StatSysUpload = 17301640,
		SymActionEmail = 17301647,
		SymCallMissed = 17301649,
		SymCallOutgoing = 17301650,
		SymContactCard = 17301652,
		SymDefAppIcon = 17301651,
		TitleBar = 17301653,
		TitleBarTall = 17301670,
		ToastFrame = 17301654,
		ZoomPlate = 17301655,
		SymCallIncoming = 17301648,
		StarBigOff = 17301619,
		StatSysSpeakerphone = 17301639,
		StatSysPhoneCallForward = 17301637,
		StarOff = 17301621,
		StarOn = 17301620,
		StatNotifyCallMute = 17301622,
		StatNotifyChat = 17301623,
		StatNotifyError = 17301624,
		StatNotifyMissedCall = 17301631,
		StatNotifyMore = 17301625,
		StatNotifySdCard = 17301626,
		StatSysPhoneCallOnHold = 17301638,
		StatNotifySdcardPrepare = 17301675,
		StatNotifySync = 17301628,
		StatNotifySyncNoAnim = 17301629,
		StatNotifyVoicemail = 17301630,
		StatSysDataBluetooth = 17301632,
		StatSysDownload = 17301633,
		StatSysDownloadDone = 17301634,
		StatSysHeadset = 17301635,
		StatSysPhoneCall = 17301636,
		StatNotifySdCardUsb = 17301627,
		IcMenuSetAs = 17301585,
		IcMenuShare = 17301586,
		IcInputGet = 17301549,
		DividerHorizontalBright = 17301522,
		DividerHorizontalDark = 17301524,
		DividerHorizontalDimDark = 17301525,
		DividerHorizontalTextfield = 17301523,
		EditText = 17301526,
		EditBoxBackground = 17301528,
		EditBoxBackgroundNormal = 17301529,
		EditBoxDropDownDarkFrame = 17301530,
		DialogHoloLightFrame = 17301683,
		EditBoxDropDownLightFrame = 17301531,
		IcButtonSpeakNow = 17301668,
		IcDelete = 17301533,
		IcDialogAlert = 17301543,
		IcDialogDialer = 17301544,
		IcDialogEmail = 17301545,
		IcDialogInfo = 17301659,
		IcDialogMap = 17301546,
		IcInputAdd = 17301547,
		GalleryThumb = 17301532,
		IcInputDelete = 17301548,
		DialogHoloDarkFrame = 17301682,
		DarkHeader = 17301669,
		AlertLightFrame = 17301505,
		ArrowDownFloat = 17301506,
		ArrowUpFloat = 17301507,
		BottomBar = 17301658,
		ButtonDefault = 17301508,
		ButtonDefaultSmall = 17301509,
		ButtonDialog = 17301527,
		ButtonDropDown = 17301510,
		DialogFrame = 17301521,
		ButtonMinus = 17301511,
		ButtonRadio = 17301513,
		ButtonStar = 17301514,
		ButtonStarBigOff = 17301515,
		ButtonStarBigOn = 17301516,
		ButtonOnoffIndicatorOff = 17301518,
		ButtonOnoffIndicatorOn = 17301517,
		CheckboxOffBackground = 17301519,
		CheckboxOnBackground = 17301520,
		ButtonPlus = 17301512,
		IcMenuSend = 17301584,
		IcLockIdleAlarm = 17301550,
		IcLockIdleCharging = 17301534,
		IcMenuEdit = 17301566,
		IcMenuGallery = 17301567,
		IcMenuHelp = 17301568,
		IcMenuInfoDetails = 17301569,
		IcMenuManage = 17301570,
		IcMenuMapmode = 17301571,
		IcMenuMonth = 17301572,
		IcMenuMore = 17301573,
		IcMenuDirections = 17301565,
		IcMenuMyCalendar = 17301574,
		IcMenuMyPlaces = 17301576,
		IcMenuPreferences = 17301577,
		IcMenuRecentHistory = 17301578,
		IcMenuReportImage = 17301579,
		IcMenuRevert = 17301580,
		IcMenuRotate = 17301581,
		IcMenuSave = 17301582,
		IcMenuSearch = 17301583,
		IcMenuMyLocation = 17301575,
		IcMenuDelete = 17301564,
		IcMenuDay = 17301563,
		IcMenuCrop = 17301562,
		IcLockIdleLock = 17301535,
		IcLockIdleLowBattery = 17301536,
		IcLockLock = 17301551,
		IcLockPowerOff = 17301552,
		IcLockSilentMode = 17301553,
		IcLockSilentModeOff = 17301554,
		IcMediaFf = 17301537,
		IcMediaNext = 17301538,
		IcMediaPause = 17301539,
		IcMediaPlay = 17301540,
		IcMediaPrevious = 17301541,
		IcMediaRew = 17301542,
		IcMenuAdd = 17301555,
		IcMenuAgenda = 17301556,
		IcMenuAlwaysLandscapePortrait = 17301557,
		IcMenuCall = 17301558,
		IcMenuCamera = 17301559,
		IcMenuCloseClearCancel = 17301560,
		IcMenuCompass = 17301561
	}
}