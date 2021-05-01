using Server.Models;
using Server.RequestModels;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.OfferSearch
{
    public interface IOfferSearchDataAccess
    {
        bool CheckHotelExistence(int hotelID);
        List<ClientOfferPreview> GetHotelOffers(int hotelID, Paging paging, OfferFilter offerFilter);

        bool CheckHotelOfferExistence(int hotelID, int offerID);
        ClientOffer GetHotelOfferDetails(int offerID);
        List<string> GetHotelOfferPictures(int offerID);
        //List<(DateTime begin, DateTime end)> GetHotelOfferAvailability(int hotelID, int offerID, DateTime from, DateTime to);
    }
}
