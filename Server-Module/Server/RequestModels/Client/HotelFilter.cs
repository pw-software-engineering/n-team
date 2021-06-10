using Microsoft.AspNetCore.Mvc;

namespace Server.RequestModels.Client
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
