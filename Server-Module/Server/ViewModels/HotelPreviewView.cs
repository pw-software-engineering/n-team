using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class HotelPreviewView
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PreviewPicture { get; set; }
    }
}
