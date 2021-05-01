using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels
{
    public class OfferSearchOptions
    {
        public int MinGuests { get; set; }
        public int CostMin { get; set; }
        public int CostMax { get; set; }
        public DataType FromTime { get; set; }
        public DataType ToTime { get; set; }
    }
}
