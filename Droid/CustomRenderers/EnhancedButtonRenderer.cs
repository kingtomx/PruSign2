using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using PruSign.CustomViews;
using PruSign.Droid.CustomRenderers;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(EnhancedButton), typeof(EnhancedButtonRenderer))]
namespace PruSign.Droid.CustomRenderers
{
    public class EnhancedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Elevation = 0;
            }
            UpdatePadding();
        }

        private void UpdatePadding()
        {
            var element = this.Element as EnhancedButton;
            if (element != null)
            {
                this.Control.SetPadding(
                    (int)element.Padding.Left,
                    (int)element.Padding.Top,
                    (int)element.Padding.Right,
                    (int)element.Padding.Bottom
                );
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(EnhancedButton.Padding))
            {
                UpdatePadding();
            }
        }
    }
}