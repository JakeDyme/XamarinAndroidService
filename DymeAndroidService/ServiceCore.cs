#region Assembly Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065
// C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\ReferenceAssemblies\Microsoft\Framework\MonoAndroid\v9.0\Mono.Android.dll
#endregion

using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Java.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Android.Util;

namespace Dyme.Services
{

	[Service(Name = "com.dyme.ServiceCore")]
	public class ServiceCore : Service
	{
		public const string ACTIONS_NOTI_TAP = "JD_NOTI_TAP";
		public const string ACTIONS_NOTI_SWIPE = "JD_NOTI_SWIPE";
		public static string ACTIONS_STOP_SERVICE = "JD_STOP_SERVICE";
		public static string ACTIONS_UPDATE_NOTI_TEXT = "JD_UPDATE_NOTI_TEXT";

		public static string MSG_STOP_SERVICE = "JD_STOP_SERVICE";
		public static string MSG_TRIGGER_ACTION = "JD_ACCEPT_ACTION";

		internal const string EXTRAS_SERVICE_OPTIONS = "JD_SERVICE_OPTIONS";
		internal const string EXTRAS_CLASS_NAME = "JD_CLASS_NAME";
		internal const string EXTRAS_NOTI_TEXT = "JD_NOTI_TEXT";
		internal const string EXTRAS_ARGS = "JD_ARGS";

		public const int NOTIFICATION_ID = 1908213;

		public Notification NotificationInstance { get; set; }
		public Notification.Builder NotificationBuilder { get; set; }
		public NotificationChannel NotificationChannel { get; set; }
		public NotificationManager NotificationManager { get; set; }
		private SimpleServiceOptions Options;
		private ISimpleService _instance { get; set; }
		private DymeBinder binder;

		public ServiceCore()
		{
			MessagingCenter.Subscribe<object, string>(this, MSG_STOP_SERVICE, (s, arg) =>
			{
				if (_instance != null) _instance.OnStop(new List<string>());
				this.StopSelf();
			});

			MessagingCenter.Subscribe<object, string>(this, MSG_TRIGGER_ACTION, (s, arg) =>
			{
				if (_instance == null) return;
				if (NotificationInstance == null) return;
				var str = CharSequence.ArrayFromStringArray(new string[] { arg })[0];
				NotificationInstance.Actions.First(a => a.Title == str).ActionIntent.Send();
			});

		}

		public void StopIt(bool removeNotification)
		{
			//StopForeground(removeNotification);
		}

		[return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			// Handle incomming action requests...
			if (intent.Action != null)
			{
				if (intent.Action == ACTIONS_UPDATE_NOTI_TEXT)
				{
					string notiText = intent.GetStringExtra(EXTRAS_NOTI_TEXT);
					UpdateNotificationText(notiText);
				}
				if (intent.Action == ACTIONS_STOP_SERVICE)
				{
					StopForeground(true);
					return StartCommandResult.RedeliverIntent;
				}
				if (intent.Action == ACTIONS_NOTI_TAP)
				{
					_instance.OnTapNotification();
					return StartCommandResult.RedeliverIntent;
				}
				if (intent.Action == ACTIONS_NOTI_SWIPE)
				{
					//_instance.OnSwipeAwayNotification();
					return StartCommandResult.RedeliverIntent;
				}
				var args = intent.GetStringArrayListExtra(EXTRAS_ARGS);
				// Forward the action on to the user's custom service class...
				_instance.OnExecuteAction(intent.Action, args);
				return StartCommandResult.RedeliverIntent;
			}
			//...else if action is null, then its the first time load...

			// Instantiate and persist the user's custom service class...
			var className = intent.GetStringExtra(EXTRAS_CLASS_NAME);
			_instance = CreateClassInstance(className) as ISimpleService;

			// Create the persisted notification...
			var serializedOptions = intent.GetStringExtra(EXTRAS_SERVICE_OPTIONS);
			Options = JsonConvert.DeserializeObject<SimpleServiceOptions>(serializedOptions);
			BuildNotificationChannel(Options);
			NotificationInstance = MakeNotificationItem(Options, NOTIFICATION_ID);
			// Enlist this instance of the service as a foreground service...			
			
			
			StartForeground(NOTIFICATION_ID, NotificationInstance);
			//OnStart += _instance.OnStart(this);
			_instance.OnStart(this);

			return StartCommandResult.RedeliverIntent;
		}

