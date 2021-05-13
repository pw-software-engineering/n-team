using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Client
{
    public class Reservation
    {
        public int ClientID { get; set; }
        public int HotelID { get; set; }
        public int OfferID { get; set; }
        public int RoomID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfAdults { get; set; }
    }
}
