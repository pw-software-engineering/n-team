using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.HotelAccountService
{
    interface IHotelAccountService
    {
        public void UpdateInfo(int hotelId,HotelUpdateInfo hotelUpdateInfo);
        public HotelGetInfo GetInfo(int hotelId);
    }
}
