using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.ViewModels.Hotel;
using System.Collections.Generic;

namespace Server.Database.DataAccess.Hotel
{
    public interface IOfferDataAccess
    {
        #region /offers GET
        List<OfferPreviewView> GetHotelOffers(int hotelID, Paging paging, bool? isActive = null);
        #endregion

        #region /offers POST
        int AddOffer(int hotelID, OfferInfo offerInfo);
        void AddOfferPictures(int offerID, List<string> picture);
        #endregion

        #region /offers/{offerID} GET
        OfferView GetOffer(int offerID);
        List<string> GetOfferRooms(int offerID);
        List<string> GetOfferPictures(int offerID);
        #endregion

        #region /offers/{offerID} PATCH
        void UpdateOffer(int offerID, OfferInfoUpdate offerInfoUpdate);
        #endregion

        #region /offers/{offerID} DELETE
        void DeleteOffer(int offerID);
        bool AreThereUnfinishedReservationsForOffer(int offerID);
        void UnpinRoomsFromOffer(int offerID);
        #endregion

        int? FindOfferAndGetOwner(int offerID);
    }
}
