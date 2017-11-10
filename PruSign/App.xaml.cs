using PruSign;
using PruSign.Background;
using PruSign.Data;
using PruSign.Data.Entities;
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
            MainPage = new LoadingPage();

            Task.Run(async () =>
            {
                // Checking if the credentials are stored in the database
                var db = new PruSignDatabase();
                var userCredentialService = new ServiceAsync<UserCredentials>(db);
                int result = await userCredentialService.GetAll().CountAsync();

                Device.BeginInvokeOnMainThread(() =>
                {
                    if(result > 0)
                    {
                        MainPage = new HomePage();
                    }
                    else
                    {
                        MainPage = new LoginPage();
                    }
                });
            });

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
            var startMessage = new StartDataSync();
            MessagingCenter.Send(startMessage, "StartDataSyncMessage");
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
