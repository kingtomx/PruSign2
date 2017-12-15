using Xamarin.Forms;
using Autofac;
using PruSign.Data.ViewModels;
using Xamarin.Forms.Xaml;
using PruSign.Data.Interfaces;

namespace PruSign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HomeViewModel _homeVm;

        public HomePage()
        {
            InitializeComponent();
            using (App.Container.BeginLifetimeScope())
            {
                _homeVm = App.Container.Resolve<HomeViewModel>(new TypedParameter(typeof(INavigation), Navigation));
                _homeVm.UpdateIncomingData();
                BindingContext = _homeVm;

                MessagingCenter.Subscribe<HomeViewModel, string>(this, "HomeError", (sender, arg) =>
                {
                    DisplayAlert("Error", arg, "Cancel");
                });

                MessagingCenter.Subscribe<HomeViewModel>(this, "HomeSuccess", (sender) =>
                {
                    DisplayAlert("Success", "Your signature has been saved", "OK");
                });

                MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) =>
                {
                    DrawingArea.ClearSignature = true;
                });

                MessagingCenter.Subscribe<App>(this, "showDataFromOtherApp", (sender) =>
                {
                    _homeVm.UpdateIncomingData();
                });
            }
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeError");
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeSuccess");
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "CleanSignature");
            MessagingCenter.Unsubscribe<App>(this, "showDataFromOtherApp");

            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _homeVm = App.Container.Resolve<HomeViewModel>(new TypedParameter(typeof(INavigation), Navigation));
        }
    }
}
