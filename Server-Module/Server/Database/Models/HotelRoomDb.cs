using System.Collections.Generic;

namespace Server.Database.Models
{
    public class HotelRoomDb
    {
        //Properties
        public int RoomID { get; set; }
        public int HotelID { get; set; }
        public string HotelRoomNumber { get; set; }
        public bool IsActive { get; set; }
        //Navigational Properties
        public HotelDb Hotel { get; set; }
        public List<OfferHotelRoomDb> OfferHotelRooms { get; set; }
    }
}
