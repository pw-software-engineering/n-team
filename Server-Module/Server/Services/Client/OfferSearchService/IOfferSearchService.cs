using Microsoft.AspNetCore.Mvc;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;

namespace Server.Services.Client
{
    public interface IOfferSearchService
    {
        #region /hotels/{hotelID}/offers
        public IServiceResult GetHotelOffers(int hotelID, OfferFilter offerFilter, Paging paging);
        #endregion

        #region /hotels/{hotelID}/offers/{offerID}
        public IServiceResult GetHotelOfferDetails(int hotelID, int offerID);
        public IServiceResult GetHotelOfferReviews(int hotelID, int offerID, Paging paging);
        #endregion
    }
}
