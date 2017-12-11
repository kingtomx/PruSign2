using PruSign.Background;
using PruSign.Data.Entities;
using PruSign.Data.ViewModels;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using PruSign.Data.Interfaces;
using PruSign.Data.Modules;
using Xamarin.Forms;

namespace PruSign
{
    public partial class App
    {
        public bool IsLocked { get; set; }
        public static IContainer Container;

        public App(IModule[] platformSpecificModules, SignatureViewModel initialData)
        {
            PrepareContainer(platformSpecificModules);
            InitializeComponent();
            MainPage = new LoadingPage();
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Checking if the credentials are stored in the database
                    using (Container.BeginLifetimeScope())
                    {
                        var serviceUserCredentials = Container.Resolve<IServiceAsync<UserCredentials>>();
                        var result = await serviceUserCredentials.GetAll().CountAsync();
                        if (result > 0)
                        {
                            MainPage = Container.Resolve<HomePage>(new TypedParameter(typeof(SignatureViewModel), initialData));
                        }
                        else
                        {
                            MainPage = Container.Resolve<LoginPage>();
                        }
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
                        MainPage = Container.Resolve<HomePage>(new TypedParameter(typeof(SignatureViewModel), initialData));
                    });
                }
            });
        }

        private static void PrepareContainer(IModule[] platformSpecificModules)
        {
            var containerBuilder = new ContainerBuilder();

            // Platform Specific Modules Loader
            RegisterPlatformSpecificModules(platformSpecificModules, containerBuilder);

            // Shared Modules Loader
            RegisterSharedModules(new IModule[] { new ModuleLoader() }, containerBuilder);

            Container = containerBuilder.Build();
        }

        private static void RegisterPlatformSpecificModules(IModule[] platformSpecificModules, ContainerBuilder containerBuilder)
        {
            foreach (var module in platformSpecificModules)
            {
                containerBuilder.RegisterModule(module);
            }
        }

        public static void RegisterSharedModules(IModule[] sharedModules, ContainerBuilder containerBuilder)
        {
            foreach (var module in sharedModules)
            {
                containerBuilder.RegisterModule(module);
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
