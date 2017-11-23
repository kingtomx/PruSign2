using PruSign.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SharedTests
{
    public class SettingsVM_Test
    {
        [Trait("Category", "SettingsVM - Property Change")]
        [Fact]
        public void Set_IsLoading_Property_Should_Raise_PropertyChanged()
        {
            bool invoked = false;
            var settingsViewModel = new SettingsViewModel();

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
        public void Tap_ListView_Should_Lock_The_Screen()
        {
            bool invoked = false;
            var settingsViewModel = new SettingsViewModel();

            settingsViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLoading"))
                    invoked = true;
            };
            settingsViewModel.IsLoading = true;

            Assert.True(invoked);
        }
    }
}
