using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PruSign.Data.Interfaces
{
    public interface IModalService
    {
        void Push(INavigation navigation, Page page, Action callback);
    }
}
