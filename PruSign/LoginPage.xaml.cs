using PruSign.Data.ViewModels;
using Autofac;
using PruSign.Data.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruSign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _loginVm { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                _loginVm = App.Container.Resolve<LoginViewModel>();
                BindingContext = _loginVm;

                MessagingCenter.Subscribe<LoginViewModel, string>(this, "Error", (sender, arg) =>
                {
                    DisplayAlert("Error", arg, "Cancel");
                });
                base.OnAppearing();

                MessagingCenter.Subscribe<LoginViewModel>(this, "LoginVM_ErrorSavingCredentials", (sender) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Error", "There was an error trying to login", "Ok");
                    });
                });

                MessagingCenter.Subscribe<App, string>(this, "showDataFromOtherApp", (sender, arg) =>
                {
                    DisplayAlert("Url", arg, "Ok");
                });
            }
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<LoginViewModel, string>(this, "Error");
            MessagingCenter.Unsubscribe<LoginViewModel>(this, "LoginVM_ErrorSavingCredentials");
            MessagingCenter.Unsubscribe<App>(this, "showDataFromOtherApp");
            _loginVm = null;
            base.OnDisappearing();
        }
    }
}