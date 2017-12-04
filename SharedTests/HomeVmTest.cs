using Moq;
using PruSign.Data.Interfaces;
using PruSign.Data.ViewModels;
using Xamarin.Forms;
using Xunit;

namespace SharedTests
{
    public class HomeVmTest
    {
        private readonly HomeViewModel _homeViewModel;

        public HomeVmTest()
        {
            var mockModalService = new Mock<IModalService>();
            var mockSignatureService = new Mock<ISignatureService>();
            var mockNavigation = new Mock<INavigation>();
            _homeViewModel = new HomeViewModel(mockNavigation.Object, mockModalService.Object, mockSignatureService.Object);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Set_ClientName_Property_Should_Raise_PropertyChanged()
        {
            // ARRANGE
            var invoked = false;
            _homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("ClientName"))
                    invoked = true;
            };

            // ACT
            _homeViewModel.ClientName = "Client Name Property Change";

            // ASSERT
            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Set_ClientId_Property_Should_Raise_PropertyChanged()
        {
            // ARRANGE
            var invoked = false;
            _homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("ClientId"))
                    invoked = true;
            };

            // ACT
            _homeViewModel.ClientId = "Client Id Property Change";

            // ASSERT
            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Set_DocumentId_Property_Should_Raise_PropertyChanged()
        {
            // ARRANGE
            bool invoked = false;
            _homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("DocumentId"))
                    invoked = true;
            };

            // ACT
            _homeViewModel.DocumentId = "Document Id Property Change";

            // ASSERT
            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Set_ApplicationId_Property_Should_Raise_PropertyChanged()
        {
            // ARRANGE
            var invoked = false;
            _homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("Application"))
                    invoked = true;
            };

            // ACT
            _homeViewModel.Application = "Application Property Change";

            // ASSERT
            Assert.True(invoked);
        }

        [Trait("Category", "HomeVM - Property Change")]
        [Fact]
        public void Creating_ViewModel_Should_Set_CurrentDate_Property()
        {
            // ARRANGE

            // ACT
            var currentDateSetted = _homeViewModel.CurrentDate != string.Empty;

            // ASSERT
            Assert.True(currentDateSetted);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_ClientName_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientId = "Test";
            _homeViewModel.DocumentId = "Test";
            _homeViewModel.Application = "Test";
            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) =>
            {
                messageReceived = true;
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_ClientId_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientName = "Test";
            _homeViewModel.DocumentId = "Test";
            _homeViewModel.Application = "Test";
            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) =>
            {
                messageReceived = true;
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_Application_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientId = "Test";
            _homeViewModel.DocumentId = "Test";
            _homeViewModel.ClientName = "Test";
            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) =>
            {
                messageReceived = true;
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Not_Be_Cleaned_If_DocumentId_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientId = "Test";
            _homeViewModel.Application = "Test";
            _homeViewModel.ClientName = "Test";
            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) =>
            {
                messageReceived = true;
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.False(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Signature_Should_Be_Cleaned_If_All_Fields_Are_Ok()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientId = "Test";
            _homeViewModel.Application = "Test";
            _homeViewModel.ClientName = "Test";
            _homeViewModel.DocumentId = "Test";
            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) =>
            {
                messageReceived = true;
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.True(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Display_Success_Message_After_Send_If_All_Fields_Are_Ok()
        {
            // ARRANGE
            var messageReceived = false;

            _homeViewModel.ClientId = "Test";
            _homeViewModel.Application = "Test";
            _homeViewModel.ClientName = "Test";
            _homeViewModel.DocumentId = "Test";
            MessagingCenter.Subscribe<HomeViewModel>(this, "HomeSuccess", (sender) =>
            {
                messageReceived = true;
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.True(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Display_Error_Message_If_ClientId_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.Application = "Test";
            _homeViewModel.ClientName = "Test";
            _homeViewModel.DocumentId = "Test";
            MessagingCenter.Subscribe<HomeViewModel, string>(this, "HomeError", (sender, message) =>
            {
                if (message.Equals("Client Id cannot be empty"))
                {
                    messageReceived = true;
                }
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.True(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Display_Error_Message_If_Application_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientId = "Test";
            _homeViewModel.ClientName = "Test";
            _homeViewModel.DocumentId = "Test";
            MessagingCenter.Subscribe<HomeViewModel, string>(this, "HomeError", (sender, message) =>
            {
                if (message.Equals("Select an Application to send the signature"))
                {
                    messageReceived = true;
                }
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.True(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Display_Error_Message_If_ClientName_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientId = "Test";
            _homeViewModel.Application = "Test";
            _homeViewModel.DocumentId = "Test";
            MessagingCenter.Subscribe<HomeViewModel, string>(this, "HomeError", (sender, message) =>
            {
                if (message.Equals("Name cannot be empty"))
                {
                    messageReceived = true;
                }
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.True(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Display_Error_Message_If_DocumentId_Is_Empty()
        {
            // ARRANGE
            var messageReceived = false;
            _homeViewModel.ClientId = "Test";
            _homeViewModel.Application = "Test";
            _homeViewModel.ClientName = "Test";
            MessagingCenter.Subscribe<HomeViewModel, string>(this, "HomeError", (sender, message) =>
            {
                if (message.Equals("Document Id cannot be empty"))
                {
                    messageReceived = true;
                }
            });

            // ACT
            _homeViewModel.OnBtnSubmitTappedCommand.Execute(null);

            // ASSERT
            Assert.True(messageReceived);
        }

        [Fact]
        [Trait("Category", "HomeVM - Behavior")]
        public void Tap_Settings_Button_Should_Lock_The_Screen()
        {
            // ARRANGE
            var invoked = true;
            _homeViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("IsLocked"))
                    invoked = true;
            };

            // ACT
            _homeViewModel.OnSettingsClickedCommand.Execute(null);

            // ASSERT
            Assert.True(invoked);
        }
    }
}