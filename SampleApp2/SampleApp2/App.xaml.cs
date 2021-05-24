using Dyme.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp2
{
	public partial class App : Application
	{
		public static SimpleServiceManager BackgroundService { get; private set;}

		public App()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{
			// You may want to stop the service so that the app can take over.
			// This is a good idea to prevent file access violations.
			if (BackgroundService != null) BackgroundService.StopService(true);
			BackgroundService = null;
		}

		protected override void OnSleep()
		{
			// Start the service to continue doing things in a background process. 
			// You can use your custom service implementation to start those processes.
			StartBackgroundService();
			// ...Note: Android does not allow some features (like microphone) to run in the service and background simultaneously.
		}

		protected override void OnResume()
		{
			// You may want to stop the service so that the app can take over.
			// This is a good idea to prevent file access violations.
			if (BackgroundService != null) BackgroundService.StopService(true);
			BackgroundService = null;
		}

		public static async void StartBackgroundService()
		{
			// Create a service with options
			var options = new SimpleServiceOptions();

			// These actions will appear as buttons in the notification.
			// Pressing the buttons will raise the "OnExecuteAction" event your ISimpleService implementation.
			options.ActionNamesAndTitles = new Dictionary<string, string>()
			{
				{ "PlayAction", "▶" },
				{ "StopAction", "■" },
				{ "ExitAction", "Exit" }
			};
			// ...The first field of each action is the "actionName" (the value passed to the "OnExecuteAction" action in the ISimpleService implementation)
			// ...The second field of each action is the label that will be displayed on the notification button.

			// Note that I'm passing in my ISimpleService implementation ("MyService")...
			App.BackgroundService = await SimpleServiceManager.Start<MyService>("Music Player", "(Expand the notification to see the action buttons)", options);
		}
	}
}
