using Server.RequestModels;
using Server.Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Hotel
{
    public interface IReservationService
    {
        public IServiceResult GetReservations(int hotelID, bool? currentOnly, int? roomID, Paging paging);
    }
}
