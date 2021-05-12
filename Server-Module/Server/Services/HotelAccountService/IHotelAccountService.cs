﻿using Server.RequestModels;
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
        public IServiceResult UpdateHotelInfo(int hotelId, HotelInfoUpdate hotelUpdateInfo);
        public IServiceResult GetHotelInfo(int hotelId);
    }
}
