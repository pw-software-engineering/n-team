using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public interface IHotelSearchDataAccess
    {
        #region api-client/hotels GET
        List<HotelPreviewView> GetHotels(Paging paging, HotelFilter hotelFilter);
        #endregion

        #region api-client/hotels/{hotelID} GET
        HotelView GetHotelDetails(int hotelID);
        List<string> GetHotelPictures(int hotelID);
        #endregion
    }
}
