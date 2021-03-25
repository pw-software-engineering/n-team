using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Interfaces
{
    interface IOfferService
    {
        #region /offers
        public List<OfferPreview> GetHotelOffers(int hotelID);
        public int AddOffer(Offer offer, int hotelID);
        #endregion

        #region /offers/{offerID}
        public bool DeleteOffer(int offerID);
        public bool UpdateOffer(int offerID, bool? isActive, string offerTitle, string description, string offerPreviewPicture, List<string> offerPictures);
        public Offer GetOffer(int offerID);
        #endregion
    }
}
