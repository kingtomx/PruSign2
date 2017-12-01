using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace PruSign.Data.Modules
{
    public class ModuleLoader : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<Generics>();
            builder.RegisterModule<Services>();
            builder.RegisterModule<ContentPages>();
            builder.RegisterModule<ViewModels>();
        }
    }
}
