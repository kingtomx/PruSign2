using PruSign;
using PruSign.Background;
using PruSign.Data;
using PruSign.Data.Entities;
using PruSign.Data.ViewModels;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using PruSign.Data.Interfaces;
using PruSign.Data.Services;
using Xamarin.Forms;

namespace PruSign
{
    public partial class App : Application
    {
        public bool IsLocked { get; set; }
        public static IContainer Container;

        public App(IModule[] platformSpecificModules)
        {
            PrepareContainer(platformSpecificModules);
            InitializeComponent();
            MainPage = new LoadingPage();
            Task.Run(async () =>
            {
                // Checking if the credentials are stored in the database
                using (var scope = App.Container.BeginLifetimeScope())
                {
                    var db = Container.Resolve<IDBService>();
                    var userCredentialService = new ServiceAsync<UserCredentials>(db);
                    int result = await userCredentialService.GetAll().CountAsync();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (result > 0)
                        {
                            MainPage = Container.Resolve<HomePage>();
                        }
                        else
                        {
                            MainPage = Container.Resolve<LoginPage>();
                        }
                    });
                }
            });

            MessagingCenter.Subscribe<LoginViewModel>(this, "RedirectToHome", (sender) =>
            {
                if (!IsLocked)
                {
                    IsLocked = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MainPage = Container.Resolve<HomePage>();
                    });
                }
            });
        }

        private static void PrepareContainer(IModule[] platformSpecificModules)
        {
            var containerBuilder = new Autofac.ContainerBuilder();

            RegisterPlatformSpecificModules(platformSpecificModules, containerBuilder);

            containerBuilder.RegisterType<ModalService>().As<IModalService>().SingleInstance();
            containerBuilder.RegisterType<DeviceLogService>().As<IDeviceLogService>().SingleInstance();
            containerBuilder.RegisterType<SignatureService>().As<ISignatureService>().SingleInstance();
            containerBuilder.RegisterType<DBService>().As<IDBService>().SingleInstance();

            containerBuilder.RegisterType<HomePage>().SingleInstance();
            containerBuilder.RegisterType<LoginPage>().SingleInstance();
            containerBuilder.RegisterType<SettingsPage>().SingleInstance();
            containerBuilder.RegisterType<LogPage>().SingleInstance();

            containerBuilder.RegisterType<HomeViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>().SingleInstance();
            containerBuilder.RegisterType<SettingsViewModel>().SingleInstance();
            containerBuilder.RegisterType<LogViewModel>().SingleInstance();

            Container = containerBuilder.Build();
        }

        private static void RegisterPlatformSpecificModules(IModule[] platformSpecificModules, ContainerBuilder containerBuilder)
        {
            foreach (var platformSpecificModule in platformSpecificModules)
            {
                containerBuilder.RegisterModule(platformSpecificModule);
            }
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
