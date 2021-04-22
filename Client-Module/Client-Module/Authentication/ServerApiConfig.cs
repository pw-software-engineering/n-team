using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.Authentication
{
    public static class ServerApiConfig
    {
        public const string BaseUrl = "https://localhost:5001/api";
        public const string TokenHeaderName = "x-client-token";
    }
}
