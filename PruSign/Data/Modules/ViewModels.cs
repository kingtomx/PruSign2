using Autofac;
using PruSign.Data.ViewModels;

namespace PruSign.Data.Modules
{
    public class ViewModels : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<HomeViewModel>().SingleInstance();
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            builder.RegisterType<LogViewModel>().SingleInstance();
        }
    }
}
