using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public class HotelAccountDataAccess : IHotelAccountDataAccess
    {
        private ServerDbContext dbContext;
        public HotelAccountDataAccess(ServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //to get info about hotel
        public HotelGetInfo GetInfo(int hotelId)
        {
            var hotelInfo = dbContext.HotelInfos.Find(hotelId);
            return new HotelGetInfo(hotelInfo);
        }

        //yo update info about hotel
        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var hotel = dbContext.HotelInfos.Find(hotelId);
                hotel.HotelName = hotelUpdateInfo.hotelName;
                hotel.HotelPreviewPicture = hotelUpdateInfo.hotelPreviewPicture;
                hotel.HotelDesc = hotelUpdateInfo.hotelDesc;

                foreach(var pic in hotelUpdateInfo.hotelPictures)
                {
                    var pictureDb= new Models.HotelPictureDb();
                    pictureDb.Picture = pic;
                    pictureDb.HotelID = hotelId;
                    hotel.HotelPictures.Add(pictureDb);
                }


                dbContext.SaveChanges();
                transaction.Commit();
            }
        }
    }
}
