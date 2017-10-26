using PruSign.Data.ViewModels;
using PruSign.Helpers;
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
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel LoginVM { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            LoginVM = new LoginViewModel();
            BindingContext = LoginVM;

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "Error", (sender, arg) =>
            {
                DisplayAlert("Error", (string)arg, "Cancel");
            });
        }
    }
}