using Server.Database.DataAccess;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.HotelAccountService
{
    public class HotelAccountService : IHotelAccountService
    {
        private IHotelAccountDataAccess hotelAccountDataAccess;

        public HotelAccountService(IHotelAccountDataAccess hotelAccountDataAccess)
        {
            this.hotelAccountDataAccess = hotelAccountDataAccess;
        }

        public HotelGetInfo GetInfo(int hotelId)
        {
            return hotelAccountDataAccess.GetInfo(hotelId);
        }

        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo)
        {
            hotelAccountDataAccess.UpdateInfo(hotelId, hotelUpdateInfo);
        }
    }
}
