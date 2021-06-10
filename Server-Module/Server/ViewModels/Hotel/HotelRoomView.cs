using System.Collections.Generic;

namespace Server.ViewModels.Hotel
{
    public class HotelRoomView
    {
        public int RoomID { get; set; }
        public string HotelRoomNumber { get; set; }
        public List<int> OfferID { get; set; }
    }
}
