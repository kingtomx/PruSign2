using System;
using PruSign.Data.Interfaces;
using Xamarin.Forms;
using Autofac;

namespace PruSign.Data.Services
{
    public class PageResolverService : IPageResolverService
    {
        public Page GetPage(Type type)
        {
            using (App.Container.BeginLifetimeScope())
            {
                return (Page)App.Container.Resolve(type);
            }
        }
    }
}
