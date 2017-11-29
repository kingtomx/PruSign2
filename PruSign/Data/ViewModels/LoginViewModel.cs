using PruSign.Data.Entities;
using PruSign.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruSign.Data.ViewModels
{
    class LoginViewModel : IViewModel
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

        //public async Task IsAuthenticated()
        //{
        //    // Checking if the credentials are stored in the database
        //    var db = new PruSignDatabase();
        //    var userCredentialService = new ServiceAsync<UserCredentials>(db);
        //    int result = await userCredentialService.GetAll().CountAsync();
        //    if(result > 0)
        //        MessagingCenter.Send<LoginViewModel>(this, "RedirectToHome");
        //}

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
                        var db = new PruSignDatabase();
                        var userCredentialService = new ServiceAsync<UserCredentials>(db);
                        await userCredentialService.Add(new UserCredentials()
                        {
                            Username = this.Username,
                            Password = this.Password
                        });
                        MessagingCenter.Send<LoginViewModel>(this, "RedirectToHome");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(ex);
                        MessagingCenter.Send<LoginViewModel>(this, "LoginVM_ErrorSavingCredentials");
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
            MessagingCenter.Send<LoginViewModel, string>(this, "Error", errorMessage);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
