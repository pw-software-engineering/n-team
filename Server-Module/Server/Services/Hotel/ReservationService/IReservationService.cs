using Server.RequestModels;
using Server.Services.Result;

namespace Server.Services.Hotel
{
    public interface IReservationService
    {
        public IServiceResult GetReservations(int hotelID, bool? currentOnly, int? roomID, Paging paging);
    }
}