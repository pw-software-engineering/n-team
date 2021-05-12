using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class HotelRoomView
    {
        public int RoomID { get; set; }
        public string HotelRoomNumber { get; set; }
        public List<int> OfferID { get; set; }
    }
}
