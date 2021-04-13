﻿using AutoMapper;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public DataAccess(IMapper mapper, ServerDbContext dbContext)
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
        public int AddOffer(Offer offer)
        {
            OfferDb offerDb = _mapper.Map<OfferDb>(offer);
            _dbContext.Offers.Add(offerDb);
            _dbContext.SaveChanges();
            return offerDb.OfferID;
        }

        public List<OfferPreview> GetHotelOffers(int hotelID)
        {
            return _mapper.Map<List<OfferPreview>>(_dbContext.Offers.Where(o => o.HotelID == hotelID).ToList());
        }

        public Offer GetOffer(int offerID)
        {
            return _mapper.Map<Offer>(_dbContext.Offers.Find(offerID));
        }
        public int? FindOfferAndGetOwner(int offerID)
        {
            List<int> owners = _dbContext.Offers.Where(o => o.OfferID == offerID).Select(o => o.HotelID).ToList();
            if (owners.Count == 0)
                return null;
            return owners[0];
        }
        public void UpdateOffer(int offerID, OfferUpdateInfo offerUpdateInfo)
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
        }

        public void AddOfferPicture(string picture, int offerID)
        {
            _dbContext.OfferPictures.Add(new OfferPictureDb(picture, offerID));
            _dbContext.SaveChanges();
        }

        public void AddOfferPictures(List<string> pictures, int offerID)
        {
            List<OfferPictureDb> picturesDb = new List<OfferPictureDb>();
            foreach (string picture in pictures)
                picturesDb.Add(new OfferPictureDb(picture, offerID));
            _dbContext.AddRange(picturesDb);
            _dbContext.SaveChanges();
        }
    }
}
