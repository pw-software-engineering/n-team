using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IOfferDataAccess
    {
        #region Offer
        int AddOffer(Offer offer);
        List<OfferPreview> GetHotelOffers(int hotelID);
        int? FindOfferAndGetOwner(int offerID);
        Offer GetOffer(int offerID);
        void UpdateOffer(int offerID, OfferUpdateInfo offer);
        void DeleteOffer(int offerID);
        #endregion

        #region OfferPictures
        void AddOfferPicture(string picture, int offerID);
        void AddOfferPictures(List<string> picture, int offerID);
        #endregion
    }
}
