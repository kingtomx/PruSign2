using System;
namespace PruSign
{
	public class StartLongRunningTaskMessage { }

	public class StopLongRunningTaskMessage { }

	public class TickedMessage
	{
		public string Message { get; set; }
	}
}
