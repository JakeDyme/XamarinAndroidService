using Dyme.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DymeAndroidService
{
	class SimpleService : ISimpleService
	{
		public List<Action> actions {get; set; } = new List<Action>();
		public void OnExecuteAction(string actionName, IList<string> args)
		{
			
		}

		public void OnStart(SimpleServiceCore core)
		{
			
		}

		public void OnStop(IList<string> args)
		{
			
		}

		public void OnTapNotification()
		{
			
		}
	}
}
