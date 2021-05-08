using Server.Database.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Database.DataAccess
{

    public class HotelAccountDataAccess : IHotelAccountDataAccess
    {
        private ServerDbContext dbContext;
        public HotelAccountDataAccess(ServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddPictures(HotelInfoDb hotelInfoDb)
        {
            if (hotelInfoDb == null || hotelInfoDb.HotelPictures == null)
                return;
            dbContext.HotelPictures.AddRange(hotelInfoDb.HotelPictures);
            dbContext.SaveChanges();
        }
        public void DeletePicteres(HotelInfoDb hotelInfoDb)
        {
            if (hotelInfoDb == null ||hotelInfoDb.HotelPictures==null)
                return;
            var pic = dbContext.HotelPictures.Find(hotelInfoDb.HotelPictures);
            dbContext.HotelPictures.RemoveRange(pic);
            dbContext.SaveChanges();
        }

        public List<string> FindPictres(int hotelId)
        {
            var pom = dbContext.HotelPictures.AsEnumerable().Where(x => x.HotelID == hotelId);
            List<string> wynik = new List<string>();
            foreach (var p in pom)
            {
                wynik.Add(p.Picture);
            }
            return wynik;
        }

        //to get info about hotel
        public HotelInfoDb GetInfo(int hotelId)
        {
            var hotel = dbContext.HotelInfos.Find(hotelId);
            return hotel;
        }

        //to update info about hotel
        public void UpdateInfo(HotelInfoDb hotelUpdateInfo)
        {
            if (hotelUpdateInfo == null)
                throw new NullReferenceException();

            //if (dbContext.HotelInfos.Find(hotelUpdateInfo.HotelID) == null)
              //  throw new Exception("key not in the database");
            dbContext.HotelInfos.Update(hotelUpdateInfo);
            
            dbContext.SaveChanges();

        }
    }
}
