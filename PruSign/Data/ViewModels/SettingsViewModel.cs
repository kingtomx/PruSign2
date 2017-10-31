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
        public ICommand OnSendLogsTappedCommand { get; set; }
        public ICommand OnBtnCloseClickedCommand { get; set; }
        public INavigation Navigation { get; set; }

        public bool IsLocked { get; set; }

        public SettingsViewModel(INavigation navigation)
        {
            OnViewLogListTappedCommand = new Command(OnViewLogListTapped);
            OnSendLogsTappedCommand = new Command(OnSendLogsTapped);
            OnBtnCloseClickedCommand = new Command(OnBtnCloseClicked);
            Navigation = navigation;
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

        public void OnSendLogsTapped()
        {

        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
