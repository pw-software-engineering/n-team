using Server.ViewModels;
using System;

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
            if (hotelInfo == null)
            {
                throw new Exception("not fund");
            }
            return new HotelGetInfo(hotelInfo);
        }

        //to update info about hotel
        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo)
        {
            if (hotelUpdateInfo == null)
                throw new NullReferenceException();

            var hotel = dbContext.HotelInfos.Find(hotelId);
            if (hotel == null)
            {
                throw new Exception();
            }

            hotel.HotelName = hotelUpdateInfo.HotelName;
            if(hotelUpdateInfo.HotelPreviewPicture!=null)
                hotel.HotelPreviewPicture = hotelUpdateInfo.HotelPreviewPicture;
            hotel.HotelDescription = hotelUpdateInfo.HotelDesc;

            if (hotelUpdateInfo.HotelPictures != null)
            {
                hotel.HotelPictures.Clear();
                foreach (var pic in hotelUpdateInfo.HotelPictures)
                {
                    var pictureDb = new Models.HotelPictureDb();
                    pictureDb.Picture = pic;
                    pictureDb.HotelID = hotelId;
                    hotel.HotelPictures.Add(pictureDb);
                }
            }

            dbContext.SaveChanges();

        }
    }
}
