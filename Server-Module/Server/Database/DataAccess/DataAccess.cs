using AutoMapper;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public class DataAccess:IDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public DataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public void AddOffer(Offer offer)
        {
            _dbContext.Offers.Add(_mapper.Map<OfferDb>(offer));
            _dbContext.SaveChanges();
        }

        public List<Offer> GetHotelOffers(int hotelID)
        {
            return _mapper.Map<List<Offer>>(_dbContext.Offers.Where(o => o.HotelID == hotelID).ToList());
        }

        public Offer GetOffer(int offerID)
        {
            return _mapper.Map<Offer>(_dbContext.Offers.Find(offerID));
        }

        public void UpdateOffer(OfferUpdateInfo offer)//Visitor?
        {
        }

        public void AddOfferPicture(OfferPicture picture)
        {
            _dbContext.OfferPictures.Add(_mapper.Map<OfferPictureDb>(picture));
            _dbContext.SaveChanges();
        }
    }
}
