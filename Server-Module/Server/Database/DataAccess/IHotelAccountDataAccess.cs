﻿using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IHotelAccountDataAccess
    {
        public HotelGetInfo GetInfo(int hotelId);
        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo);

        public int AddHotelInfo(HotelUpdateInfo hotelUpdateInfo);
    }
}
