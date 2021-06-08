using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Client
{
    public interface IOfferSearchService
    {
        #region /hotels/{hotelID}/offers
        public IServiceResult GetHotelOffers(int hotelID, OfferFilter offerFilter, Paging paging);
        #endregion

        #region /hotels/{hotelID}/offers/{offerID}
        public IServiceResult GetHotelOfferDetails(int hotelID, int offerID);
        //public IServiceResult GetHotelOfferAvailability(int hotelID, int offerID, DateTime from, DateTime to);
        public IServiceResult GetHotelOfferReviews(int hotelID, int offerID);
        #endregion

        public IServiceResult GetHotelReviews(int hotelID, int offerID, Paging paging);
    }
}
