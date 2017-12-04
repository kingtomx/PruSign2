using Xamarin.Forms;
using Autofac;
using PruSign.Data.ViewModels;
using Xamarin.Forms.Xaml;

namespace PruSign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        private HomeViewModel _homeVm;

        public HomePage()
        {
            InitializeComponent();
            using (App.Container.BeginLifetimeScope())
            {
                _homeVm = App.Container.Resolve<HomeViewModel>(new TypedParameter(typeof(INavigation), Navigation));
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
            }
        }

        protected override void OnDisappearing()
        {
            _homeVm = null;
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeError");
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeSuccess");
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "CleanSignature");

            base.OnDisappearing();
        }
    }
}
