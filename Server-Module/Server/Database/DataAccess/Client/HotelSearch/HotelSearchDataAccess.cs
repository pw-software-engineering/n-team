using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System.Collections.Generic;
using System.Linq;

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
                ret = ret.Where(hotel => hotel.HotelName.Contains(hotelFilter.HotelName));

            if (!string.IsNullOrEmpty(hotelFilter.City))
                ret = ret.Where(hotel => hotel.City.Contains(hotelFilter.City));

            if (!string.IsNullOrEmpty(hotelFilter.Country))
                ret = ret.Where(hotel => hotel.Country.Contains(hotelFilter.Country));

            ret = ret.OrderByDescending(hdb => hdb.HotelID)
                     .Skip((paging.PageNumber - 1) * paging.PageSize)
                     .Take(paging.PageSize);

            return _mapper.Map<List<HotelPreviewView>>(ret.ToList());
        }

        public HotelView GetHotelDetails(int hotelID)
        {
            return _mapper.Map<HotelView>(_dbContext.Hotels.Find(hotelID));
        }

        public List<string> GetHotelPictures(int hotelID)
        {
            return _dbContext.HotelPictures.Where(picture => picture.HotelID == hotelID)
                                           .Select(picture => picture.Picture)
                                           .ToList();
        }
        public List<ReviewView> GetHotelReviews(int hotelID, Paging paging)
        {
            List<ReviewView> reviewInfos = new List<ReviewView>();
            List<ClientReviewDb> reviews = _dbContext.ClientReviews
                                           .Where(cr => cr.HotelID == hotelID)
                                           .OrderByDescending(cr => cr.ReviewID)
                                           .Skip((paging.PageNumber - 1) * paging.PageSize)
                                           .Take(paging.PageSize)
                                           .ToList();
            foreach (ClientReviewDb review in reviews)
            {
                string clientName = _dbContext.Clients.Find(review.ClientID).Name;
                reviewInfos.Add(new ReviewView
                {
                    ReviewID = review.ReviewID,
                    Content = review.Content,
                    Rating = (int)review.Rating,
                    ReviewerUsername = clientName,
                    CreationDate = review.ReviewDate
                });
            }
            return reviewInfos;
        }

        public bool DoesHotelExist(int hotelID)
        {
            return !(_dbContext.Hotels.Find(hotelID) is null);
        }
    }
}
