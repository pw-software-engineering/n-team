using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Database.DataAccess.Hotel
{
    public class HotelAccountDataAccess : IHotelAccountDataAccess
    {
        private IMapper _mapper;
        private ServerDbContext _dbContext;
        public HotelAccountDataAccess(ServerDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public void AddPictures(List<string> pictures, int hotelId)
        {
            if (pictures == null)
                throw new ArgumentNullException("pictures");
            foreach(string picture in pictures)
            {
                HotelPictureDb pictureDb = new HotelPictureDb();
                pictureDb.HotelID = hotelId;
                pictureDb.Picture = picture;
                _dbContext.HotelPictures.Add(pictureDb);
            }
            _dbContext.SaveChanges();
        }
        public void DeletePictures(int hotelId)
        {
            IEnumerable<HotelPictureDb> pictures = _dbContext.HotelPictures.Where(x => x.HotelID == hotelId);
            _dbContext.HotelPictures.RemoveRange(pictures);
            _dbContext.SaveChanges();
        }

        public List<string> GetPictures(int hotelId)
        {
            return _dbContext.HotelPictures.Where(x => x.HotelID == hotelId).Select(x => x.Picture).ToList();
        }

        public HotelInfoView GetHotelInfo(int hotelId)
        { 
            return _mapper.Map<HotelInfoView>(_dbContext.Hotels.Find(hotelId));
        }
        public void UpdateHotelInfo(int hotelId, HotelInfoUpdate hotelInfoUpdate)
        {
            if (hotelInfoUpdate == null)
                throw new ArgumentNullException("hotelUpdateInfo");
            HotelDb hotelDb = _dbContext.Hotels.Find(hotelId);
            hotelDb.HotelDescription = hotelInfoUpdate.HotelDesc ?? hotelDb.HotelDescription;
            hotelDb.HotelPreviewPicture = hotelInfoUpdate.HotelPreviewPicture ?? hotelDb.HotelPreviewPicture;
            hotelDb.HotelName = hotelInfoUpdate.HotelName ?? hotelDb.HotelName;
            _dbContext.SaveChanges();
        }
    }
}
