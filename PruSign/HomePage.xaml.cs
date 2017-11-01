using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using PruSign.Data.ViewModels;

namespace PruSign
{
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
        }

        protected override void OnAppearing()
        {
            if(HomeVM != null)
            {
                // TO-DO Check if the user credentials are stored in the database

            }
            base.OnAppearing(); 
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeError");
            MessagingCenter.Unsubscribe<HomeViewModel>(this, "HomeSuccess");

            base.OnDisappearing();
        }
    }
}
