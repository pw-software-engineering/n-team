using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class OfferPreviewView
    {
        public int OfferID { get; set; }
        public bool IsActive { get; set; }
        public string OfferTitle { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public uint MaxGuests { get; set; }
        public string OfferPreviewPicture { get; set; }
    }
}
