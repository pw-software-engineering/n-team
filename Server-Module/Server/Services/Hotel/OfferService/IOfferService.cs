using Server.Database.Models;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.RequestModels;
using Server.RequestModels.Hotel;

namespace Server.Services.Hotel
{
    public interface IOfferService
    {
        #region /offers
        public IServiceResult GetHotelOffers(int hotelID, Paging paging, bool? isActive = null);
        public IServiceResult AddOffer(int hotelID, OfferInfo offerInfo);
        #endregion

        #region /offers/{offerID}
        public IServiceResult DeleteOffer(int hotelID, int offerID);
        public IServiceResult UpdateOffer(int hotelID, int offerID, OfferInfoUpdate offerInfoUpdate);
        public IServiceResult GetOffer(int hotelID, int offerID);
        #endregion
    }
}
