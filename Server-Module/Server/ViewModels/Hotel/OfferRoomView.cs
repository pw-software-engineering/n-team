using System.Collections.Generic;

namespace Server.ViewModels.Hotel
{
    public class OfferRoomView
    {
        public int RoomID { get; set; }
        public string HotelRoomNumber { get; set; }
        public List<int> OfferID { get; set; }
    }
}
