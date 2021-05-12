using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class HotelPictureDb
    {
        //Properties
        public int HotelID { get; set; }
        public int PictureID { get; set; }
        public string Picture { get; set; }
        //Navigational Properties
        public HotelDb Hotel { get; set; }
    }
}
