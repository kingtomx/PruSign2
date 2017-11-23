using PruSign.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruSign.Data.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OnViewLogListTappedCommand { get; set; }
        public ICommand OnBtnSendLogsClickedCommand { get; set; }
        public ICommand OnBtnCloseClickedCommand { get; set; }
        public INavigation Navigation { get; set; }

        public bool IsLocked { get; set; }

        // Used to show the Activity Indicator
        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
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

        public SettingsViewModel()
        {

        }

        public void OnViewLogListTapped()
        {
            if (!IsLocked)
            {
                IsLocked = true;
                ModalHelper.Push(Navigation, new LogPage(), () => IsLocked = false);
            }
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
                    if (response.IsSuccessStatusCode)
                    {
                        // If the operation was successfull, we'll show a success message
                        MessagingCenter.Send<SettingsViewModel>(this, "SettingsVM_SendLogsSuccess");
                    }
                    else
                    {
                        MessagingCenter.Send<SettingsViewModel>(this, "SettingsVM_SendLogsError");
                    }
                }

                MessagingCenter.Unsubscribe<SettingsPage, bool>(this, "SettingsVM_SendLogsConfirmation");

            });
            MessagingCenter.Send<SettingsViewModel>(this, "SettingsVM_SendLogs");
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
