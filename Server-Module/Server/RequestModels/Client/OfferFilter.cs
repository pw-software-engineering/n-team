using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Server.RequestModels.Client
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
