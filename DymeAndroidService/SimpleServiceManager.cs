using Android.Content;
using DymeAndroidService;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Dyme.Services
{
	public class SimpleServiceManager
	{
		string _packageName;
		private DymeServiceConnection _newServiceConnection = new DymeServiceConnection();
		private SimpleServiceCore _serviceInstance;

		public static async Task<SimpleServiceManager> Start(string name, string content, SimpleServiceOptions options = null)
		{
			options = options ?? new SimpleServiceOptions();
			options.NotificationTitle = name;
			options.NotificationText = content;
			return await Start<SimpleService>(name, content, options);
		}

		public static async Task<SimpleServiceManager> Start<T>(string name, string content, SimpleServiceOptions options = null) where T : ISimpleService
		{
			options = options ?? new SimpleServiceOptions();
			options.NotificationTitle = name;
			options.NotificationText = content;
			var intent = new Intent(Android.App.Application.Context, typeof(SimpleServiceCore));
			intent.SetPackage(options.Advanced.PackageName);
			intent.PutExtra(SimpleServiceCore.EXTRAS_SERVICE_OPTIONS, JsonConvert.SerializeObject(options));
			intent.PutExtra(SimpleServiceCore.EXTRAS_CLASS_NAME, typeof(T).FullName);
			var newServiceManager = new SimpleServiceManager(options.Advanced.PackageName);
			await newServiceManager.ExecuteIntent(intent);
			return newServiceManager;
		}

		public SimpleServiceManager(string packageName)
		{
			_packageName = packageName;
		}

		public void StopService(bool removeNotification)
		{
			_serviceInstance?.StopForeground(removeNotification);
		}

		public async Task StopExistingService(string packageName)
		{
			// Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(SimpleServiceCore));
			intent.SetPackage(packageName);
			intent.SetAction(SimpleServiceCore.CALL_STOP_SERVICE);
			await ExecuteIntent(intent);
		}

		public async Task ExecuteAction(string actionName, string[] args = null)
		{
			// Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(SimpleServiceCore));
			intent.SetPackage(_packageName);
			intent.SetAction(actionName);
			if (args != null) intent.PutStringArrayListExtra(SimpleServiceCore.EXTRAS_ARGS, args);
			await ExecuteIntent(intent);
		}

		public async Task UpdateNotificationText(string notificationText)
		{
			//Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(SimpleServiceCore));
			intent.SetPackage(_packageName);
			intent.SetAction(SimpleServiceCore.CALL_UPDATE_NOTI_TEXT);
			intent.PutExtra(SimpleServiceCore.EXTRAS_NOTI_TEXT, notificationText);
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
				Android.App.Application.Context.BindService(intent, _newServiceConnection, Bind.AutoCreate);
				_serviceInstance = _newServiceConnection.serviceInstance;
			});
		}
	}

}