using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using PruSign;
using PruSign.CustomViews;
using PruSign.Droid;
using Android.Graphics;


[assembly: ExportRenderer(typeof(ImageWithTouch), typeof(ImageWithTouchRenderer))]

namespace PruSign.Droid
{
	public class ImageWithTouchRenderer : ViewRenderer<ImageWithTouch, DrawView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ImageWithTouch> e)
		{
			base.OnElementChanged(e);

			int androidId = this.Id;
			Guid xamarinId = e.NewElement.Id;

			if (e.OldElement == null)
			{
				SetNativeControl(new DrawView(Context));
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ImageWithTouch.CurrentLineColorProperty.PropertyName)
			{
				UpdateControl();
			}

            if (e.PropertyName == ImageWithTouch.ClearSignatureProperty.PropertyName)
            {
                var PenWidth = 3.0f;
                Paint DrawPaintReset = new Paint
                {
                    Color = Android.Graphics.Color.White,
                    AntiAlias = true,
                    StrokeWidth = PenWidth
                };
                Control.DrawCanvas.DrawPaint(DrawPaintReset);
                Element.ClearSignature = false;
            }
        }

		private void UpdateControl()
		{
			Control.CurrentLineColor = Element.CurrentLineColor.ToAndroid();
		}


	}
}