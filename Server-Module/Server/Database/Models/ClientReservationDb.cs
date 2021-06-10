using System;

namespace Server.Database.Models
{
    public class ClientReservationDb
    {
        //Properties
        public int ReservationID { get; set; }
        public int? RoomID { get; set; }
        public int ClientID { get; set; }
        public int? ReviewID { get; set; }
        //ReservationInfo Properties
        public int HotelID { get; set; }
        public int OfferID { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfAdults { get; set; }
        //Navigational Properties
        public HotelDb Hotel { get; set; }
        public OfferDb Offer { get; set; }
        public ClientDb Client { get; set; }
        public HotelRoomDb Room { get; set; }
        public ClientReviewDb Review { get; set; }
    }
}
