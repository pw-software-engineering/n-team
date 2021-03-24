using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.Models
{
    public class HotelOffersModel
    {
        [DisplayFormat(DataFormatString = "yyyy-mm-dd")]
        public DateTime? From { get; set; }

        [DisplayFormat(DataFormatString = "yyyy-mm-dd")]
        public DateTime? To { get; set; }
        public int? MinCost { get; set; }
        public int? MaxCost { get; set; }
        public int? MinGuests { get; set; }
    }
}
