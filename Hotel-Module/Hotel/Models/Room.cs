using System.Collections.Generic;

namespace Hotel.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string HotelRoomNumber { get; set; }
        public IEnumerable<int> OfferID { get; set; }
    }
}
