using Server.RequestModels.Hotel;
using Server.ViewModels.Hotel;
using System.Collections.Generic;

namespace Server.Database.DataAccess.Hotel
{
    public interface IHotelAccountDataAccess
    {
        public void AddPictures(List<string> pictures,int hotelId);
        public void DeletePictures(int hotelId);
        public HotelInfoView GetHotelInfo(int hotelId);
        public void UpdateHotelInfo(int hotelId, HotelInfoUpdate hotelUpdateInfo);
        public List<string> GetPictures(int hotelId);
    }
}
