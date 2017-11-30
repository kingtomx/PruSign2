using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PruSign.Data.Interfaces;
using Xamarin.Forms;

namespace PruSign.Data.Services
{
    public class ModalService : IModalService
    {
        public void Push(INavigation navigation, Page page, Action callback)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await navigation.PushModalAsync(page);
                await Task.Delay(50); // This sucks, but it also prevents multiple taps.
                callback();
            });
        }
    }
}
