using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IRoomDataAccess
    {
        List<RoomView> GetRooms(int hotelID, string hotelRoomNumber = null);
        int AddRoom(int hotelID, string hotelRoomNumber);
        void DeleteRoom(int hotelID, int roomID)
    }
}
