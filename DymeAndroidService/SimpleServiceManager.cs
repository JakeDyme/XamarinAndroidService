using Android.App;
using Android.Content;
using DymeAndroidService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Dyme.Services
{
	public class SimpleServiceManager
	{
		string _packageName;
		private DymeServiceConnection _newServiceConnection = new DymeServiceConnection();
		private ServiceCore _serviceInstance;
		//private T _userService;
		//private Dictionary<string, Action> Actions = new Dictionary<string, Action>();

		public static SimpleServiceManager Start(string name, SimpleServiceOptions options = null)
		{
			options = options ?? new SimpleServiceOptions(name);
			// Shut down previous instance...

			//StopExistingService(options.PackageName);
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(options.Advanced.PackageName);
			intent.PutExtra(ServiceCore.EXTRAS_SERVICE_OPTIONS, JsonConvert.SerializeObject(options));
			intent.PutExtra(ServiceCore.EXTRAS_CLASS_NAME, typeof(SimpleService).FullName);
			var newServiceManager = new SimpleServiceManager(options.Advanced.PackageName);
			newServiceManager.ExecuteIntent(intent);
			return newServiceManager;
		}

		public static SimpleServiceManager Start<T>(SimpleServiceOptions options = null) where T : ISimpleService
		{
			options = options ?? new SimpleServiceOptions();
			// Shut down previous instance...
			
			//StopExistingService(options.PackageName);
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(options.Advanced.PackageName);
			intent.PutExtra(ServiceCore.EXTRAS_SERVICE_OPTIONS, JsonConvert.SerializeObject(options));
			intent.PutExtra(ServiceCore.EXTRAS_CLASS_NAME, typeof(T).FullName);
			var newServiceManager = new SimpleServiceManager(options.Advanced.PackageName);
			newServiceManager.ExecuteIntent(intent);
			return newServiceManager;
		}

		public SimpleServiceManager(string packageName)
		{
			_packageName = packageName;
		}

		public void StopService(bool removeNotification)
		{
			//StopExistingService(_packageName);
			_serviceInstance?.StopForeground(removeNotification);
		}

		public void StopExistingService(string packageName)
		{
			// Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(packageName);
			intent.SetAction(ServiceCore.ACTIONS_STOP_SERVICE);
			ExecuteIntent(intent);
		}

		public void ExecuteAction(string actionName, string[] args = null)
		{
			// Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(_packageName);
			intent.SetAction(actionName);
			if (args != null) intent.PutStringArrayListExtra(ServiceCore.EXTRAS_ARGS, args);
			ExecuteIntent(intent);
		}

		public void UpdateNotificationText(string notificationText)
		{
			// Wrap the user's action into an intent...
			var intent = new Intent(Android.App.Application.Context, typeof(ServiceCore));
			intent.SetPackage(_packageName);
			intent.SetAction(ServiceCore.ACTIONS_UPDATE_NOTI_TEXT);
			intent.PutExtra(ServiceCore.EXTRAS_NOTI_TEXT, notificationText);
			ExecuteIntent(intent);
		}

		private void ExecuteIntent(Intent intent)
		{
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
			{
				Android.App.Application.Context.StartForegroundService(intent);
				Android.App.Application.Context.BindService(intent, _newServiceConnection, Bind.AutoCreate);// .bindService(serviceIntent, mTestServiceConnection, Context.BIND_AUTO_CREATE);
				_serviceInstance = _newServiceConnection.serviceInstance;
			}
			else
			{
				Android.App.Application.Context.StartService(intent);
			}
		}

	}

}