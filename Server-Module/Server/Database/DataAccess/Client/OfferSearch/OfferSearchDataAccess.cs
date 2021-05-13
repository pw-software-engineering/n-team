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
    public class OfferSearchDataAccess : IOfferSearchDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public OfferSearchDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public List<OfferPreviewView> GetHotelOffers(int hotelID, OfferFilter offerFilter,  Paging paging)
        {
            if(paging == null)
            {
                throw new ArgumentNullException("paging");
            }
            if(offerFilter == null)
            {
                throw new ArgumentNullException("offerFilter");
            }

            IEnumerable<OfferDb> offers = _dbContext.Offers.Where(o => !o.IsDeleted && o.IsActive && o.HotelID == hotelID);
            if(offerFilter.CostMax.HasValue)
            {
                offers = offers.Where(o => Math.Max(o.CostPerAdult, o.CostPerChild) <= offerFilter.CostMax);
            }
            if(offerFilter.CostMin.HasValue)
            {
                offers = offers.Where(o => Math.Min(o.CostPerAdult, o.CostPerChild) >= offerFilter.CostMin);
            }
            if(offerFilter.MinGuests.HasValue)
            {
                offers = offers.Where(o => o.MaxGuests >= offerFilter.MinGuests);
            }

            offers = offers.AsEnumerable();
            offers = offers.Where(o => CheckHotelOfferAvailability(o.OfferID, offerFilter.FromTime.Value, offerFilter.ToTime.Value))
                           .Skip((paging.PageNumber - 1) * paging.PageSize)
                           .Take(paging.PageSize);

            return _mapper.Map<List<OfferPreviewView>>(offers.ToList());
        }

        public bool CheckHotelOfferAvailability(int offerID, DateTime from, DateTime to)
        {
            foreach(OfferHotelRoomDb offerRoom in _dbContext.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID))
            {
                if(_dbContext.ClientReservations.Where(cr => cr.RoomID == offerRoom.RoomID && !(cr.ToTime < from || cr.FromTime > to)).Any())
                {
                    continue;
                }
                return true;
            }
            return false;
        }
        public OfferView GetHotelOfferDetails(int offerID)
        {
            return _mapper.Map<OfferView>(_dbContext.Offers.Find(offerID));
        }
        public List<string> GetHotelOfferPictures(int offerID)
        {
            return _dbContext.OfferPictures.Where(odb => odb.OfferID == offerID).Select(odb => odb.Picture).ToList();
        }

        public bool CheckHotelOfferExistence(int hotelID, int offerID)
        {
            return _dbContext.Offers.Any(o => o.HotelID == hotelID && o.OfferID == offerID);
        }

        public bool CheckHotelExistence(int hotelID)
        {
            return _dbContext.Hotels.Any(h => h.HotelID == hotelID);
        }

    }
}
