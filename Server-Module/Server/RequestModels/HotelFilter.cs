using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels
{
    public class HotelFilter
    {
        [FromQuery]
        public string Country { get; set; }
        [FromQuery]
        public string City { get; set; }
        [FromQuery]
        public string HotelName { get; set; }
    }
}
