using Dyme.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleApp2
{
	public class MyService : ISimpleService
	{
		DateTime _startTime;
		private bool _isPlaying;

		#region IMPLEMENTATION METHODS

		public void OnExecuteAction(string actionName, IList<string> args)
		{
			switch (actionName)
			{
				case "PlayAction":
					Play();
					break;
				case "StopAction":
					Stop();
					break;
				case "ExitAction":
					Exit();
					break;
			}
		}

		public void OnStart(SimpleServiceCore core)
		{
			//...you can grab the service core here, 
			// but you will need to reference the Mono.Android lib in order to use it.
		}

		public void OnStop(IList<string> args)
		{
			//...do stuff when the service exits
		}

		public void OnTapNotification()
		{
			//...do stuff when the user taps the notification
		}

		#endregion

		#region SERVICE METHODS

		private void Exit()
		{
			_isPlaying = false;
			// to shut down the service you can broadcast the special message defined in SimpleServiceCore.CALL_STOP_SERVICE
			MessagingCenter.Send<object, string>(this, SimpleServiceCore.CALL_STOP_SERVICE, null);
		}

		private void Stop()
		{
			_isPlaying = false;
			MessagingCenter.Send<object, string>(this, "STOP PLAYER BROADCAST", "some extra service data");
		}

		private void Play()
		{
			_startTime = DateTime.Now;
			MessagingCenter.Send<object, string>(this, "START PLAYER BROADCAST", "some extra service data");
			_isPlaying = true;
			Task.Run(async () =>
			{
				while (GetIsPlaying())
				{
					PlayerTick();
					await Task.Delay(1000);
				}
			});
		}

		private bool GetIsPlaying()
		{
			return _isPlaying;
		}

		private void PlayerTick()
		{
			var t = DateTime.Now - _startTime;
			var newNotificationText = $"Playing: {t.Hours.ToString("00")}:{t.Minutes.ToString("00")}:{t.Seconds.ToString("00")}";
			// If you want to avoid refencing the Mono.Android library to update the notification text, 
			// you can broadcast the special message defined in SimpleServiceCore.CALL_UPDATE_NOTI_TEXT"
			MessagingCenter.Send<object, string>(this, SimpleServiceCore.CALL_UPDATE_NOTI_TEXT, newNotificationText);
		}
		#endregion

	}
}
