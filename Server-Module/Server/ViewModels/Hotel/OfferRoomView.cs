using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels.Hotel
{
    public class OfferRoomView
    {
        public int RoomID { get; set; }
        public string HotelRoomNumber { get; set; }
        public List<int> OfferID { get; set; }
    }
}
