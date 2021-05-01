using Server.Database.Models;
using Server.Models;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.RequestModels;

namespace Server.Services.OfferService
{
    public interface IOfferService
    {
        #region /offers
        public IServiceResult GetHotelOffers(Paging paging, int hotelID, bool? isActive = null);
        public IServiceResult AddOffer(OfferView offer, int hotelID);
        #endregion

        #region /offers/{offerID}
        public IServiceResult DeleteOffer(int offerID, int hotelID);
        public IServiceResult UpdateOffer(int offerID, int hotelID, OfferUpdateInfo offerUpdateInfo);
        public IServiceResult GetOffer(int offerID, int hotelID);
        #endregion
    }
}
