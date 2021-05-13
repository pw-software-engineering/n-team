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
    public interface IHotelSearchService
    {
        #region /hotels
        IServiceResult GetHotels(HotelFilter hotelFilter, Paging paging);
        #endregion

        #region /hotels/{hotelID}
        IServiceResult GetHotelDetails(int hotelID);
        #endregion
    }
}
