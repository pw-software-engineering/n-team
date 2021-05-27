using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Module.Authentication
{
    public static class ServerApiConfig
    {
        public static string BaseUrl { get; set; } = "https://localhost:6001/api-hotel";
        public static string TokenHeaderName { get; set; } = "x-hotel-token";
    }
}
