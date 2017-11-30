using PruSign.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PruSign.Data.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PruSign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogPage : ContentPage
    {
        private LogViewModel _logVm { get; set; }

        public LogPage()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                _logVm = App.Container.Resolve<LogViewModel>(new TypedParameter(typeof(INavigation), Navigation));
                BindingContext = _logVm;

                MessagingCenter.Subscribe<LogViewModel>(this, "LogVM_SendLogs", (sender) =>
                {
                    DisplayAlert("Send Confirmation", "Please confirm that you want to send the logs", "Send", "Cancel")
                        .ContinueWith(action =>
                        {
                            MessagingCenter.Send<LogPage, bool>(this, "LogVM_SendLogsConfirmation", action.Result);
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

                            });
                    });
                });
            }
        }

        protected override void OnAppearing()
        {
            if (_logVm != null)
            {
                Task.Run(async () =>
                {
                    if (_logVm.IsLoading)
                        await _logVm.Initialize();
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
            _logVm = null;
            base.OnDisappearing();
        }
    }
}