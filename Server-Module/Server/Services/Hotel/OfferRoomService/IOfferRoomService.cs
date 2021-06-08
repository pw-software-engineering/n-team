using Server.RequestModels;
using Server.Services.Result;

namespace Server.Services.Hotel
{
    public interface IOfferRoomService
    {
        public IServiceResult GetOfferRooms(int offerID, int hotelID, string hotelRoomNumber, Paging paging);
        public IServiceResult AddRoomToOffer(int roomID, int offerID, int hotelID);
        public IServiceResult RemoveRoomFromOffer(int roomID, int offerID, int hotelID);
    }
}
