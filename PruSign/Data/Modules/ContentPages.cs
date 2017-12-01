using Autofac;

namespace PruSign.Data.Modules
{
    public class ContentPages : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<HomePage>().SingleInstance();
            builder.RegisterType<LoginPage>().SingleInstance();
            builder.RegisterType<SettingsPage>().SingleInstance();
            builder.RegisterType<LogPage>().SingleInstance();
        }
    }
}
