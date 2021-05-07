using Dyme.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void btnStartService_Clicked(object sender, EventArgs e)
		{
			var serviceManager = SimpleServiceManager.Start("MySimpleService");
		}

		private void btnStopService_Clicked(object sender, EventArgs e)
		{

		}

		private void btnNotify_Clicked(object sender, EventArgs e)
		{

		}
	}
}
