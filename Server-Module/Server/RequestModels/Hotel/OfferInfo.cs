using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels.Hotel
{
    public class OfferInfo
    {
        public bool IsActive { get; set; }
        public string OfferTitle { get; set; }
        public double? CostPerChild { get; set; }
        public double? CostPerAdult { get; set; }
        public int? MaxGuests { get; set; }
        public string Description { get; set; }
        public string OfferPreviewPicture { get; set; }
        public List<string> Pictures { get; set; }
        public List<string> Rooms { get; set; }
    }
}
