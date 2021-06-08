using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System.Collections.Generic;

namespace Server.Database.DataAccess.Client
{
    public interface IHotelSearchDataAccess
    {
        #region api-client/hotels GET
        List<HotelPreviewView> GetHotels(HotelFilter hotelFilter, Paging paging);
        #endregion

        #region api-client/hotels/{hotelID} GET
        HotelView GetHotelDetails(int hotelID);
        List<string> GetHotelPictures(int hotelID);
        #endregion
        List<ReviewView> GetHotelReviews(int hotelID, Paging paging);
        bool DoesHotelExist(int hotelID);
    }
}
