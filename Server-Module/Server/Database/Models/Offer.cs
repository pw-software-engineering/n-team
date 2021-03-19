using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class Offer
    {
        //Properties
        public int OfferID { get; set; }
        public int HotelID { get; set; }
        public string Title { get; set; }
        public byte[] OfferPreviewPicture { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<AvalaibleTimeInterval> AvalaibleTimeIntervals { get; set; }
        //OfferInfo Properties
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public uint MaxGuests { get; set; }
        public string Description { get; set; }
    }

    public class AvalaibleTimeInterval
    {
        public int TimeIntervalID { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
    }
}
