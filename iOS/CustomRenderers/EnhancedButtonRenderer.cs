using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using PruSign.iOS.CustomRenderers;
using PruSign.CustomViews;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(EnhancedButton), typeof(EnhancedButtonRenderer))]
namespace PruSign.iOS.CustomRenderers
{
    public class EnhancedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            UpdatePadding();
        }

        private void UpdatePadding()
        {
            var element = this.Element as EnhancedButton;
            if (element != null)
            {
                this.Control.ContentEdgeInsets = new UIEdgeInsets(

                    (int)element.Padding.Top,
                    (int)element.Padding.Left,
                    (int)element.Padding.Bottom,
                    (int)element.Padding.Right
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