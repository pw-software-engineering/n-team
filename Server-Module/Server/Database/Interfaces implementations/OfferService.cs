using Server.Database.Interfaces;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Interfaces_implementations
{
    public class OfferService : IOfferService
    {
        private ServerDbContext db;
        public OfferService(ServerDbContext db)
        {
            this.db = db;
        }
        public int AddOffer(Offer offer, int hotelID)
        {
            OfferDb offerDb = new OfferDb(offer, hotelID);
            db.Offers.Add(offerDb);

            foreach (string picture in offer.pictures)
                db.OfferPictures.Add(new OfferPictureDb(picture, offerDb.OfferID));
            
            db.SaveChanges();
            return offerDb.OfferID;
        }
        public bool DeleteOffer(int offerID)
        {
            OfferDb offer = db.Offers.Find(offerID);
            if (offer != null)
            {
                offer.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<OfferPreview> GetHotelOffers(int hotelID)
        {
            List<OfferPreview> offerPreviews = new List<OfferPreview>();
            foreach (OfferDb offer in db.Offers.Where(o => o.HotelID == hotelID).ToList())
                offerPreviews.Add(new OfferPreview(offer));
            return offerPreviews; 
        }

        public Offer GetOffer(int offerID)
        {
            return new Offer(db.Offers.Find(offerID));
        }

        public bool UpdateOffer(int offerID, bool? isActive, string offerTitle, string description, string offerPreviewPicture, List<string> offerPictures)
        {
            OfferDb offer = db.Offers.Find(offerID);
            if(offer!=null)
            {
                offer.IsActive = isActive ?? offer.IsActive;
                offer.OfferTitle = offerTitle ?? offer.OfferTitle;
                offer.Description = description ?? offer.Description;
                offer.OfferPreviewPicture = offerPreviewPicture ?? offer.OfferPreviewPicture;
                if (offerPictures == null)
                {
                    offer.OfferPictures.RemoveAll(op => op.OfferID == offerID);
                    foreach (string picture in offerPictures)
                        offer.OfferPictures.Add(new OfferPictureDb(picture, offerID));
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
