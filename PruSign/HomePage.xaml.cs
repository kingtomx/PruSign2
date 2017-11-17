using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using PruSign.Data.ViewModels;
using PruSign.CustomViews;
using Xamarin.Forms.Xaml;

namespace PruSign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HomeViewModel HomeVM { get; set; }
        public HomePage()
        {
            InitializeComponent();

            HomeVM = new HomeViewModel(Navigation);
            BindingContext = HomeVM;

            MessagingCenter.Subscribe<HomeViewModel, string>(this, "HomeError", (sender, arg) => {
                DisplayAlert("Error", (string)arg, "Cancel");
            });

            MessagingCenter.Subscribe<HomeViewModel>(this, "HomeSuccess", (sender) => {
                DisplayAlert("Success", "Your signature has been saved", "OK");
            });

            MessagingCenter.Subscribe<HomeViewModel>(this, "CleanSignature", (sender) => {
                DrawingArea.ClearSignature = true;
            });
        }

        protected override void OnDisappearing()
        {
            HomeVM = null;
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeError");
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeSuccess");
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "CleanSignature");

            base.OnDisappearing();
        }
    }
}
