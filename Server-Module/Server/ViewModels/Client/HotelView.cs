using System.Collections.Generic;

namespace Server.ViewModels.Client
{
    public class HotelView
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string HotelDescription { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public List<string> HotelPictures { get; set; }
    }
}
