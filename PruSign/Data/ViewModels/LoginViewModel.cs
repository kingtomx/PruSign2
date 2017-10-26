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
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OnBtnSubmitTappedCommand { get; set; }

        #region Properties
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public LoginViewModel()
        {
            OnBtnSubmitTappedCommand = new Command(OnBtnSubmitTapped);
        }

        public void OnBtnSubmitTapped()
        {
            try
            {
                if (string.IsNullOrEmpty(Username))
                {
                    SendError("Username cannot be empty");
                }
                else if (string.IsNullOrEmpty(Password))
                {
                    SendError("Password cannot be empty");
                }
                else
                {
                    MessagingCenter.Send<LoginViewModel>(this, "RedirectToHome");
                }
            }
            catch (Exception ex)
            {
                SendError(ex.Message);
            }
        }

        void SendError(string errorMessage)
        {
            MessagingCenter.Send<LoginViewModel, string>(this, "Error", errorMessage);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
