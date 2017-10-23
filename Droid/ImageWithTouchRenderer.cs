﻿using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using PruSign;
using PruSign.Android;

[assembly: ExportRenderer(typeof(ImageWithTouch), typeof(ImageWithTouchRenderer))]

namespace PruSign.Android
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
		}

		private void UpdateControl()
		{
			Control.CurrentLineColor = Element.CurrentLineColor.ToAndroid();
		}


	}
}