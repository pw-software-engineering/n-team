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
        List<HotelRoom> GetRooms(Paging paging, int hotelID);
        List<HotelRoom> GetRoomsWithRoomNumber(Paging paging, int hotelID, string roomNumber);
        void GetOffersForRooms(List<HotelRoom> hotelRooms);
        int AddRoom(int hotelID, string hotelRoomNumber);
        void DeleteRoom(int roomID);
        int? FindRoomAndGetOwner(int roomID);
    }
}
