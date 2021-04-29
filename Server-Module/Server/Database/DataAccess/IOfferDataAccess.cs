using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
{
    public interface IOfferDataAccess
    {
        #region /offers GET
        List<OfferPreview> GetHotelOffers(Paging paging, int hotelID, bool? isActive = null);
        #endregion

        #region /offers POST
        int AddOffer(Offer offer);
        void AddOfferPicture(string picture, int offerID);
        void AddOfferPictures(List<string> picture, int offerID);
        #endregion

        #region /offers/{offerID} GET
        Offer GetOffer(int offerID);
        List<string> GetOfferRooms(int offerID);
        List<string> GetOfferPictures(int offerID);
        #endregion

        #region /offers/{offerID} PATCH
        void UpdateOffer(int offerID, OfferUpdateInfo offer);
        #endregion

        #region /offers/{offerID} DELETE
        void DeleteOffer(int offerID);
        bool AreThereUnfinishedReservationsForOffer(int offerID);
        void UnpinRoomsFromOffer(int offerID);
        #endregion

        int? FindOfferAndGetOwner(int offerID);
    }
}
