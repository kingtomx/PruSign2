﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SharedTests
{
    internal class MockPlatformServices : IPlatformServices
    {
        Action<Action> _invokeOnMainThread;
        Action<Uri> _openUriAction;
        Func<Uri, CancellationToken, Task<Stream>> _getStreamAsync;

        public MockPlatformServices(Action<Action> invokeOnMainThread = null, Action<Uri> openUriAction = null, Func<Uri, CancellationToken, Task<Stream>> getStreamAsync = null)
        {
            _invokeOnMainThread = invokeOnMainThread;
            _openUriAction = openUriAction;
            _getStreamAsync = getStreamAsync;
        }

        public string GetMD5Hash(string input)
        {
            throw new NotImplementedException();
        }

        public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
        {
            switch (size)
            {
                case NamedSize.Default:
                    return 10;
                case NamedSize.Micro:
                    return 4;
                case NamedSize.Small:
                    return 8;
                case NamedSize.Medium:
                    return 12;
                case NamedSize.Large:
                    return 16;
                default:
                    throw new ArgumentOutOfRangeException("size");
            }
        }

        public void OpenUriAction(Uri uri)
        {
            if (_openUriAction != null)
                _openUriAction(uri);
            else
                throw new NotImplementedException();
        }

        public bool IsInvokeRequired
        {
            get { return false; }
        }

        public string RuntimePlatform { get; set; }

        public void BeginInvokeOnMainThread(Action action)
        {
            if (_invokeOnMainThread == null)
                action();
            else
                _invokeOnMainThread(action);
        }

        public Ticker CreateTicker()
        {
            return new MockTicker();
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            Timer timer = null;
            TimerCallback onTimeout = o => BeginInvokeOnMainThread(() => {
                if (callback())
                    return;

                timer.Dispose();
            });
            timer = new Timer(onTimeout, null, interval, interval);
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (_getStreamAsync == null)
                throw new NotImplementedException();
            return _getStreamAsync(uri, cancellationToken);
        }

        public Assembly[] GetAssemblies()
        {
            return new Assembly[0];
        }

        public IIsolatedStorageFile GetUserStoreForApplication()
        {
            throw new NotImplementedException();
        }

        public void QuitApplication()
        {
            throw new NotImplementedException();
        }
    }

    internal class MockDeserializer : IDeserializer
    {
        public Task<IDictionary<string, object>> DeserializePropertiesAsync()
        {
            return Task.FromResult<IDictionary<string, object>>(new Dictionary<string, object>());
        }

        public Task SerializePropertiesAsync(IDictionary<string, object> properties)
        {
            return Task.FromResult(false);
        }
    }

    internal class MockResourcesProvider : ISystemResourcesProvider
    {
        public IResourceDictionary GetSystemResources()
        {
            var dictionary = new ResourceDictionary
            {
                ["PrudentialBlue"] = Color.FromHex("0679BD"),
                ["FaluRed"] = Color.FromHex("801515"),
                ["HippieGreen"] = Color.FromHex("5C8F49"),
                ["GreenHouse"] = Color.FromHex("22540F")
            };

            return dictionary;
        }
    }

    internal class MockTicker : Ticker
    {
        bool _enabled;

        protected override void EnableTimer()
        {
            _enabled = true;

            while (_enabled)
            {
                SendSignals(16);
            }
        }

        protected override void DisableTimer()
        {
            _enabled = false;
        }
    }

    internal class MockDeviceInfo : DeviceInfo
    {
        public override Size PixelScreenSize => throw new NotImplementedException();

        public override Size ScaledScreenSize => throw new NotImplementedException();

        public override double ScalingFactor => throw new NotImplementedException();
    }
}
