using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module
{
    public static class ServerApiConfig
    {
        public static string BaseUrl { get; } = "https://localhost:5001";
        public static string TokenHeaderName { get; } = "x-client-token";
    }
}
