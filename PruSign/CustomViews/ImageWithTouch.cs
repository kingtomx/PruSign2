using Xamarin.Forms;

namespace PruSign.CustomViews
{
	public class ImageWithTouch : Image
	{
		public static readonly BindableProperty CurrentLineColorProperty =
			BindableProperty.Create((ImageWithTouch w) => w.CurrentLineColor, Color.Default);

		public Color CurrentLineColor
		{
			get
			{
				return (Color)GetValue(CurrentLineColorProperty);
			}
			set
			{
				SetValue(CurrentLineColorProperty, value);
			}
		}

        public static readonly BindableProperty ClearSignatureProperty =
            BindableProperty.Create((ImageWithTouch w) => w.ClearSignature, false);

        public bool ClearSignature
        {
            get
            {
                return (bool)GetValue(ClearSignatureProperty);
            }
            set
            {
                SetValue(ClearSignatureProperty, value);
            }
        }

	}
}