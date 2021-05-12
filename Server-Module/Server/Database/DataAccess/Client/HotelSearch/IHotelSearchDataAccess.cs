using Server.Models;
using Server.RequestModels;
using Server.Services.HotelSearchService;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public interface IHotelSearchDataAccess
    {
        #region api-client/hotels GET
        List<HotelPreview> GetHotels(Paging paging, HotelFilter hotelFilter);
        #endregion

        #region api-client/hotels/{hotelID} GET
        Hotel GetHotelDetails(int hotelID);
        List<string> GetHotelPictures(int hotelID);
        #endregion
    }
}
