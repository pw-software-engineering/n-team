using AutoMapper;
using Server.Database.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Database.DataAccess
{

    public class HotelAccountDataAccess : IHotelAccountDataAccess
    {
        private IMapper mapper;
        private ServerDbContext dbContext;
        public HotelAccountDataAccess(ServerDbContext dbContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }
        public void AddPictures(List<string> pictures, int hotelId)
        {
            if (pictures == null)
                return;
            foreach(var pic in pictures)
            {
                var DBpic = new HotelPictureDb();
                DBpic.HotelID = hotelId;
                DBpic.Picture = pic;
                dbContext.HotelPictures.Add(DBpic);
            }
            dbContext.SaveChanges();
        }
        public void DeletePicteres(int hotelId)
        {

            var pic = dbContext.HotelPictures.Where(x => x.HotelID == hotelId);
            dbContext.HotelPictures.RemoveRange(pic);
            dbContext.SaveChanges();
        }

        public List<string> FindPictres(int hotelId)
        {
            var pics = dbContext.HotelPictures.Where(x => x.HotelID == hotelId).ToList().ConvertAll<string>(x => x.Picture);
            return pics;
        }

        //to get info about hotel
        public HotelGetInfo GetInfo(int hotelId)
        {
            
            var hotel = dbContext.HotelInfos.Find(hotelId);
            if (hotel == null)
                throw new Exception("reasores not fund");
            return mapper.Map<HotelGetInfo>(hotel);
        }

        //to update info about hotel
        public void UpdateInfo(int hotelId , HotelUpdateInfo hotelUpdateInfo)
        {
            if (hotelUpdateInfo == null)
                throw new NullReferenceException();
            var DBHotelInfo = mapper.Map<HotelInfoDb>(hotelUpdateInfo);
            DBHotelInfo.HotelID = hotelId;
            dbContext.HotelInfos.Update(DBHotelInfo);
            
            dbContext.SaveChanges();

        }
    }
}
