using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
{
    public class OfferDataAccess : IOfferDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public OfferDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public void DeleteOffer(int offerID)
        {
            OfferDb offer = _dbContext.Offers.Find(offerID);
            offer.IsDeleted = true;
            _dbContext.SaveChanges();
        }
        public int AddOffer(int hotelID, OfferInfo offer)
        {
            OfferDb offerDb = _mapper.Map<OfferDb>(offer);
            offerDb.HotelID = hotelID;
            _dbContext.Offers.Add(offerDb);
            _dbContext.SaveChanges();

            return offerDb.OfferID;
        }
        public List<OfferPreviewView> GetHotelOffers(int hotelID, Paging paging, bool? isActive)
        {
            var ret = _mapper.Map<List<OfferPreviewView>>(_dbContext.Offers
                             .Where(o => o.HotelID == hotelID && !o.IsDeleted));

            if (isActive.HasValue)
                ret = ret.Where(o => o.IsActive == isActive).ToList();

            return ret.Skip((paging.PageNumber - 1) * paging.PageSize)
                      .Take(paging.PageSize)
                      .ToList();
        }

        public OfferView GetOffer(int offerID)
        {
            OfferDb offer = _dbContext.Offers.Find(offerID);
            if (offer is null || offer.IsDeleted)
                return null;
            return _mapper.Map<OfferView>(_dbContext.Offers.Find(offerID));
        }
        public int? FindOfferAndGetOwner(int offerID)
        {
            OfferDb offer = _dbContext.Offers.Find(offerID);
            if (offer is null || offer.IsDeleted)
                return null;
            return offer.HotelID;
        }
        public void UpdateOffer(int offerID, OfferInfoUpdate offerInfoUpdate)
        {
            OfferDb offer = _dbContext.Offers.Find(offerID);
            offer.IsActive = offerInfoUpdate.IsActive ?? offer.IsActive;
            offer.OfferTitle = offerInfoUpdate.OfferTitle ?? offer.OfferTitle;
            offer.Description = offerInfoUpdate.Description ?? offer.Description;
            offer.OfferPreviewPicture = offerInfoUpdate.OfferPreviewPicture ?? offer.OfferPreviewPicture;
            if (!(offerInfoUpdate.OfferPictures == null))
            {
                _dbContext.OfferPictures.RemoveRange(_dbContext.OfferPictures.Where(p => p.OfferID == offerID));
                foreach (string picture in offerInfoUpdate.OfferPictures)
                    _dbContext.OfferPictures.Add(new OfferPictureDb(picture, offerID));
            }
            _dbContext.SaveChanges();
        }

        public void AddOfferPictures(int offerID, List<string> pictures)
        {
            List<OfferPictureDb> picturesDb = new List<OfferPictureDb>();
            foreach (string picture in pictures)
                picturesDb.Add(new OfferPictureDb(picture, offerID));

            _dbContext.OfferPictures.AddRange(picturesDb);
            _dbContext.SaveChanges();
        }

        public bool AreThereUnfinishedReservationsForOffer(int offerID)
        {
            return _dbContext.ClientReservations.Any(cr => cr.OfferID == offerID && cr.ToTime > DateTime.Now);
        }

        public void UnpinRoomsFromOffer(int offerID)
        {
            IQueryable<OfferHotelRoomDb> roomsToUnpin = _dbContext.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID);
            _dbContext.OfferHotelRooms.RemoveRange(roomsToUnpin);
            _dbContext.SaveChanges();
        }

        public List<string> GetOfferRooms(int offerID)
        {
            return _dbContext.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID)
                                             .Select(ohr => ohr.Room.HotelRoomNumber)
                                             .ToList();
        }

        public List<string> GetOfferPictures(int offerID)
        {
            return _dbContext.OfferPictures.Where(op => op.OfferID == offerID)
                                           .Select(op => op.Picture)
                                           .ToList();
        }
    }
}
