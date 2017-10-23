using UIKit;

namespace PruSign.iOS
{
	public static class UIViewExtensions
	{

		public static UIImage AsImage(this UIView view)
		{
			UIGraphics.BeginImageContextWithOptions(view.Bounds.Size, view.Opaque, 1);
			view.DrawViewHierarchy(view.Frame, true); //this was key line
			UIImage img = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return img;
		}

		public static UIImage TakeScreenshot()
		{
			return UIApplication.SharedApplication.KeyWindow.AsImage();
		}

	}


}