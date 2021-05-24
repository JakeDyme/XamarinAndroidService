using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SampleApp2
{

	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			// Listen for commands comming from the service...
			MessagingCenter.Subscribe<object, string>(this, "START PLAYER BROADCAST", (s, arg) =>
			{
				lblPlayerStatus.Text = "Player Started";
				lblPlayerStatus.TextColor = Color.YellowGreen;
			});

			MessagingCenter.Subscribe<object, string>(this, "STOP PLAYER BROADCAST", (s, arg) =>
			{ 	
				lblPlayerStatus.Text = "Player Stopped";
				lblPlayerStatus.TextColor = Color.Salmon;
			});

			InitializeComponent();
	}

	private void btnStartService_Clicked(object sender, EventArgs e)
	{
		// Maybe you want to start the service from inside the app...
		App.StartBackgroundService();
		// Might be better though to start it from the "App.xaml.cs" file (I've added some methods there to demonstrate).
	}

	private void btnStopService_Clicked(object sender, EventArgs e)
	{
		// Maybe you want to stop the service from inside the app...
		App.BackgroundService.StopService(true);
		// Look into the "App.xaml.cs" file to see other methods for stopping and starting your service.
	}


}
}
