﻿using PruSign.Data.Entities;
using PruSign.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        private bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        // Used to show the Activity Indicator
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private List<LogEntry> _logs;
        public List<LogEntry> Logs
        {
            get => _logs;
            set
            {
                _logs = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public LogViewModel(INavigation navigation)
        {
            OnBtnSendLogsClickedCommand = new Command(OnBtnSendLogsClicked);
            OnBtnCloseClickedCommand = new Command(OnBtnCloseClicked);
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
                IsEmpty = (Logs.Count <= 0);

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                //FailedToLoad = true;
                MessagingCenter.Send(this, "LogVM_CannotGetLogs");
            }

            IsLoading = false;
        }

        public void OnBtnCloseClicked()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PopModalAsync();
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
                    MessagingCenter.Send(this,
                        response.IsSuccessStatusCode ? "LogVM_SendLogsSuccess" : "LogVM_SendLogsError");
                }

                MessagingCenter.Unsubscribe<LogPage, bool>(this, "LogVM_SendLogsConfirmation");

            });

            MessagingCenter.Send(this, "LogVM_SendLogs");
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
