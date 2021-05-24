using Android.App;
using System.Collections.Generic;

namespace Dyme.Services
{
	public interface ISimpleService
	{
		void OnStart(SimpleServiceCore core);
		void OnExecuteAction(string actionName, IList<string> args);
		void OnTapNotification();
		void OnStop(IList<string> args);
		//void OnSwipeAwayNotification();
	}

}