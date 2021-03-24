﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class AvalaibleTimeInterval
    {
        public int TimeIntervalID { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public int OfferID { get; set; }
        //Navigational Properties
        public Offer Offer { get; set; }
    }
}