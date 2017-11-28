using PruSign;
using PruSign.Data.ViewModels;
using Xunit;

namespace SharedTests
{
    public class LogVmTest
    {

        public LogPage Log { get; set; }

        public LogVmTest()
        {
            MockForms.Init();
            var app = new App();
            Log = new LogPage();
        }

        [Trait("Category", "LogVM - Property Change")]
        [Fact]
        public void Set_IsEmpty_Property_Should_Raise_PropertyChanged()
        {
            var invoked = false;
            var logViewModel = new LogViewModel(Log.Navigation);

            logViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsEmpty"))
                    invoked = true;
            };
            logViewModel.IsEmpty = true;

            Assert.True(invoked);
        }
    }
}
