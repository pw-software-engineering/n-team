using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class OfferPicture
    {
        //Properties
        public int OfferID { get; set; }
        public int PictureID { get; set; }
        public byte[] Picture { get; set; }
    }
}
