using System;
using Xamarin.Forms;

namespace PruSign.Data.Interfaces
{
    public interface IModalService
    {
        void Push(INavigation navigation, Type type, Action callback);
    }
}
