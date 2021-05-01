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
        public DateTime? From { get; set; }

        [FromQuery]
        [Required]
        public DateTime? To { get; set; }

        [FromQuery]
        public int? MinGuests { get; set; }

        [FromQuery]
        public int? MinCost { get; set; }

        [FromQuery]
        public int? MaxCost { get; set; }
    }
}
