using Server.RequestModels;
using Server.Services.Response;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.RoomService
{
    public interface IRoomService
    {
        IServiceResult GetHotelRooms(Paging paging, int hotelID, string hotelRoomNumber = null);
        IServiceResult AddRoom(int hotelID, string hotelRoomNumber);
        IServiceResult DeleteRoom(int hotelID, int roomID);
    }
}
