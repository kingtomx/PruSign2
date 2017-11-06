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
            base.OnAppearing();

            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginVM_ErrorSavingCredentials", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Error", "There was an error trying to login", "Ok");
                });
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<LoginViewModel, string>(this, "Error");
            MessagingCenter.Unsubscribe<LoginViewModel>(this, "LoginVM_ErrorSavingCredentials");
            LoginVM = null;
            base.OnDisappearing();
        }
    }
}