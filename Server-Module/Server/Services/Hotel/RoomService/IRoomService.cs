using Server.RequestModels;
using Server.Services.Result;

namespace Server.Services.Hotel
{
    public interface IRoomService
    {
        IServiceResult GetHotelRooms(int hotelID, Paging paging, string hotelRoomNumber = null);
        IServiceResult AddRoom(int hotelID, string hotelRoomNumber);
        IServiceResult DeleteRoom(int hotelID, int roomID);
    }
}
