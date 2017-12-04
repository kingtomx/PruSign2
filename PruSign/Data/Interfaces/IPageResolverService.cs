using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PruSign.Data.Interfaces
{
    public interface IPageResolverService
    {
        Page GetPage(Type type);
    }
}
