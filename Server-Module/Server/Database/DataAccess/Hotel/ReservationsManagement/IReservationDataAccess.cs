using Server.RequestModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
{
    public interface IReservationDataAccess
    {
        ClientView GetClientDetails(int clientID);
        RoomView GetRoomDetails(int roomID);
        List<ReservationObjectView> GetReservations(int hotelID, int? roomID, bool currentOnly, Paging paging);
        int? FindRoomAndGetOwner(int roomID);
    }
}
