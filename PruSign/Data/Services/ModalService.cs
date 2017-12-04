using System;
using System.Threading.Tasks;
using PruSign.Data.Interfaces;
using Xamarin.Forms;

namespace PruSign.Data.Services
{
    public class ModalService : IModalService
    {
        private readonly IPageResolverService _pageResolverService;
        public ModalService(IPageResolverService pageResolverService)
        {
            _pageResolverService = pageResolverService;
        }

        public void Push(INavigation navigation, Type type, Action callback)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = _pageResolverService.GetPage(type);
                await navigation.PushModalAsync(page);
                await Task.Delay(50); // This sucks, but it also prevents multiple taps.
                callback();
            });
        }
    }
}
