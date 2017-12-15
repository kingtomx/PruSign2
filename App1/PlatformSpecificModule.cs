using Autofac;
using PruSign.Data.Entities;
using PruSign.iOS.Data;

namespace App1
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