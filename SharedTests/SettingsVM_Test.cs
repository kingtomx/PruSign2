using PruSign.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PruSign;
using Xamarin.Forms;
using Xunit;

namespace SharedTests
{
    public class SettingsVM_Test
    {
        public SettingsPage Settings { get; set; }

        public SettingsVM_Test()
        {
            MockForms.Init();
            var app = new App();
            Settings = new SettingsPage();
        }

        [Trait("Category", "SettingsVM - Property Change")]
        [Fact]
        public void Set_IsLoading_Property_Should_Raise_PropertyChanged()
        {
            bool invoked = false;
            var settingsViewModel = new SettingsViewModel(Settings.Navigation);

            settingsViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLoading"))
                    invoked = true;
            };
            settingsViewModel.IsLoading = true;

            Assert.True(invoked);
        }

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Tap_LogErrorList_Button_Should_Lock_The_Screen()
        {
            bool invoked = false;
            var settingsViewModel = new SettingsViewModel(Settings.Navigation);

            settingsViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLocked"))
                    invoked = true;
            };
            settingsViewModel.OnViewLogListTappedCommand.Execute(null);

            Assert.True(invoked);
        }

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Send_Logs_Button_Tapped_Should_Trigger_Send_Helper()
        {
            var messageReceived = false;
            var settingsViewModel = new SettingsViewModel(Settings.Navigation);

            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogs", (sender) =>
            {
                messageReceived = true;
            });
            settingsViewModel.OnBtnSendLogsClickedCommand.Execute(null);

            Assert.True(messageReceived);
        }

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Cancel_Send_Logs_If_User_Does_Not_Confirm_Shipping()
        {
            var shippingConfirmed = false;
            const bool confirm = false;

            var settingsViewModel = new SettingsViewModel(Settings.Navigation);


            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsSuccess", (sender) =>
            {
                shippingConfirmed = true;
            });

            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsError", (sender) =>
            {
                shippingConfirmed = true;
            });

            settingsViewModel.OnBtnSendLogsClickedCommand.Execute(null);

            MessagingCenter.Send<SettingsPage, bool>(Settings, "SettingsVM_SendLogsConfirmation", confirm);

            Assert.False(shippingConfirmed);
        }

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Try_To_Send_Logs_If_User_Confirm_Shipping()
        {
            var shippingConfirmed = false;
            const bool confirm = true;

            var settingsViewModel = new SettingsViewModel(Settings.Navigation);


            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsSuccess", (sender) =>
            {
                shippingConfirmed = true;
            });

            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsError", (sender) =>
            {
                shippingConfirmed = true;
            });

            settingsViewModel.OnBtnSendLogsClickedCommand.Execute(null);

            MessagingCenter.Send<SettingsPage, bool>(Settings, "SettingsVM_SendLogsConfirmation", confirm);

            Assert.True(shippingConfirmed);
        }

        //Ideas

    }
}