		private void UpdateNotificationText(string notiText)
		{
			NotificationInstance.TickerText = CharSequence.ArrayFromStringArray(new string[] { notiText })[0];
		}

		private static object CreateClassInstance(string className)
		{
			var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetTypes().Any(t => t.FullName == className)).ToList();
			if (allAssemblies.Count < 1) throw new Exception("Unable to find Assembly with class: " + className);
			if (allAssemblies.Count > 1) throw new Exception($"Multiple assemblies exist with class: {className}. Try renaming the class to something unique");
			var assembly = allAssemblies.Single(a => a.GetTypes().Any(t => t.FullName == className));
			var type = assembly.GetTypes().Single(t => t.FullName == className);
			return Activator.CreateInstance(type);
		}

		private Notification MakeNotificationItem(SimpleServiceOptions options, int notificationId)
		{
			NotificationBuilder = new Notification.Builder(Android.App.Application.Context)
					.SetContentTitle(options.NotificationTitle ?? Android.App.Application.Context.ApplicationInfo.Name)
					.SetContentText(options.NotificationText ?? "")
					.SetSmallIcon(Resource.Drawable.IcDialogInfo)
					.SetOngoing(true)
					//.SetAutoCancel(true)
					//.SetPriority(0)
					.SetChannelId(options.Advanced.ChannelId);

			NotificationBuilder.SetSound(null,null);
			NotificationBuilder.SetContentIntent(BuildIntentToShowMainActivity(options));
			//notificationBuilder.SetFullScreenIntent(BuildIntentToShowMainActivity(), false);
			NotificationBuilder.SetDeleteIntent(BuildIntentToRemoveNotification(options, notificationId));
			// Add custom action buttons...
			foreach (var action in options.ActionNamesAndTitles)
			{
				var intent = BuildAction(action.Key, action.Value);
				NotificationBuilder.AddAction(intent);
			}
			
			return NotificationBuilder.Build();
		}

		public void BuildNotificationChannel(SimpleServiceOptions options)
		{
			NotificationChannel = new NotificationChannel(options.Advanced.ChannelId, options.Advanced.ChannelName, NotificationImportance.Low)
			{
				Description = options.Advanced.ChannelDescription
			};
			NotificationManager = (NotificationManager)Android.App.Application.Context.GetSystemService(Android.App.Application.NotificationService);
			NotificationManager.CreateNotificationChannel(NotificationChannel);
		}
		
		public void DeleteNotificationChannel(SimpleServiceOptions options)
		{
			NotificationManager = (NotificationManager)Android.App.Application.Context.GetSystemService(Android.App.Application.NotificationService);
			NotificationManager.DeleteNotificationChannel(options.Advanced.ChannelId);
		}

		PendingIntent BuildIntentToShowMainActivity(SimpleServiceOptions options)
		{
			var intent = new Intent(Android.App.Application.Context, GetType());
			intent.SetAction(ACTIONS_NOTI_TAP);
			intent.SetPackage(options.Advanced.PackageName);
			var pendingIntent = PendingIntent.GetService(Android.App.Application.Context, 0, intent, 0);
			return pendingIntent;
		}

		//private PendingIntent createOnDismissedIntent(Context context, int notificationId)
		//{
		//	Intent intent = new Intent(context, NotificationDismissedReceiver.class);
		//	intent.putExtra($"{_packageName}.notificationId", notificationId);
		//	PendingIntent pendingIntent = PendingIntent.GetBroadcast(context.getApplicationContext(), notificationId, intent, 0);
		//	return pendingIntent;
		//}

	PendingIntent BuildIntentToRemoveNotification(SimpleServiceOptions options, int notificationId)
		{
			var intent = new Intent(Android.App.Application.Context, GetType());
			intent.SetPackage(options.Advanced.PackageName);
			intent.SetAction(ACTIONS_STOP_SERVICE);
			//notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
			var pendingIntent = PendingIntent.GetService(Android.App.Application.Context, 0, intent, 0);//PendingIntent.GetBroadcast(Android.App.Application.Context, notificationId, intent, 0);
			return pendingIntent;
		}

