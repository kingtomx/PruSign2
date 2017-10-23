using Android.Views;
using Android.Graphics;
using Android.Content;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PruSign.Android
{

	public class GestureListener : GestureDetector.SimpleOnGestureListener
	{
		private static bool doubleTapped;
		private static long lastTouched = -1;
		private static long oneSecond = 10000000;

		public override bool OnDown(MotionEvent e)
		{
			long nowTouched = System.DateTime.Now.Ticks;
			if (lastTouched != -1 && (nowTouched - lastTouched < oneSecond))
			{
				lastTouched = nowTouched;
				return true;
			}
			else
			{
				lastTouched = nowTouched;
				return false;
			}

		}

		public override bool OnDoubleTap(MotionEvent e)
		{
			return true;
		}
	}
	
	

}