using PruSign.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruSign
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        private SettingsViewModel SettingsVM { get; set; }

        public SettingsPage ()
		{
			InitializeComponent ();
            SettingsVM = new SettingsViewModel(Navigation);
            BindingContext = SettingsVM;
		}
	}
}