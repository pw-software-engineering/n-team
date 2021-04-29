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
        public string hotelName;
        public string hotelDesc;
        public string hotelPreviewPicture;
        public string[] hotelPictures;

        public HotelUpdateInfo() { }

        public HotelUpdateInfo(HotelInfoDb hotelInfoDb)
        {
            hotelDesc = hotelInfoDb.HotelDesc;
            hotelName = hotelInfoDb.HotelName;
            hotelPreviewPicture = hotelInfoDb.HotelPreviewPicture;
            if (hotelInfoDb.HotelPictures != null)
            {
                var pictureDbs = hotelInfoDb.HotelPictures.ToArray();
                List<string> pomString = new List<string>();
                foreach (var pic in pictureDbs)
                {
                    pomString.Add(pic.Picture);
                }
                hotelPictures = pomString.ToArray();
            }
            else
            {
                hotelPictures = null;
            }
        }
    }

}
