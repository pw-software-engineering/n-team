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
        public string Country { get; set; }
        public string City { get; set; }

        public HotelGetInfo():base()
        {
        }

        public HotelGetInfo(HotelInfoDb hotelInfoDb): base(hotelInfoDb)
        {
            Country = hotelInfoDb.Country;
            City = hotelInfoDb.City;
            
        }
    }
}
