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
    public partial class LogPage : ContentPage
    {
        private LogViewModel LogVM { get; set; }

        public LogPage()
        {
            InitializeComponent();
            LogVM = new LogViewModel(Navigation);
            BindingContext = LogVM;

            MessagingCenter.Subscribe<LogViewModel>(this, "LogVM_SendLogs", (sender) =>
            {
                DisplayAlert("Send Confirmation", "Please confirm that you want to send the logs", "Send", "Cancel")
                .ContinueWith(action =>
                {
                    // If the user confirms, then we go back to the view model to process the POST to the backend.
                    if (action.Result)
                    {
                        MessagingCenter.Send<LogPage>(this, "LogVM_SendLogsConfirmation");
                    }
                });
            });

            MessagingCenter.Subscribe<LogViewModel>(this, "LogVM_SendLogsError", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Error", "There was an error trying to send the logs", "Ok");
                });
            });

            MessagingCenter.Subscribe<LogViewModel>(this, "LogVM_SendLogsSuccess", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Success", "Your logs have been sent", "Ok");
                });
            });

            MessagingCenter.Subscribe<LogViewModel>(this, "LogVM_CannotGetLogs", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Error", "There was an error trying to get the logs", "Ok")
                    .ContinueWith(action =>
                    {
                        Navigation.PopModalAsync();
                    });
                });
            });
        }

        protected override void OnAppearing()
        {
            if (LogVM != null)
            {
                Task.Run(async () =>
                {
                    if (LogVM.IsLoading)
                        await LogVM.Initialize();
                });
            }

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<LogViewModel>(this, "LogVM_SendLogs");
            MessagingCenter.Unsubscribe<LogViewModel>(this, "LogVM_SendLogsError");
            MessagingCenter.Unsubscribe<LogViewModel>(this, "LogVM_SendLogsSuccess");
            MessagingCenter.Unsubscribe<LogViewModel>(this, "LogVM_CannotGetLogs");
            base.OnDisappearing();
        }
    }
}