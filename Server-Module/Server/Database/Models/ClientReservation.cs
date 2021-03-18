using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class ClientReservation
    {
        //Properties
        public int ReservationID { get; set; }
        public int RoomID { get; set; }
        public int ClientID { get; set; }
        //ReservationInfo Properties
        public int HotelID { get; set; }
        public int OfferID { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public uint NumberOfChildren { get; set; }
        public uint NumberOfAdults { get; set; }
    }
}
