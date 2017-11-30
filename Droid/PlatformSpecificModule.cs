using Autofac;
using PruSign.Data.Entities;
using PruSign.Droid.Data;

namespace PruSign.Droid
{
    public class PlatformSpecificModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<SQLiteAndroid>().As<ISQLite>();
        }
    }
}