using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Foundation;
using PruSign.Data.Entities;
using PruSign.iOS.Data;
using UIKit;

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