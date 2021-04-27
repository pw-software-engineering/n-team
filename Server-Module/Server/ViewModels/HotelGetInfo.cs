using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    // class used by HotelAccountControler to get info about hotel (endpoint /hotelInfo Get)
    public class HotelGetInfo: HotelUpdateInfo
    {
        public string country;
        public string city;

        public HotelGetInfo():base()
        {
        }

        public HotelGetInfo(HotelInfoDb hotelInfoDb): base(hotelInfoDb)
        {
            country = hotelInfoDb.Country;
            city = hotelInfoDb.City;
            
        }
    }
}
