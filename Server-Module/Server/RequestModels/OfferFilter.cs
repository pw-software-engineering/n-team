using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels
{
    public class OfferFilter
    {
        [FromQuery]
        [Required]
        public DateTime? FromTime { get; set; }

        [FromQuery]
        [Required]
        public DateTime? ToTime { get; set; }

        [FromQuery]
        public int? MinGuests { get; set; }

        [FromQuery]
        public int? CostMin { get; set; }

        [FromQuery]
        public int? CostMax { get; set; }
    }
}
