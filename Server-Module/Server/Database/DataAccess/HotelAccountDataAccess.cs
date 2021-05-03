using Server.ViewModels;
using System;

namespace Server.Database.DataAccess
{
    public class NotFundExepcion : Exception
    {
        public NotFundExepcion() : base("element not fund") { }
    }

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
                throw new NotFundExepcion();
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
                throw new NotFundExepcion();
            }

            hotel.HotelName = hotelUpdateInfo.hotelName;
            hotel.HotelPreviewPicture = hotelUpdateInfo.hotelPreviewPicture;
            hotel.HotelDescription = hotelUpdateInfo.hotelDesc;

            if (hotelUpdateInfo.hotelPictures != null)
            {
                hotel.HotelPictures.Clear();
                foreach (var pic in hotelUpdateInfo.hotelPictures)
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
