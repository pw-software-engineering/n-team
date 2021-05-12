using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels
{
    public class HotelInfoUpdate
    {
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string HotelPreviewPicture { get; set; }
        public List<string> HotelPictures { get; set; }
    }
}
