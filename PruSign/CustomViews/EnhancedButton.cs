using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PruSign.CustomViews
{
    public class EnhancedButton : Button
    {
        #region Padding    

        public static BindableProperty PaddingProperty = 
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(EnhancedButton), default(Thickness), defaultBindingMode: BindingMode.OneWay);

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        #endregion Padding
    }
}
