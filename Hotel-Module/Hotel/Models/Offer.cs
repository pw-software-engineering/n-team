using Microsoft.AspNetCore.Http;

namespace Hotel.Models
{
    //skeleton
    public class Offer
    {
        public uint OfferID { get; set; }
        public bool IsActive { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public uint MaxGuests { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PreviewPicture { get; set; }
        public string[] Pictures { get; set; }
        //public Room[] Rooms { get; set; }
    }
}
