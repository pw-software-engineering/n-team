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
            try
            {
                return hotelAccountDataAccess.GetInfo(hotelId);
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo)
        {
            try {
                hotelAccountDataAccess.UpdateInfo(hotelId, hotelUpdateInfo);
            }catch(NotFundExepcion e)
            {
                throw new Exception(e.Message);
            }
        }   
    }
}
