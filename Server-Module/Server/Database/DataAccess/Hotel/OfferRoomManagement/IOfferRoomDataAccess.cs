using Server.RequestModels;
using Server.ViewModels.Hotel;
using System.Collections.Generic;

namespace Server.Database.DataAccess.Hotel
{
    public interface IOfferRoomDataAccess
    {
        void AddRoomToOffer(int roomID, int offerID);
        List<OfferRoomView> GetOfferRooms(int offerID, Paging paging, string hotelRoomNumber);
        void UnpinRoomFromOffer(int roomID, int offerID);
        int? FindRoomAndGetOwner(string hotelRoomNumber);
        int? FindRoomAndGetOwner(int roomID);
        int? FindOfferAndGetOwner(int offerID);
        bool IsRoomAlreadyAddedToOffer(int roomID, int offerID);
        bool DoesRoomHaveUnfinishedReservations(int roomID, int offerID);
    }
}
