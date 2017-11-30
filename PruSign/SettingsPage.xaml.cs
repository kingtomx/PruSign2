using PruSign.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruSign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _settingsVm { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                _settingsVm = App.Container.Resolve<SettingsViewModel>(new TypedParameter(typeof(INavigation), Navigation));
                BindingContext = _settingsVm;
            }
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogs", (sender) =>
            {
                DisplayAlert("Send Confirmation", "Please confirm that you want to send the logs", "Send", "Cancel")
                .ContinueWith(action =>
                {
                    MessagingCenter.Send<SettingsPage, bool>(this, "SettingsVM_SendLogsConfirmation", action.Result);
                });
            });

            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsError", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Error", "There was an error trying to send the logs", "Ok");
                });
            });


            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsSuccess", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Success", "Your logs have been sent", "Ok");
                });
            });
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<SettingsViewModel>(this, "SettingsVM_SendLogs");
            MessagingCenter.Unsubscribe<SettingsViewModel>(this, "SettingsVM_SendLogsError");
            MessagingCenter.Unsubscribe<SettingsViewModel>(this, "SettingsVM_SendLogsSuccess");
            _settingsVm = null;
            base.OnDisappearing();
        }
    }
}