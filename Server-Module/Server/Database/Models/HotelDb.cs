﻿using System.Collections.Generic;

namespace Server.Database.Models
{
    public class HotelDb
    {
        //Properties
        public int HotelID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelPreviewPicture { get; set; }
        public string HotelName { get; set; }
        public string HotelDescription { get; set; }
        public string AccessToken { get; set; }
        //Navigational Properties
        public List<HotelPictureDb> HotelPictures { get; set; }
        public List<OfferDb> Offers { get; set; }
        public List<HotelRoomDb> HotelRooms { get; set; }
        public List<ClientReviewDb> Reviews { get; set; }
    }
}
