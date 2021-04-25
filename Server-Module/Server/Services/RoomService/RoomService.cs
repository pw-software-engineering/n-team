using Server.Services.Response;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.RoomService
{
    public class RoomService : IRoomService
    {
        public IServiceResult AddRoom(int hotelID, string hotelRoomNumber)
        {
            throw new NotImplementedException();
        }

        public IServiceResult DeleteRoom(int hotelID, int roomID)
        {
            throw new NotImplementedException();
        }

        public IServiceResult GetHotelRooms(Paging paging, int hotelID, string hotelRoomNumber = null)
        {
            throw new NotImplementedException();
        }
    }
}
