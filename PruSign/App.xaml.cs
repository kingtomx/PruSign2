using PruSign.Background;
using PruSign.Data.Entities;
using PruSign.Data.ViewModels;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using PruSign.Data.Interfaces;
using PruSign.Data.Modules;
using Xamarin.Forms;
using System;
using Device = Xamarin.Forms.Device;

namespace PruSign
{
    public partial class App : Application
    {
        public bool IsLocked { get; set; }
        public static IContainer Container;
        private string _incomingData { get; set; }
        public App(IModule[] platformSpecificModules)
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
                            MainPage = Container.Resolve<HomePage>();
                        }
                        else
                        {
                            MainPage = Container.Resolve<LoginPage>();
                        }
                        MessagingCenter.Send(this, "startIOSBackgroundSync");
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
                        MainPage = Container.Resolve<HomePage>();
                    });
                }
            });

            // The IMEI will be stored the first time the user loads the application
            if (!App.Current.Properties.ContainsKey("IMEI"))
            {
                SetImei();
            }
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

        public void UpdateIncomingData()
        {
            MessagingCenter.Send(this, "showDataFromOtherApp");
        }

        public void HandleIncomingDataForAndroid(SignatureViewModel signatureData)
        {
            SetAppProperty("customerName", signatureData.customerName);
            SetAppProperty("customerId", signatureData.customerId);

            CheckIfApplicationIsOpen();
        }

        public void HandleIncomingDataForIOS(string url)
        {
            try
            {
                url = url.Split('?')[1];

                var incomingParams = url.Split('&');
                foreach (var param in incomingParams)
                {
                    var key = param.Split('=')[0];
                    var value = param.Split('=')[1];
                    // TO-DO Check why I need to decode two times. I think the error is in the sender app
                    value = System.Web.HttpUtility.UrlDecode(value);
                    value = System.Web.HttpUtility.UrlDecode(value);
                    SetAppProperty(key, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            CheckIfApplicationIsOpen();
        }

        public void CheckIfApplicationIsOpen()
        {
            // Check if the application is already open when the parameters are sent
            if (Properties.ContainsKey("isOpen"))
            {
                // TO-DO call Update
                UpdateIncomingData();
            }
        }

        public static void SetAppProperty(string key, string value)
        {
            App.Current.Properties[key] = value;
        }

        public void SetImei()
        {
            var deviceDetails = Container.Resolve<IUniqueID>();
            App.SetAppProperty("IMEI", deviceDetails.GetIdentifier());
        }

        public static string GetImei()
        {
            if (App.Current.Properties.ContainsKey("IMEI"))
            {
                return Current.Properties["IMEI"] as string;
            }
            return string.Empty;
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
