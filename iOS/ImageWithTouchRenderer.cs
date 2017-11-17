using System.Drawing;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using PruSign;
using PruSign.iOS;
using System.ComponentModel;
using PruSign.CustomViews;

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

            if(e.PropertyName == ImageWithTouch.ClearSignatureProperty.PropertyName)
            {
                Control.Lines.Clear();
                Element.ClearSignature = true;
            }
		}

		private void UpdateControl()
		{
			Control.CurrentLineColor = Element.CurrentLineColor.ToUIColor();
		}

	}
}