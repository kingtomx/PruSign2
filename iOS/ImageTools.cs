using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using UIKit;
using System.Drawing;
using CoreGraphics;

namespace PruSign.iOS
{
	public static class ImageTools
	{


		public static void send(ImageSource img)
		{
			Task<UIImage> bmp = GetBitmapAsync(img);

			UIImage bmpResult = bmp.Result;
			nfloat w = bmpResult.Size.Width;

		}


		// IOS
		public static async Task<UIImage> GetBitmapAsync(ImageSource source)
		{
			var handler = GetHandler(source);
			var returnValue = (UIImage)null;

			returnValue = await handler.LoadImageAsync(source);

			return returnValue;
		}

		// ANDROID
		/*
		private async Task<Bitmap> GetBitmapAsync(ImageSource source)
		{
			var handler = GetHandler(source);
			var returnValue = (Bitmap)null;

			returnValue = await handler.LoadImageAsync(source, this.Context);

			return returnValue;
		}
		*/



		// IOS & ANDROID
		private static IImageSourceHandler GetHandler(ImageSource source)
		{
			IImageSourceHandler returnValue = null;
			if (source is UriImageSource)
			{
				returnValue = new ImageLoaderSourceHandler();
			}
			else if (source is FileImageSource)
			{
				returnValue = new FileImageSourceHandler();
			}
			else if (source is StreamImageSource)
			{
				returnValue = new StreamImagesourceHandler();
			}
			return returnValue;
		}


	}
}

