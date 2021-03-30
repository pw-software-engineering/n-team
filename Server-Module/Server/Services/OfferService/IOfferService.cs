using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.OfferService
{
    interface IOfferService
    {
        #region /offers
        public List<OfferPreviewView> GetHotelOffers(int hotelID);
        public int AddOffer(OfferView offer, int hotelID);
        #endregion

        #region /offers/{offerID}
        public void DeleteOffer(int offerID, int hotelID);
        public void UpdateOffer(int offerID, int hotelID, bool? isActive, string offerTitle, string description, string offerPreviewPicture, List<string> offerPictures);
        public OfferView GetOffer(int offerID, int hotelID);
        #endregion
    }
}
