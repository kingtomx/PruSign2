using System;
using Foundation;
using MultipeerConnectivity;

namespace PruSign.iOS
{
	public class MySessionDelegate : NSUrlSessionDelegate	{
		
		public override void DidFinishEventsForBackgroundSession(NSUrlSession session)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				String excepcion = ex.Message;
			}
		}

	}
}
