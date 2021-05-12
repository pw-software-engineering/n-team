using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class HotelInfoView
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string HotelPreviewPicture { get; set; }
        public List<string> HotelPictures { get; set; }
    }
}
