using System.Collections.Generic;

namespace Server.RequestModels.Hotel
{
    public class HotelInfoUpdate
    {
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string HotelPreviewPicture { get; set; }
        public List<string> HotelPictures { get; set; }
    }
}
