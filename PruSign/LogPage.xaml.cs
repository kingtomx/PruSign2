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

            MessagingCenter.Subscribe<LogViewModel>(this, "CannotGetLogs", (sender) =>
            {
                DisplayAlert("Error", "There was an error trying to get the logs", "Ok");
            });

            MessagingCenter.Subscribe<LogViewModel>(this, "SendLogs", (sender) =>
            {
                DisplayAlert("Error", "There was an error trying to get the logs", "Send","Cancel").ContinueWith(action => {
                    if (action.Result)
                    {
                        // TO - DO: Send Logs as JSON
                        
                    }
                });
            });
        }

        protected override void OnAppearing()
        {
            if (LogVM != null)
            {
                Task.Run(async () =>
                {
                    if(LogVM.IsLoading)
                        await LogVM.Initialize();
                });
            }

            base.OnAppearing();
        }
    }
}