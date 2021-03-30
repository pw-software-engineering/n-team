using Server.Database;
using Server.Exceptions;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.OfferService
{   
    public class OfferService : IOfferService
    {
        private ServerDbContext db;
        public OfferService(ServerDbContext db)
        {
            this.db = db;
        }
        
        public int AddOffer(OfferView offer, int hotelID)
        {
            OfferDb offerDb = new OfferDb(offer, hotelID);
            db.Offers.Add(offerDb);

            foreach (string picture in offer.pictures)
                db.OfferPictures.Add(new OfferPictureDb(picture, offerDb.OfferID));
            
            db.SaveChanges();
            return offerDb.OfferID;
        }
        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void DeleteOffer(int offerID, int hotelID)
        {
            OfferDb offer = db.Offers.Find(offerID);
            CheckExceptions(offer, hotelID);
            offer.IsDeleted = true;
            db.SaveChanges();
        }

        public List<OfferPreviewView> GetHotelOffers(int hotelID)
        {
            List<OfferPreviewView> offerPreviews = new List<OfferPreviewView>();
            foreach (OfferDb offer in db.Offers.Where(o => o.HotelID == hotelID).ToList())
                offerPreviews.Add(new OfferPreviewView(offer));
            return offerPreviews; 
        }
        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public OfferView GetOffer(int offerID, int hotelID)
        {
            OfferDb offer = db.Offers.Find(offerID);
            CheckExceptions(offer, hotelID);
            return new OfferView(offer);
        }
        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void UpdateOffer(int offerID, int hotelID, bool? isActive, string offerTitle, string description, string offerPreviewPicture, List<string> offerPictures)
        {         
            OfferDb offer = db.Offers.Find(offerID);
            CheckExceptions(offer, hotelID);

            offer.IsActive = isActive ?? offer.IsActive;
            offer.OfferTitle = offerTitle ?? offer.OfferTitle;
            offer.Description = description ?? offer.Description;
            offer.OfferPreviewPicture = offerPreviewPicture ?? offer.OfferPreviewPicture;
            if (offerPictures != null)
            {
                offer.OfferPictures.RemoveAll(op => op.OfferID == offerID);
                foreach (string picture in offerPictures)
                    offer.OfferPictures.Add(new OfferPictureDb(picture, offerID));
            }
            db.SaveChanges();
        }
        /// <exception cref="NotOwnerException" cref="NotFoundException"></exception>
        public void CheckExceptions(OfferDb offer, int hotelID)
        {
            if (offer == null)
                throw new NotFoundException();
            if (offer.HotelID != hotelID)
                throw new NotOwnerException();
        }
    }
}
