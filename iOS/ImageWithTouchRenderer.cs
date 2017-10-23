using System.Drawing;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using PruSign;
using PruSign.iOS;
using System.ComponentModel;
using CoreGraphics;

[assembly: ExportRenderer(typeof(ImageWithTouch), typeof(ImageWithTouchRenderer))]
namespace PruSign.iOS
{
	public class ImageWithTouchRenderer : ViewRenderer<ImageWithTouch, DrawView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ImageWithTouch> e)
		{
			base.OnElementChanged(e);

			SetNativeControl(new DrawView(RectangleF.Empty));
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ImageWithTouch.CurrentLineColorProperty.PropertyName)
			{
				UpdateControl();
			}
		}

		private void UpdateControl()
		{
			Control.CurrentLineColor = Element.CurrentLineColor.ToUIColor();
		}



	}
}