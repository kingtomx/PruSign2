using PruSign.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruSign.Data.ViewModels
{
    class SettingsViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OnViewLogListTappedCommand { get; set; }
        public ICommand OnBtnSendLogsClickedCommand { get; set; }
        public ICommand OnBtnCloseClickedCommand { get; set; }
        public INavigation Navigation { get; set; }

        public bool IsLocked { get; set; }

        // Used to show the Activity Indicator
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel(INavigation navigation)
        {
            IsLoading = false;
            OnViewLogListTappedCommand = new Command(OnViewLogListTapped);
            OnBtnSendLogsClickedCommand = new Command(OnBtnSendLogsClicked);
            OnBtnCloseClickedCommand = new Command(OnBtnCloseClicked);
            Navigation = navigation;
        }

        public void OnViewLogListTapped()
        {
            if (IsLocked) return;
            IsLocked = true;
            ModalHelper.Push(Navigation, new LogPage(), () => IsLocked = false);
        }

        public void OnBtnCloseClicked()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopModalAsync();
            });
        }

        public void OnBtnSendLogsClicked()
        {
            // This Message waits for user confirmation before run the SendDeviceLogs function
            MessagingCenter.Subscribe<SettingsPage, bool>(this, "SettingsVM_SendLogsConfirmation", async (sender, flag) =>
            {
                if (flag)
                {
                    IsLoading = true;
                    var response = await SendHelper.SendDeviceLogs();
                    IsLoading = false;
                    MessagingCenter.Send(this,
                        response.IsSuccessStatusCode ? "SettingsVM_SendLogsSuccess" : "SettingsVM_SendLogsError");
                }

                MessagingCenter.Unsubscribe<SettingsPage, bool>(this, "SettingsVM_SendLogsConfirmation");

            });
            MessagingCenter.Send(this, "SettingsVM_SendLogs");
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
