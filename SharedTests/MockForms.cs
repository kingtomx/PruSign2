using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SharedTests
{
    public class MockForms
    {

        public static void Init()
        {
            Device.Info = new MockDeviceInfo();
            Device.PlatformServices = new MockPlatformServices();

            DependencyService.Register<MockResourcesProvider>();
            DependencyService.Register<MockDeserializer>();
        }
    }
}
