using Server.RequestModels;
using Server.Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Hotel
{
    public interface IOfferRoomService
    {
        public IServiceResult GetOfferRooms(int offerID, int hotelID, string hotelRoomNumber, Paging paging);
        public IServiceResult AddRoomToOffer(int roomID, int offerID, int hotelID);
        public IServiceResult RemoveRoomFromOffer(int roomID, int offerID, int hotelID);
    }
}
