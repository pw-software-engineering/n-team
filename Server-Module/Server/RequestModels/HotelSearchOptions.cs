using System;

namespace Server.RequestModels
{
    public class HotelSearchOptions
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string HotelName { get; set; }
    }
}
