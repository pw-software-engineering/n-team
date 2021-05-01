using AutoMapper;
using Server.Database.Models;
using Server.Models;
using Server.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.OfferSearch
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

        public List<ClientOfferPreview> GetHotelOffers(int hotelID, Paging paging, OfferFilter offerFilter)
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
            if(offerFilter.MaxCost.HasValue)
            {
                offers = offers.Where(o => Math.Max(o.CostPerAdult, o.CostPerChild) <= offerFilter.MaxCost);
            }
            if(offerFilter.MinCost.HasValue)
            {
                offers = offers.Where(o => Math.Min(o.CostPerAdult, o.CostPerChild) >= offerFilter.MinCost);
            }
            if(offerFilter.MinGuests.HasValue)
            {
                offers = offers.Where(o => o.MaxGuests >= offerFilter.MinGuests);
            }

            offers = offers.AsEnumerable();
            offers = offers.Where(o => CheckHotelOfferAvailability(o.OfferID, offerFilter.From.Value, offerFilter.To.Value))
                           .Skip((paging.pageNumber - 1) * paging.pageSize)
                           .Take(paging.pageSize);

            return _mapper.Map<List<ClientOfferPreview>>(offers.ToList());
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
        public ClientOffer GetHotelOfferDetails(int offerID)
        {
            return _mapper.Map<ClientOffer>(_dbContext.Offers.Find(offerID));
        }
        public List<string> GetHotelOfferPictures(int offerID)
        {
            if(_dbContext.Offers.Find(offerID) == null)
            {
                return null;
            }
            return _dbContext.OfferPictures.Where(odb => odb.OfferID == offerID).Select(odb => odb.Picture).ToList();
        }

        public bool CheckHotelOfferExistence(int hotelID, int offerID)
        {
            return _dbContext.Offers.Any(o => o.HotelID == hotelID && o.OfferID == offerID);
        }

        public bool CheckHotelExistence(int hotelID)
        {
            return _dbContext.HotelInfos.Any(h => h.HotelID == hotelID);
        }

    }
}
