using Autofac;
using PruSign.Data.Entities;
using PruSign.iOS.Data;

namespace PruSign.iOS
{
    public class PlatformSpecificModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<SQLiteIOS>().As<ISQLite>();
        }
    }
}