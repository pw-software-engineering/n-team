using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class ClientReview
    {
        //Properties
        public int ReviewID { get; set; }
        public int ClientID { get; set; }
        //ReviewInfo Properties
        public int OfferID { get; set; }
        public string Content { get; set; }
        public uint Rating { get; set; }
        public DateTime ReviewData { get; set; }
        //Navigational Properties
        public Offer Offer { get; set; }
        public Client Client { get; set; }
    }
}
