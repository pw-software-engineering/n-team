using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IRoomDataAccess
    {
        #region /rooms GET
        List<HotelRoom> GetRooms(Paging paging, int hotelID, string roomNumber = null);
        void GetOffersForRooms(List<HotelRoom> hotelRooms);
        #endregion

        #region /rooms POST
        bool DoesRoomAlreadyExist(int hotelID, string hotelRoomNumber);
        int AddRoom(int hotelID, string hotelRoomNumber);
        #endregion

        #region /rooms/{roomID} DELETE
        void DeleteRoom(int roomID);
        void ChangeActivationMark(int roomID, bool mark);
        bool DoesRoomHaveAnyUnfinishedReservations(int roomID);
        void UnpinRoomFromAnyOffers(int roomID);
        void RemoveRoomFromPastReservations(int roomID);
        #endregion
        int? FindRoomAndGetOwner(int roomID);
    }
}
