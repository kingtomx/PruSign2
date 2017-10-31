using PruSign;
using PruSign.Data.ViewModels;
using Xamarin.Forms;

namespace PruSign
{
	public partial class App : Application
	{
        public bool IsLocked { get; set; }

        public App()
		{
			InitializeComponent();

			MainPage = new PruSignPage();

            MessagingCenter.Subscribe<LoginViewModel>(this, "RedirectToHome", (sender) =>
            {
                if (!IsLocked)
                {
                    IsLocked = true;
                    MainPage = new PruSignPage();
                }
            });
        }

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
