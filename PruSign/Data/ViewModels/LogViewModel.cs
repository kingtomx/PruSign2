using PruSign.Data.Entities;
using PruSign.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PruSign.Data.ViewModels
{
    class LogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OnBtnCloseClickedCommand { get; set; }
        public ICommand OnBtnSendLogsClickedCommand { get; set; }
        public INavigation Navigation { get; set; }
        private PruSignDatabase Db { get; set; }
        private ServiceAsync<LogEntry> ServiceLogs { get; set; }

        #region Properties
        private bool isEmpty;
        public bool IsEmpty
        {
            get { return isEmpty; }
            set
            {
                isEmpty = value;
                OnPropertyChanged();
            }
        }

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

        private List<LogEntry> logs;
        public List<LogEntry> Logs
        {
            get { return logs; }
            set
            {
                logs = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public LogViewModel(INavigation navigation)
        {
            OnBtnCloseClickedCommand = new Command(OnBtnCloseClicked);
            OnBtnSendLogsClickedCommand = new Command(OnBtnSendLogsClicked);
            IsLoading = true;
            IsEmpty = false;
            Navigation = navigation;
            Logs = new List<LogEntry>();
            Db = new PruSignDatabase();
            ServiceLogs = new ServiceAsync<LogEntry>(Db);
        }

        public async Task Initialize()
        {
            try
            {
                // Try to get the log list. If there is any issue retrieving results 
                Logs = await ServiceLogs.GetAll()
                    .OrderByDescending(log => log.Created)
                    .Take(20).ToListAsync();
                IsEmpty = (Logs.Count > 0) ? false : true;

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                //FailedToLoad = true;
                MessagingCenter.Send<LogViewModel>(this, "LogVM_CannotGetLogs");
            }

            IsLoading = false;
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
            MessagingCenter.Subscribe<LogPage, bool>(this, "LogVM_SendLogsConfirmation", async (sender, flag) =>
            {
                if (flag)
                {
                    IsLoading = true;
                    var response = await SendHelper.SendDeviceLogs();
                    IsLoading = false;
                    if (response.IsSuccessStatusCode)
                    {
                        // If the operation was successfull, we'll show a success message
                        MessagingCenter.Send<LogViewModel>(this, "LogVM_SendLogsSuccess");
                    }
                    else
                    {
                        MessagingCenter.Send<LogViewModel>(this, "LogVM_SendLogsError");
                    }
                }

                MessagingCenter.Unsubscribe<LogPage, bool>(this, "LogVM_SendLogsConfirmation");

            });

            MessagingCenter.Send<LogViewModel>(this, "LogVM_SendLogs");
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
