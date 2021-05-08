using Android;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using DymeAndroidService;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Dyme.Services
{
	public class ServiceManager
	{
		string _packageName;
		private DymeServiceConnection _newServiceConnection = new DymeServiceConnection();
		private ServiceCore _serviceInstance;
		//private T _userService;
		//private Dictionary<string, Action> Actions = new Dictionary<string, Action>();

		public static async Task<ServiceManager> Start(string name, string content, SimpleServiceOptions options = null)
		{
			options = options ?? new SimpleServiceOptions(name, content);
			return await Start<SimpleService>(options);
		}

		public static async Task<ServiceManager> Start<T>(SimpleServiceOptions options = null) where T : ISimpleService
		{
			options = options ?? new SimpleServiceOptions();
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(options.Advanced.PackageName);
			intent.PutExtra(ServiceCore.EXTRAS_SERVICE_OPTIONS, JsonConvert.SerializeObject(options));
			intent.PutExtra(ServiceCore.EXTRAS_CLASS_NAME, typeof(T).FullName);
			var newServiceManager = new ServiceManager(options.Advanced.PackageName);
			await newServiceManager.ExecuteIntent(intent);
			return newServiceManager;
		}

		public ServiceManager(string packageName)
		{
			_packageName = packageName;
		}

		public void StopService(bool removeNotification)
		{
			_serviceInstance?.StopForeground(removeNotification);
			//_serviceInstance?.StopSelf();
		}

		public async Task StopExistingService(string packageName)
		{
			// Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(packageName);
			intent.SetAction(ServiceCore.ACTIONS_STOP_SERVICE);
			await ExecuteIntent(intent);
		}

		public async Task ExecuteAction(string actionName, string[] args = null)
		{
			// Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(_packageName);
			intent.SetAction(actionName);
			if (args != null) intent.PutStringArrayListExtra(ServiceCore.EXTRAS_ARGS, args);
			await ExecuteIntent(intent);
		}

		public async Task UpdateNotificationText(string notificationText)
		{
			//Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(_packageName);
			intent.SetAction(ServiceCore.ACTIONS_UPDATE_NOTI_TEXT);
			intent.PutExtra(ServiceCore.EXTRAS_NOTI_TEXT, notificationText);
			//_serviceInstance.NotificationBuilder.SetContentText(CharSequence.ArrayFromStringArray(new string[] { message })[0]);// = new Notification.Builder(Android.App.Application.Context)
			//_serviceInstance.StartForeground(ServiceCore.NOTIFICATION_ID, _core.NotificationBuilder.Build());
			await ExecuteIntent(intent);
		}

		private async Task ExecuteIntent(Intent intent)
		{
			DymeServiceConnection.ServiceIsActive = false;
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
			{
				
				Android.App.Application.Context.StartForegroundService(intent);
				Android.App.Application.Context.BindService(intent, _newServiceConnection, Bind.AutoCreate);
				await WaitForServiceToBind(intent);
			}
			else
			{
				Android.App.Application.Context.StartService(intent);
				Android.App.Application.Context.BindService(intent, _newServiceConnection, Bind.AutoCreate);
				await WaitForServiceToBind(intent);
			}
		}

		private async Task WaitForServiceToBind(Intent intent)
		{
			await Task.Run(async () =>
			{
				var startTime = DateTime.Now;
				while ((DateTime.Now - startTime).TotalSeconds < 10 && !DymeServiceConnection.ServiceIsActive)
				{
					await Task.Delay(500);
				}
				//Android.App.Application.Context.StartForegroundService(intent);
				Android.App.Application.Context.BindService(intent, _newServiceConnection, Bind.AutoCreate);
				_serviceInstance = _newServiceConnection.serviceInstance;
			});
		}
	}

}