using Server.Database.Models;
using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.OfferService
{
    public interface IOfferService
    {
        #region /offers
        public List<OfferPreviewView> GetHotelOffers(int hotelID);
        public int AddOffer(OfferView offer, int hotelID);
        #endregion

        #region /offers/{offerID}
        public void DeleteOffer(int offerID, int hotelID);
        public void UpdateOffer(int offerID, int hotelID, OfferUpdateInfo offerUpdateInfo);
        public OfferView GetOffer(int offerID, int hotelID);
        #endregion
    }
}
