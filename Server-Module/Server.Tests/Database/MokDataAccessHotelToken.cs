using Server.Database.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Tests.Database
{
    class MokDataAccessHotelToken : IHotelTokenDataAccess
    {
        public int? GetHotelIdFromToken(string hotelToken)
        {
            if (hotelToken == "OnlyValidToken")
                return 1;
            return null;
        }
    }
}
