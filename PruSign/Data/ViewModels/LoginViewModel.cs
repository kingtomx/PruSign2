using PruSign.Data.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PruSign.Data.Interfaces;
using Xamarin.Forms;

namespace PruSign.Data.ViewModels
{
    class LoginViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OnBtnSubmitTappedCommand { get; set; }
        private readonly IDBService _db;
        private readonly IDeviceLogService _deviceLogService;

        #region Properties
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public LoginViewModel(IDBService db, IDeviceLogService deviceLogService)
        {
            _db = db;
            _deviceLogService = deviceLogService;
            OnBtnSubmitTappedCommand = new Command(OnBtnSubmitTapped);
        }

        public async void OnBtnSubmitTapped()
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
                    // TO-DO CHECK AGAINST THE AUTHORIZATIOIN SERVER TO VALIDATE THE CREDENTIALS

                    // Saving credentials
                    try
                    {
                        var userCredentialService = new ServiceAsync<UserCredentials>(_db);
                        await userCredentialService.Add(new UserCredentials()
                        {
                            Username = Username,
                            Password = Password
                        });
                        MessagingCenter.Send(this, "RedirectToHome");
                    }
                    catch (Exception ex)
                    {
                        _deviceLogService.Log(ex);
                        MessagingCenter.Send(this, "LoginVM_ErrorSavingCredentials");
                    }
                }
            }
            catch (Exception ex)
            {
                SendError(ex.Message);
            }
        }

        void SendError(string errorMessage)
        {
            MessagingCenter.Send(this, "Error", errorMessage);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
