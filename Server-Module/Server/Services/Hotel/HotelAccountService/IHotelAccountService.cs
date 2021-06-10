using Server.RequestModels.Hotel;
using Server.Services.Result;

namespace Server.Services.Hotel
{
    public interface IHotelAccountService
    {
        public IServiceResult UpdateHotelInfo(int hotelId, HotelInfoUpdate hotelUpdateInfo);
        public IServiceResult GetHotelInfo(int hotelId);
    }
}
