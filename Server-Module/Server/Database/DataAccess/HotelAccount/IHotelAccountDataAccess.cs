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

        public void AddPictures(HotelInfoDb hotelInfoDb);
        public void DeletePicteres(HotelInfoDb hotelInfoDb);
        public HotelInfoDb GetInfo(int hotelId);
        public void UpdateInfo( HotelInfoDb hotelUpdateInfo);
        public List<string> FindPictres(int hotelId);

    }
}
