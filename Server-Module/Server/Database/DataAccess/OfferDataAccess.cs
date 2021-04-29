using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
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
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                OfferDb offer = _dbContext.Offers.Find(offerID);
                offer.IsDeleted = true;
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }
        public int AddOffer(Offer offer)
        {
            OfferDb offerDb = _mapper.Map<OfferDb>(offer);
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Offers.Add(offerDb);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            return offerDb.OfferID;
        }
        public List<OfferPreview> GetHotelOffers(Paging paging, int hotelID, bool? isActive)
        {
            var ret = _mapper.Map<List<OfferPreview>>(_dbContext.Offers
                             .Where(o => o.HotelID == hotelID));

            if (isActive.HasValue)
                ret = ret.Where(o => o.IsActive == isActive.Value).ToList();

            return ret.Skip((paging.pageNumber - 1) * paging.pageSize)
                      .Take(paging.pageSize)
                      .ToList();
        }

        public Offer GetOffer(int offerID)
        {
            return _mapper.Map<Offer>(_dbContext.Offers.Find(offerID));
        }
        public int? FindOfferAndGetOwner(int offerID)
        {
            List<int> owners = _dbContext.Offers.Where(o => o.OfferID == offerID)
                                                .Select(o => o.HotelID)
                                                .ToList();
            if (owners.Count == 0)
                return null;
            return owners[0];
        }
        public void UpdateOffer(int offerID, OfferUpdateInfo offerUpdateInfo)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                OfferDb offer = _dbContext.Offers.Find(offerID);
                offer.IsActive = offerUpdateInfo.IsActive ?? offer.IsActive;
                offer.OfferTitle = offerUpdateInfo.OfferTitle ?? offer.OfferTitle;
                offer.Description = offerUpdateInfo.Description ?? offer.Description;
                offer.OfferPreviewPicture = offerUpdateInfo.OfferPreviewPicture ?? offer.OfferPreviewPicture;
                if (offerUpdateInfo.OfferPictures != null)
                {
                    offer.OfferPictures.RemoveAll(op => op.OfferID == offerID);
                    foreach (string picture in offerUpdateInfo.OfferPictures)
                        offer.OfferPictures.Add(new OfferPictureDb(picture, offerID));
                }
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public void AddOfferPicture(string picture, int offerID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                OfferPictureDb offerPicture = new OfferPictureDb { Picture = picture, OfferID = offerID };
                _dbContext.OfferPictures.Add(offerPicture);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public void AddOfferPictures(List<string> pictures, int offerID)
        {
            List<OfferPictureDb> picturesDb = new List<OfferPictureDb>();
            foreach (string picture in pictures)
                picturesDb.Add(new OfferPictureDb(picture, offerID));

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.OfferPictures.AddRange(picturesDb);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public bool AreThereUnfinishedReservationsForOffer(int offerID)
        {
            return _dbContext.ClientReservations.Where(cr => cr.OfferID == offerID && cr.ToTime > DateTime.Now).Any();
        }

        public void UnpinRoomsFromOffer(int offerID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                IQueryable<OfferHotelRoomDb> roomsToUnpin = _dbContext.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID);
                _dbContext.OfferHotelRooms.RemoveRange(roomsToUnpin);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
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
