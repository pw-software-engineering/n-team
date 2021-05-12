using Server.RequestModels;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public interface IOfferSearchDataAccess
    {
        bool CheckHotelExistence(int hotelID);
        List<OfferSearchPreviewView> GetHotelOffers(int hotelID, Paging paging, OfferFilter offerFilter);

        bool CheckHotelOfferExistence(int hotelID, int offerID);
        ClientOfferView GetHotelOfferDetails(int offerID);
        List<string> GetHotelOfferPictures(int offerID);
        //List<(DateTime begin, DateTime end)> GetHotelOfferAvailability(int hotelID, int offerID, DateTime from, DateTime to);
    }
}
