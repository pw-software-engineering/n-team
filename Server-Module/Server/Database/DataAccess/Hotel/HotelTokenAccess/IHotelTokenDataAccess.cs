﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
{
    public interface IHotelTokenDataAccess
    {
        int? GetHotelIdFromToken(string hotelToken);
    }
}