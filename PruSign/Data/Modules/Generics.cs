﻿using Autofac;
using PruSign.Data.Interfaces;
using PruSign.Data.Services;

namespace PruSign.Data.Modules
{
    public class Generics : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterGeneric(typeof(ServiceAsync<>)).As(typeof(IServiceAsync<>))
                .InstancePerDependency();
        }
    }
}
