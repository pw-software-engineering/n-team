﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IHotelTokenDataAccess
    {
        int? GetHotelIdFromToken(string HotelToken);
    }
}