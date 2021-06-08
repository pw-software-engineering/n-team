using Microsoft.AspNetCore.Mvc;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;

namespace Server.Services.Client
{ 
    public interface IHotelSearchService
    {
        #region /hotels
        IServiceResult GetHotels(HotelFilter hotelFilter, Paging paging);
        #endregion

        #region /hotels/{hotelID}
        IServiceResult GetHotelDetails(int hotelID);
        IActionResult GetHotelReviews(int hotelID, Paging paging);
        #endregion
    }
}
