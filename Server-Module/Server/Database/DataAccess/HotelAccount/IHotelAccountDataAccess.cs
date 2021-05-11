using Server.Database.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IHotelAccountDataAccess
    {

        public void AddPictures(List<string> pictures,int hotelId);
        public void DeletePicteres(int hotelId);
        public HotelGetInfo GetInfo(int hotelId);
        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo);
        public List<string> FindPictres(int hotelId);

    }
}
