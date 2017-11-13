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
    class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OnBtnSubmitTappedCommand { get; set; }
        public ICommand OnSettingsClickedCommand { get; set; }
        public INavigation Navigation { get; set; }
        public bool IsLocked { get; set; }

        #region Properties
        private string clientName;
        public string ClientName
        {
            get { return clientName; }
            set
            {
                clientName = value;
                OnPropertyChanged();
            }
        }

        private string clientId;
        public string ClientId
        {
            get { return clientId; }
            set
            {
                clientId = value;
                OnPropertyChanged();
            }
        }

        private string documentId;
        public string DocumentId
        {
            get { return documentId; }
            set
            {
                documentId = value;
                OnPropertyChanged();
            }
        }

        private string application;
        public string Application
        {
            get { return application; }
            set
            {
                application = value;
                OnPropertyChanged();
            }
        }

        private string currentDate;
        public string CurrentDate
        {
            get { return currentDate; }
            set
            {
                currentDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public HomeViewModel(INavigation navigation)
        {
            OnBtnSubmitTappedCommand = new Command(OnBtnSubmitTapped);
            OnSettingsClickedCommand = new Command(OnSettingsClicked);
            CurrentDate = DateTime.Now.ToString("dd-MM-yyy hh:mm:ss tt");
            Navigation = navigation;
        }

        public void OnBtnSubmitTapped()
        {
            try
            {
                if (string.IsNullOrEmpty(ClientName))
                {
                    SendError("Name cannot be empty");
                }
                else if (string.IsNullOrEmpty(ClientId))
                {
                    SendError("Client Id cannot be empty");
                }
                else if (string.IsNullOrEmpty(DocumentId))
                {
                    SendError("Document Id cannot be empty");
                }
                else if (string.IsNullOrEmpty(Application))
                {
                    SendError("Select an Application to send the signature");
                }
                else
                {
                    SendHelper.SaveSign(ClientName, ClientId, DocumentId, Application, CurrentDate);
                    ClientName = String.Empty;
                    DocumentId = String.Empty;
                    Application = String.Empty;
                    ClientId = String.Empty;
                    CurrentDate = DateTime.Now.ToString("dd-MM-yyy hh:mm:ss tt");
                    MessagingCenter.Send<HomeViewModel>(this, "HomeSuccess");
                    return;
                }
            }
            catch (Exception ex)
            {
                SendError(ex.Message);
            }
        }

        public void OnSettingsClicked()
        {
            if (!IsLocked)
            {
                IsLocked = true;
                ModalHelper.Push(Navigation, new SettingsPage(), () => IsLocked = false);
            }
        }

        void SendError(string errorMessage)
        {
            MessagingCenter.Send<HomeViewModel, string>(this, "HomeError", errorMessage);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
