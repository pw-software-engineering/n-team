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
        public int AddOffer(Offer offer)
        {
            db.Offers.Add(offer);
            db.SaveChanges();
            return offer.OfferID;
        }
        public bool DeleteOffer(int offerID)
        {
            Offer offer = db.Offers.Find(offerID);
            if (offer != null)
            {
                offer.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Offer> GetHotelOffers(int hotelID)
        {
            return db.Offers.Where(o => o.HotelID == hotelID).ToList();
        }

        public Offer GetOffer(int offerID)
        {
            return db.Offers.Find(offerID);
        }

        public bool UpdateOffer(int offerID, bool? isActive, string offerTitle, string description, string offerPreviewPicture, List<string> offerPictures)
        {
            Offer offer = db.Offers.Find(offerID);
            if(offer!=null)
            {
                if (isActive.HasValue)
                    offer.IsActive = isActive.Value;  
                if (offerTitle==null)
                    offer.Title = offerTitle;
                if (description == null)
                    offer.Description = description;
                if (offerPreviewPicture == null)
                    offer.OfferPreviewPicture = offerPreviewPicture;
                if (offerPictures == null)
                {
                    offer.OfferPictures.RemoveAll(op => op.OfferID == offerID);
                    foreach (string picture in offerPictures)
                        offer.OfferPictures.Add(new OfferPicture { OfferID = offerID, Picture = picture });
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
