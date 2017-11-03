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

        public SettingsPage()
        {
            InitializeComponent();
            SettingsVM = new SettingsViewModel(Navigation);
            BindingContext = SettingsVM;
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
            SettingsVM = null;
            base.OnDisappearing();
        }
    }
}