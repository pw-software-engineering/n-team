using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ClientOffer
    {
        [JsonIgnore]
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferDescription { get; set; }
        public int MaxGuests { get; set; }
        public double CostPerAdult { get; set; }
        public double CostPerChild { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> OfferPictures { get; set; }
    }
}
