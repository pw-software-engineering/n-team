using Server.RequestModels;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
{
    public interface IRoomDataAccess
    {
        #region /rooms GET
        List<HotelRoomView> GetRooms(int hotelID, Paging paging, string roomNumber = null);
        List<int> GetOfferIDsForRoom(int roomID);
        #endregion

        #region /rooms POST
        bool DoesRoomAlreadyExist(int hotelID, string hotelRoomNumber);
        int AddRoom(int hotelID, string hotelRoomNumber);
        #endregion

        #region /rooms/{roomID} DELETE
        void DeleteRoom(int roomID);
        void ChangeActivationMark(int roomID, bool activeState);
        bool CheckAnyUnfinishedReservations(int roomID);
        void UnpinRoomFromAnyOffers(int roomID);
        void RemoveRoomFromPastReservations(int roomID);
        #endregion
        int? FindRoomAndGetOwner(int roomID);
    }
}
