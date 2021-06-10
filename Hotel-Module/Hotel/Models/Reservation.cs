using System;

namespace Hotel.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int OfferID { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public int ChildrenCount { get; set; }
        public int AdultsCount { get; set; }
    }
}
