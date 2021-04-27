using Server.Database.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public class NotFundExepcion:Exception
    {
        public NotFundExepcion():base("element not fund") { }
    }

    public class HotelAccountDataAccess : IHotelAccountDataAccess
    {
        private ServerDbContext dbContext;
        public HotelAccountDataAccess(ServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int AddHotelInfo(HotelUpdateInfo hotelUpdateInfo)
        {
            var hotel = new HotelInfoDb();

            using (var transaction = dbContext.Database.BeginTransaction())
            {

                hotel.HotelName = hotelUpdateInfo.hotelName;
                hotel.HotelPreviewPicture = hotelUpdateInfo.hotelPreviewPicture;
                hotel.HotelDesc = hotelUpdateInfo.hotelDesc;

                foreach (var pic in hotelUpdateInfo.hotelPictures)
                {
                    var pictureDb = new Models.HotelPictureDb();
                    pictureDb.Picture = pic;
                    pictureDb.HotelID = hotel.HotelID;
                    hotel.HotelPictures.Add(pictureDb);
                }


                dbContext.SaveChanges();
                transaction.Commit();
            }
            return hotel.HotelID;
        }

        //to get info about hotel
        public HotelGetInfo GetInfo(int hotelId)
        {
            var hotelInfo = dbContext.HotelInfos.Find(hotelId);
            if(hotelInfo==null)
            {
                throw new NotFundExepcion();
            }
            return new HotelGetInfo(hotelInfo);
        }

        //to update info about hotel
        public void UpdateInfo(int hotelId, HotelUpdateInfo hotelUpdateInfo)
        {
            var hotel = dbContext.HotelInfos.Find(hotelId);
            if (hotel == null)
            {
                throw new NotFundExepcion();
            }
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                
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