		Notification.Action BuildAction(string actionName, string actionTitle)
		{
			var intent = new Intent(Android.App.Application.Context, GetType());
			intent.SetAction(actionName);
			var pendingIntent = PendingIntent.GetService(Android.App.Application.Context, 0, intent, 0);

			var builder = new Notification.Action.Builder(
				Resource.Drawable.IcMediaPause
				, actionTitle
				, pendingIntent);
			return builder.Build();
		}

		public override IBinder OnBind(Intent intent)
		{
			binder = new DymeBinder(this);
			return binder;
		}

		/// <summary>
		/// Builds a Notification.Action that will instruct the service to restart the timer.
		/// </summary>
		/// <returns>The restart timer action.</returns>
		//Notification.Action BuildRestartTimerAction()
		//{
		//	var restartTimerIntent = new Intent(this, GetType());
		//	restartTimerIntent.SetAction(ActionStart);
		//	var restartTimerPendingIntent = PendingIntent.GetService(Android.App.Application.Context, 0, restartTimerIntent, 0);

		//	var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPlay,
		//																		"Start service",
		//																		restartTimerPendingIntent);
		//	return builder.Build();
		//}

		/// <summary>
		/// Builds the Notification.Action that will allow the user to stop the service via the
		/// notification in the status bar
		/// </summary>
		/// <returns>The stop service action.</returns>
		//Notification.Action BuildStopServiceAction()
		//{
		//	var stopServiceIntent = new Intent(Android.App.Application.Context, GetType());
		//	stopServiceIntent.SetAction(ActionStop);
		//	var stopServicePendingIntent = PendingIntent.GetService(Android.App.Application.Context, 0, stopServiceIntent, 0);

		//	var builder = new Notification.Action.Builder(
		//		Resource.Drawable.IcMediaPause
		//		, "Stop service"
		//		, stopServicePendingIntent);
		//	return builder.Build();
		//}

	}

	public class DymeBinder: Binder
	{
		public ServiceCore Service { get; private set; }
		public DymeBinder(ServiceCore service)
		{
			this.Service = service;
		}
	}

	public class DymeServiceConnection : Java.Lang.Object, IServiceConnection
	{
		public ServiceCore serviceInstance;

		public void OnServiceConnected(ComponentName name, IBinder service)
		{
			serviceInstance = ((DymeBinder)service).Service;
		}

		public void OnServiceDisconnected(ComponentName name)
		{
			Console.WriteLine("OnServiceDisConnected() Method called");
		}

		//	static readonly string TAG = typeof(DymeServiceConnection).FullName;

		//	//Activity mainActivity;
		//	public bool IsConnected { get; private set; }
		//	public DymeBinder Binder { get; private set; }

		//	public DymeServiceConnection()
		//	{
		//		IsConnected = false;
		//		Binder = null;
		//		//mainActivity = activity;
		//	}

		//	public void OnServiceConnected(ComponentName name, IBinder service)
		//	{
		//		Binder = service as DymeBinder;
		//		IsConnected = this.Binder != null;

		//		string message = "onServiceConnected - ";
		//		Log.Debug(TAG, $"OnServiceConnected {name.ClassName}");

		//		if (IsConnected)
		//		{
		//			message = message + " bound to service " + name.ClassName;
		//			//mainActivity.UpdateUiForBoundService();
		//		}
		//		else
		//		{
		//			message = message + " not bound to service " + name.ClassName;
		//			//mainActivity.UpdateUiForUnboundService();
		//		}

		//		Log.Info(TAG, message);
		//		//mainActivity.timestampMessageTextView.Text = message;

		//	}

		//	public void OnServiceDisconnected(ComponentName name)
		//	{
		//		Log.Debug(TAG, $"OnServiceDisconnected {name.ClassName}");
		//		IsConnected = false;
		//		Binder = null;
		//		//mainActivity.UpdateUiForUnboundService();
		//	}

		//	//public string GetFormattedTimestamp()
		//	//{
		//	//	if (!IsConnected)
		//	//	{
		//	//		return null;
		//	//	}

		//	//	return Binder?.GetFormattedTimestamp();
		//	//}
		//}
	}
	}