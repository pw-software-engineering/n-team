using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IDataAccess
    {
        #region Offer
        void AddOffer(Offer offer, int hotelID);
        List<OfferPreview> GetHotelOffers(int hotelID);
        Offer GetOffer(int offerID);
        void DeleteOffer(int offerID);
        void UpdateOffer(int offerID, Offer offer);
        #endregion

        #region OfferPictures
        void AddOfferPicture(string pictrue);
        #endregion


    }
}
