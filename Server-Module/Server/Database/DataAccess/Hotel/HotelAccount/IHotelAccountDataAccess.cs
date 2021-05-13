using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
