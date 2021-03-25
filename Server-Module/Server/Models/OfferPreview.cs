using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class OfferPreview
    {
        public bool isActive { get; set; }
        public string offerTitle { get; set; }
        public double costPerChild { get; set; }
        public double costPerAdult { get; set; }
        public uint maxGuests { get; set; }
        public string offerPreviewPicture { get; set; }
        public OfferPreview(OfferDb offer)
        {
            isActive = offer.IsActive;
            offerTitle = offer.OfferTitle;
            costPerChild = offer.CostPerChild;
            costPerAdult = offer.CostPerAdult;
            maxGuests = offer.MaxGuests;
            offerPreviewPicture = offer.OfferPreviewPicture;
        }
    }
}
