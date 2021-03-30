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
        void AddOffer(OfferUpdateInfo offer);
        List<OfferUpdateInfo> GetHotelOffers(int hotelID);
        OfferUpdateInfo GetOffer(int offerID);
        void UpdateOffer(OfferUpdateInfo offer);
        #endregion

        #region OfferPictures
        void AddOfferPicture(OfferPicture pictrue);
        #endregion
    }
}
