using Moq;
using PruSign;
using PruSign.Data.Entities;
using PruSign.Data.Interfaces;
using PruSign.Data.ViewModels;
using Xamarin.Forms;
using Xunit;

namespace SharedTests
{
    public class LogVmTest
    {
        private readonly LogViewModel _logViewModel;

        public LogVmTest()
        {
            var mockNavigation = new Mock<INavigation>();
            var mockDeviceLogService = new Mock<IDeviceLogService>();
            var mockLogEntryServiceAsync = new Mock<IServiceAsync<LogEntry>>();
            _logViewModel = new LogViewModel(mockNavigation.Object, mockDeviceLogService.Object, mockLogEntryServiceAsync.Object);
        }

        [Trait("Category", "LogVM - Property Change")]
        [Fact]
        public void Set_IsEmpty_Property_Should_Raise_PropertyChanged()
        {
            // ARRANGE
            var invoked = false;
            _logViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsEmpty"))
                    invoked = true;
            };

            // ACT
            _logViewModel.IsEmpty = true;

            // ASSERT
            Assert.True(invoked);
        }
    }
}
