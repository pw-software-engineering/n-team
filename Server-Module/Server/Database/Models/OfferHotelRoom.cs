using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class OfferHotelRoom
    {
        //Properties
        public int OfferID { get; set; }
        public int RoomID { get; set; }
        //Navigational Properties
        public Offer Offer { get; set; }
        public HotelRoom Room { get; set; }
    }
}
