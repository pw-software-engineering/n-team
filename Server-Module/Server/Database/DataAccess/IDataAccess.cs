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
        void AddOffer(Offer offer);
        List<Offer> GetHotelOffers(int hotelID);
        Offer GetOffer(int offerID);
        void UpdateOffer(OfferUpdateInfo offer);
        #endregion

        #region OfferPictures
        void AddOfferPicture(OfferPicture pictrue);
        #endregion
    }
}
