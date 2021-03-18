using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class HotelInfo
    {
        //Properties
        public int HotelID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public byte[] HotelPreviewPicture { get; set; }
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string AccessToken { get; set; }
    }
}
