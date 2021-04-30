using AutoMapper;
using Server.Database.Models;
using Server.Models;
using Server.RequestModels;
using Server.Services.HotelSearchService;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
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
        public List<HotelPreview> GetHotels(Paging paging, HotelFilter hotelFilter)
        {
            IQueryable<HotelInfoDb> ret = _dbContext.HotelInfos;

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
                     .Skip((paging.pageNumber - 1) * paging.pageSize)
                     .Take(paging.pageSize);

            return _mapper.Map<List<HotelPreview>>(ret.ToList());
        }

        public Hotel GetHotelDetails(int hotelID)
        {
            Hotel hotel = _mapper.Map<Hotel>(_dbContext.HotelInfos.Find(hotelID));
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
