using System.Collections.Generic;

namespace Hotel.Models
{
    public class Offer
    {
        public uint OfferID { get; set; }
        public bool IsActive { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public uint MaxGuests { get; set; }
        public string OfferTitle { get; set; }
        public string Description { get; set; }
        public string OfferPreviewPicture { get; set; }
        public string[] Pictures { get; set; }
        public IEnumerable<string> Rooms { get; set; }
    }
}
