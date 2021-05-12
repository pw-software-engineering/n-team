using Server.RequestModels;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Client
{ 
    public interface IHotelSearchService
    {
        #region /hotels
        IServiceResult GetHotels(Paging paging, HotelFilter hotelFilter);
        #endregion

        #region /hotels/{hotelID}
        IServiceResult GetHotelDetails(int hotelID);
        #endregion
    }
}
