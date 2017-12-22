using System;
using System.Collections.Generic;
using System.Text;

namespace PruSign
{
    public static class Constants
    {
        public const string BACKEND_HOST_NAME = "http://192.168.0.13";
        //public const string BACKEND_HOST_NAME = "https://api.prudentialseguros.com.ar:4043/prusign";

        // Minimum number of seconds between a background refresh on iOS
        // 10 minutes = 10 * 60 = 600 seconds
        public const int BACKGROUND_SEND_INTERVAL = 900;

        public static class MessageKeys
        {
            public const string CleanSignature = "ClearSignature";
        }
    }
}
