﻿using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class OfferView
    {
        public bool isActive { get; set; }
        public string offerTitle { get; set; }
        public double costPerChild { get; set; }
        public double costPerAdult { get; set; }
        public uint maxGuests { get; set; }
        public string description { get; set; }
        public string offerPreviewPicture { get; set; }
        public List<string> pictures { get; set; }
        public List<string> rooms { get; set; }
    }
}
