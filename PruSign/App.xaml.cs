using PruSign;
using PruSign.Data;
using PruSign.Data.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PruSign
{
    public partial class App : Application
    {
        public bool IsLocked { get; set; }

        public App()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                // Checking if the credentials are stored in the database
                var db = new PruSignDatabase();
                var userCredentialService = new ServiceAsync<UserCredentials>(db);
                int result = await userCredentialService.GetAll().CountAsync();

                Device.BeginInvokeOnMainThread(() =>
                {
                    if(result < 0)
                    {
                        MainPage = new HomePage();
                    }
                    else
                    {
                        MainPage = new LoginPage();
                    }
                });
            });

            MainPage = new LoginPage();

            MessagingCenter.Subscribe<LoginViewModel>(this, "RedirectToHome", (sender) =>
            {
                if (!IsLocked)
                {
                    IsLocked = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MainPage = new HomePage();
                    });
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
