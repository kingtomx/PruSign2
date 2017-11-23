﻿using Moq;
using PruSign;
using PruSign.Data.ViewModels;
using Xamarin.Forms;
using Xunit;

namespace SharedTests
{
    public class HomeVM_Test
    {
        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void SetClientNamePropertyShouldRaisePropertyChanged()
        {
            bool invoked = false;
            var homeViewModel = new HomeViewModel();

            homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("ClientName"))
                    invoked = true;
            };
            homeViewModel.ClientName = "Client Name Property Change";

            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Set_ClientId_Property_Should_Raise_PropertyChanged()
        {
            bool invoked = false;
            var homeViewModel = new HomeViewModel();

            homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("ClientId"))
                    invoked = true;
            };
            homeViewModel.ClientId = "Client Id Property Change";

            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Set_DocumentId_Property_Should_Raise_PropertyChanged()
        {
            bool invoked = false;
            var homeViewModel = new HomeViewModel();

            homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("DocumentId"))
                    invoked = true;
            };
            homeViewModel.DocumentId = "Document Id Property Change";

            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Set_ApplicationId_Property_Should_Raise_PropertyChanged()
        {
            bool invoked = false;
            var homeViewModel = new HomeViewModel();

            homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Application"))
                    invoked = true;
            };
            homeViewModel.Application = "Application Property Change";

            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Creating_ViewModel_Should_Set_CurrentDate_Property()
        {
            var homeViewModel = new HomeViewModel();

            bool currentDateSetted = false;
            currentDateSetted = homeViewModel.CurrentDate != string.Empty;

            Assert.True(currentDateSetted);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_ClientName_Is_Empty()
        {
            bool messageReceived = false;

            var homeViewModel = new HomeViewModel()
            {
                ClientId = "Test",
                DocumentId = "Test",
                Application = "Test",
            };

            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) => {
                messageReceived = true;
            });
            homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_ClientId_Is_Empty()
        {
            bool messageReceived = false;

            var homeViewModel = new HomeViewModel()
            {
                ClientName = "Test",
                DocumentId = "Test",
                Application = "Test",
            };

            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) => {
                messageReceived = true;
            });
            homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_Application_Is_Empty()
        {

            bool messageReceived = false;

            var homeViewModel = new HomeViewModel()
            {
                ClientId = "Test",
                DocumentId = "Test",
                ClientName = "Test",
            };

            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) => {
                messageReceived = true;
            });
            homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_DocumentId_Is_Empty()
        {
            bool messageReceived = false;

            var homeViewModel = new HomeViewModel()
            {
                ClientId = "Test",
                Application = "Test",
                ClientName = "Test",
            };

            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) => {
                messageReceived = true;
            });
            homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Tap_Settings_Button_Should_Lock_The_Screen()
        {
            bool invoked = false;
            var mockModalHelper = new Mock<HomePage>();

            var homeViewModel = new HomeViewModel();
            homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLocked"))
                    invoked = true;
            };

            homeViewModel.OnSettingsClickedCommand.Execute(null);

            Assert.True(invoked);
        }
    }
}