using Server.RequestModels;
using Server.ViewModels.Hotel;
using System.Collections.Generic;

namespace Server.Database.DataAccess.Hotel
{
    public interface IReservationDataAccess
    {
        List<ReservationObjectView> GetReservations(int hotelID, int? roomID, bool? currentOnly, Paging paging);
        int? FindRoomAndGetOwner(int roomID);
    }
}
