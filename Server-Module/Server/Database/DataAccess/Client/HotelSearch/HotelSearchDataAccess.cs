using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public class HotelSearchDataAccess : IHotelSearchDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public HotelSearchDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public List<HotelPreviewView> GetHotels(HotelFilter hotelFilter, Paging paging)
        {
            IQueryable<HotelDb> ret = _dbContext.Hotels;

            if (!string.IsNullOrEmpty(hotelFilter.HotelName))
            {
                ret = ret.Where(hotel => hotel.HotelName.Contains(hotelFilter.HotelName));
            }
            if (!string.IsNullOrEmpty(hotelFilter.City))
            {
                ret = ret.Where(hotel => hotel.City.Contains(hotelFilter.City));
            }
            if (!string.IsNullOrEmpty(hotelFilter.Country))
            {
                ret = ret.Where(hotel => hotel.Country.Contains(hotelFilter.Country));
            }

            ret = ret.OrderBy(hdb => hdb.HotelID)
                     .Skip((paging.PageNumber - 1) * paging.PageSize)
                     .Take(paging.PageSize);

            return _mapper.Map<List<HotelPreviewView>>(ret.ToList());
        }

        public HotelView GetHotelDetails(int hotelID)
        {
            HotelView hotel = _mapper.Map<HotelView>(_dbContext.Hotels.Find(hotelID));
            return hotel;
        }

        public List<string> GetHotelPictures(int hotelID)
        {
            return _dbContext.HotelPictures.Where(picture => picture.HotelID == hotelID)
                                           .Select(picture => picture.Picture)
                                           .ToList();
        }
    }
}
