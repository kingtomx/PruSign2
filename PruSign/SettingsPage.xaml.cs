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

            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogs", (sender) =>
            {
                DisplayAlert("Send Confirmation", "Please confirm that you want to send the logs", "Send", "Cancel")
                .ContinueWith(action =>
                {
                    // If the user confirms, then we go back to the view model to process the POST to the backend.
                    if (action.Result)
                    {
                        MessagingCenter.Send<SettingsPage>(this, "SettingsVM_SendLogsConfirmation");
                    }
                });
            });

            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsError", (sender) =>
            {
                DisplayAlert("Error", "There was an error trying to send the logs", "Ok");
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<SettingsViewModel>(this, "SettingsVM_SendLogsError");
            MessagingCenter.Unsubscribe<SettingsViewModel>(this, "SettingsVM_SendLogs");

            base.OnDisappearing();
        }
    }
}