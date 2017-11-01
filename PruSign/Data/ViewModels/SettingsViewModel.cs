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

        public SettingsViewModel(INavigation navigation)
        {
            OnViewLogListTappedCommand = new Command(OnViewLogListTapped);
            OnBtnSendLogsClickedCommand = new Command(OnBtnSendLogsClicked);
            OnBtnCloseClickedCommand = new Command(OnBtnCloseClicked);
            Navigation = navigation;

            MessagingCenter.Subscribe<SettingsPage>(this, "SettingsVM_SendLogsConfirmation", async (sender) =>
            {
                try
                {
                    await SenderUtil.SendDeviceLogs();
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                    MessagingCenter.Send<SettingsViewModel>(this, "SettingsVM_SendLogsError");
                    Console.WriteLine(ex.ToString());
                }
            });
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
            MessagingCenter.Send<SettingsViewModel>(this, "SettingsVM_SendLogs");
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
