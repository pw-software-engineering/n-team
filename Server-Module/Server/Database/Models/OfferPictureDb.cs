using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class OfferPictureDb
    {
        //Properties
        public int OfferID { get; set; }
        public int PictureID { get; set; }
        public string Picture { get; set; }
        //Navigational Properties
        public OfferDb Offer { get; set; }
        public OfferPictureDb(string picture, int offerID)
        {
            Picture = picture;
            OfferID = offerID;
        }
    }
}
