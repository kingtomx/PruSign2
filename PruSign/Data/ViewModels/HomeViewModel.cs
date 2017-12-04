using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PruSign.Data.Interfaces;
using Xamarin.Forms;

namespace PruSign.Data.ViewModels
{
    public class HomeViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OnBtnSubmitTappedCommand { get; set; }
        public ICommand OnSettingsClickedCommand { get; set; }
        private readonly INavigation _navigation;
        private readonly IModalService _modalService;
        private readonly ISignatureService _signatureService;

        #region Properties
        private bool _isLocked;
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                OnPropertyChanged();
            }
        }

        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged();
            }
        }

        private string _clientId;
        public string ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                OnPropertyChanged();
            }
        }

        private string _documentId;
        public string DocumentId
        {
            get => _documentId;
            set
            {
                _documentId = value;
                OnPropertyChanged();
            }
        }

        private string _application;
        public string Application
        {
            get => _application;
            set
            {
                _application = value;
                OnPropertyChanged();
            }
        }

        private string _currentDate;
        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public HomeViewModel(INavigation navigation, IModalService modalService, ISignatureService signatureService)
        {
            _signatureService = signatureService;
            OnBtnSubmitTappedCommand = new Command(OnBtnSubmitTapped);
            OnSettingsClickedCommand = new Command(OnSettingsClicked);
            CurrentDate = DateTime.Now.ToString("dd-MM-yyy hh:mm:ss tt");
            _navigation = navigation;
            _modalService = modalService;
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
                    _signatureService.SaveSign(ClientName, ClientId, DocumentId, Application, CurrentDate);
                    ClientName = string.Empty;
                    DocumentId = string.Empty;
                    Application = string.Empty;
                    ClientId = string.Empty;
                    CurrentDate = DateTime.Now.ToString("dd-MM-yyy hh:mm:ss tt");
                    MessagingCenter.Send(this, "CleanSignature");
                    MessagingCenter.Send(this, "HomeSuccess");
                }
            }
            catch (Exception ex)
            {
                SendError(ex.Message);
            }
        }

        public virtual void OnSettingsClicked()
        {
            if (IsLocked) return;
            IsLocked = true;
            _modalService.Push(_navigation, typeof(SettingsPage), () => IsLocked = false);
        }

        public void SendError(string errorMessage)
        {
            MessagingCenter.Send(this, "HomeError", errorMessage);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
