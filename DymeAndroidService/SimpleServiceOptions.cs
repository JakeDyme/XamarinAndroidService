using Android.App;
using System;
using System.Collections.Generic;

namespace Dyme.Services
{
	public class SimpleServiceAdvancedOptions
	{
		public string ChannelName { get; set; }
		public string ChannelId { get; set; }
		public string ChannelDescription { get; set; }
		public string ServiceName { get; set; }
		public string PackageName { get; set; }
		public bool Silent { get; set; } = false;
		public SimpleServiceAdvancedOptions(string name)
		{
			var packageName = PackageName ?? Application.Context.PackageName;
			ChannelName = $"Channel_{name}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelId = $"ChannelId_{name}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelDescription = packageName;
			ServiceName = $"Service_{name}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			PackageName = packageName;
		}

		public SimpleServiceAdvancedOptions()
		{
			var packageName = PackageName ?? Application.Context.PackageName;
			ChannelName = $"Channel_{packageName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelId = $"ChannelId_{packageName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			ChannelDescription = packageName;
			ServiceName = $"Service_{packageName}_{Guid.NewGuid().ToString().Substring(0, 4)}";
			PackageName = packageName;
		}
	}

	public class SimpleServiceOptions
	{
		public string NotificationTitle { get; set; }
		public string NotificationText { get; set; }
		public string OnNotificationClickActionName { get; set; }
		public string OnNotificationTapEventName { get; set; }
		public Dictionary<string, string> ActionNamesAndTitles { get; set; }
		public SimpleServiceAdvancedOptions Advanced { get; set; }

		public SimpleServiceOptions()
		{
			NotificationTitle = Application.Context.PackageName;
			ActionNamesAndTitles = new Dictionary<string, string>();
			Advanced = new SimpleServiceAdvancedOptions();
		}
		public SimpleServiceOptions(string name)
		{
			NotificationTitle = name;
			ActionNamesAndTitles = new Dictionary<string, string>();
			Advanced = new SimpleServiceAdvancedOptions(name);
		}

		public void AddActionButton(string key, string friendlyTitle = null)
		{
			ActionNamesAndTitles.Add(key, friendlyTitle ?? key);
		}
	}

}