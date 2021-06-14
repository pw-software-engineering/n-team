using System.Collections.Generic;

namespace Hotel.Models
{
    public class HotelInfo
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string HotelPreviewPicture { get; set; }
        public IEnumerable<string> HotelPictures { get; set; }
    }
}
