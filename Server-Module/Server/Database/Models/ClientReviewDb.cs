using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class ClientReviewDb
    {
        //Properties
        public int ReviewID { get; set; }
        public int ClientID { get; set; }
        //ReviewInfo Properties
        public int OfferID { get; set; }
        public int HotelID { get; set; }
        public string Content { get; set; }
        public uint Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        //Navigational Properties
        public OfferDb Offer { get; set; }
        public ClientDb Client { get; set; }
        public ClientReservationDb Reservation { get; set; }
        public HotelDb Hotel { get; set; }
    }
}
