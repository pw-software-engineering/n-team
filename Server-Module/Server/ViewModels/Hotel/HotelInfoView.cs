using System.Collections.Generic;

namespace Server.ViewModels.Hotel
{
    public class HotelInfoView
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string HotelPreviewPicture { get; set; }
        public List<string> HotelPictures { get; set; }
    }
}
