using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class HotelRoomView
    {
        public int roomID { get; set; }
        public string hotelRoomNumber { get; set; }
        public List<int> offerID { get; set; }
    }
}
