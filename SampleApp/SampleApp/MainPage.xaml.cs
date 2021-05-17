using Dyme.Services;
using System;
using Xamarin.Forms;

namespace SampleApp
{
	public partial class MainPage : ContentPage
	{
		ServiceManager _simpleService;
		int _updateCount;

		public MainPage()
		{
			InitializeComponent();
		}

		private async void btnStartService_Clicked(object sender, EventArgs e)
		{
			_simpleService = await ServiceManager.Start("My Simple Service", "Hello world");
		}

		private void btnStopService_Clicked(object sender, EventArgs e)
		{
			_simpleService.StopService(removeNotification: true);
		}

		private async void btnNotify_Clicked(object sender, EventArgs e)
		{
			await _simpleService.UpdateNotificationText($"updated {++_updateCount} times");
		}
	}
}
