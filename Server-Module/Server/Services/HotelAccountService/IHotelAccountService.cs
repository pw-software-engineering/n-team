using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.HotelAccountService
{
    public interface IHotelAccountService
    {
        public IServiceResult UpdateInfo(int hotelId,HotelUpdateInfo hotelUpdateInfo);
        public IServiceResult GetInfo(int hotelId);
    }
}
