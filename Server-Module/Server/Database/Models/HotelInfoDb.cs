using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class HotelInfoDb
    {
        //Properties
        public int HotelID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelPreviewPicture { get; set; }
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string AccessToken { get; set; }
        //Navigational Properties
        public List<HotelPictureDb> HotelPictures { get; set; }
        public List<OfferDb> Offers { get; set; }
        public List<HotelRoomDb> HotelRooms { get; set; }
    }
}
