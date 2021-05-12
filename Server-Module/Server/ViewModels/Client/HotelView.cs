﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels.Client
{
    public class HotelView
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string HotelDescription { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public List<string> HotelPictures { get; set; }
    }
}
