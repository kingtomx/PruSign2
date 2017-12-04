using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PruSign.Data.ViewModels;
using Moq;
using PruSign.Data.Interfaces;
using Xamarin.Forms;
using Xunit;

namespace SharedTests
{
    public class SettingsVmTest
    {
        private readonly SettingsViewModel _settingsViewModel;
        private readonly Mock<INavigation> _mockNavigation;
        private readonly Mock<IModalService> _mockModalService;
        private readonly Mock<IDeviceLogService> _mockDeviceLogService;

        public SettingsVmTest()
        {
            _mockModalService = new Mock<IModalService>();
            _mockDeviceLogService = new Mock<IDeviceLogService>();
            _mockNavigation = new Mock<INavigation>();
            _settingsViewModel = new SettingsViewModel(_mockNavigation.Object, _mockModalService.Object, _mockDeviceLogService.Object);
            MockForms.Init();
        }

        [Trait("Category", "SettingsVM - Property Change")]
        [Fact]
        public void Set_IsLoading_Property_Should_Raise_PropertyChanged()
        {
            // ARRANGE
            var invoked = false;
            _settingsViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLoading"))
                    invoked = true;
            };

            // ACT
            _settingsViewModel.IsLoading = true;

            // ASSERT
            Assert.True(invoked);
        }


        // IMPLEMENT AUTOFAC BEFORE CONFIG THIS TEST

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Tap_LogErrorList_Button_Should_Lock_The_Screen()
        {
            // ARRANGE
            var invoked = false;
            _settingsViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLocked"))
                    invoked = true;
            };

            // ACT
            _settingsViewModel.OnViewLogListTappedCommand.Execute(null);

            // ASSERT
            Assert.True(invoked);
        }

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Send_Logs_Button_Tapped_Should_Trigger_Send_Helper()
        {
            // ARRANGE
            var messageReceived = false;
            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogs", (sender) =>
            {
                messageReceived = true;
            });

            // ACT
            _settingsViewModel.OnBtnSendLogsClickedCommand.Execute(null);

            // ASSERT
            Assert.True(messageReceived);
        }

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Cancel_Send_Logs_If_User_Does_Not_Confirm_Shipping()
        {
            // ARRANGE
            var shippingConfirmed = false;
            const bool confirm = false;
            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsSuccess", (sender) =>
            {
                shippingConfirmed = true;
            });
            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsError", (sender) =>
            {
                shippingConfirmed = true;
            });

            // ACT
            _settingsViewModel.OnBtnSendLogsClickedCommand.Execute(null);
            MessagingCenter.Send(_settingsViewModel, "SettingsVM_SendLogsConfirmation", confirm);

            // ASSERT
            Assert.False(shippingConfirmed);
        }

        [Trait("Category", "SettingsVM - Behavior")]
        [Fact]
        public void Try_To_Send_Logs_If_User_Confirm_Shipping()
        {
            // ARRANGE
            var shippingConfirmed = false;
            const bool confirm = true;
            _mockDeviceLogService.Setup(dl => dl.SendDeviceLogs()).Returns(() =>
            {
                return Task.Run(() => new HttpResponseMessage(HttpStatusCode.OK));
            });
            var settingsViewModel = new SettingsViewModel(_mockNavigation.Object, _mockModalService.Object, _mockDeviceLogService.Object);
            MessagingCenter.Subscribe<SettingsViewModel>(this, "SettingsVM_SendLogsSuccess", (sender) =>
            {
                shippingConfirmed = true;
            });

            // ACT
            _settingsViewModel.OnBtnSendLogsClickedCommand.Execute(null);
            MessagingCenter.Send(settingsViewModel, "SettingsVM_SendLogsConfirmation", confirm);

            // ASSERT
            Assert.True(shippingConfirmed);
        }

        //Ideas

    }
}
