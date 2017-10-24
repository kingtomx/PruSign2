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

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
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

        public HomeViewModel()
        {
            OnBtnSubmitTappedCommand = new Command(OnBtnSubmitTapped);
            CurrentDate = DateTime.Now.ToString("dd-MM-yyy hh:mm:ss tt");
        }

        public void OnBtnSubmitTapped()
        {
            try
            {
                if (ClientName == null)
                {
                    ErrorMessage = "Name cannot be empty";
                }
                else if (ClientId == null)
                {
                    ErrorMessage = "Customer Id cannot be empty";
                }
                else if (DocumentId == null)
                {
                    ErrorMessage = "Document Id cannot be empty";
                }
                else if (Application == null)
                {
                    ErrorMessage = "Select an Application to send the signature";
                }
                else
                {
                    SenderUtil.SendSign(ClientName, ClientId, DocumentId, Application, CurrentDate);
                    MessagingCenter.Send<HomeViewModel>(this, "Success");
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            MessagingCenter.Send<HomeViewModel, string>(this, "Error", ErrorMessage);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
