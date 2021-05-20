using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.Models
{
    public class HotelOfferDetailsModel
    {
        public int HotelID { get; set; }
        public int OfferID { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
    }
}
