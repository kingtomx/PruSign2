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
                ShowNoResults = !failedToLoad && isEmpty && !isLoading;
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

        // Used to show an error when there was any problem trying to retrieve the logs.
        private bool failedToLoad;
        public bool FailedToLoad
        {
            get { return failedToLoad; }
            set
            {
                failedToLoad = value;
                OnPropertyChanged();
                ShowNoResults = !failedToLoad && isEmpty && !isLoading;
            }
        }

        // Used to show a friendly message when there are no results to display.
        private bool showNoResults;
        public bool ShowNoResults
        {
            get { return showNoResults; }
            set
            {
                showNoResults = !failedToLoad && isEmpty;
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
            FailedToLoad = false;
            IsEmpty = false;
            ShowNoResults = false;
            Navigation = navigation;
            Logs = new List<LogEntry>();
            Db = new PruSignDatabase();
            ServiceLogs = new ServiceAsync<LogEntry>(Db);

            MessagingCenter.Subscribe<LogViewModel>(this, "LogVM_SendLogsConfirmation", async (sender) =>
            {
                try
                {
                    var request = await SenderUtil.SendDeviceLogs();
                    Console.WriteLine(request);
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                    MessagingCenter.Send<LogViewModel>(this, "LogVM_SendLogsError");
                }
            });
        }

        public async Task Initialize()
        {
            try
            {
                // Try to get the log list. If there is any issue retrieving results, 
                // the application will display an alert message and return to the previous page
                Logs = await ServiceLogs.GetAll()
                    .OrderByDescending(log => log.Created)
                    .Take(20).ToListAsync();
                IsEmpty = (Logs.Count > 0) ? false : true;

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                FailedToLoad = true;
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
            MessagingCenter.Send<LogViewModel>(this, "LogVM_SendLogs");
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
