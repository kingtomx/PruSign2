using Autofac;
using PruSign.Data.Interfaces;
using PruSign.Data.Services;

namespace PruSign.Data.Modules
{
    public class Services : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<DBService>().As<IDBService>().SingleInstance();
            builder.RegisterType<ModalService>().As<IModalService>().SingleInstance();
            builder.RegisterType<DeviceLogService>().As<IDeviceLogService>().SingleInstance();
            builder.RegisterType<SignatureService>().As<ISignatureService>().SingleInstance();
            builder.RegisterType<PageResolverService>().As<IPageResolverService>().SingleInstance();
        }
    }
}
