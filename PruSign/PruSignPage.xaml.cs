using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using PruSign.Data.ViewModels;

namespace PruSign
{
    public partial class PruSignPage : ContentPage
    {
        private HomeViewModel HomeVM { get; set; }
        public PruSignPage()
        {
            InitializeComponent();

            HomeVM = new HomeViewModel(Navigation);
            BindingContext = HomeVM;

            MessagingCenter.Subscribe<HomeViewModel, string>(this, "Error", (sender, arg) => {
                DisplayAlert("Error", (string)arg, "Cancel");
            });

            MessagingCenter.Subscribe<HomeViewModel>(this, "Success", (sender) => {
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
    }
}
