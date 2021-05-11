using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    // class used to in HotelAccountController to updet Info about hotel (endpoint /hotelIngo PATCH)
    public class HotelUpdateInfo
    {
        public string HotelName { get; set; }
        public string HotelDesc { get; set; }
        public string HotelPreviewPicture { get; set; }
        public List<string> HotelPictures { get; set; }

        public HotelUpdateInfo() { }

        public HotelUpdateInfo(HotelInfoDb hotelInfoDb)
        {
            HotelDesc = hotelInfoDb.HotelDescription;
            HotelName = hotelInfoDb.HotelName;
            HotelPreviewPicture = hotelInfoDb.HotelPreviewPicture;
            if (hotelInfoDb.HotelPictures != null)
            {
                var pictureDbs = hotelInfoDb.HotelPictures.ToArray();
                List<string> pomString = new List<string>();
                foreach (var pic in pictureDbs)
                {
                    pomString.Add(pic.Picture);
                }
                HotelPictures = pomString;
            }
            else
            {
                HotelPictures = null;
            }
        }
    }

}
