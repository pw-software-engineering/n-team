using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public interface IOfferSearchDataAccess
    {
        bool CheckHotelExistence(int hotelID);
        List<OfferPreviewView> GetHotelOffers(int hotelID, OfferFilter offerFilter, Paging paging);

        bool CheckHotelOfferExistence(int hotelID, int offerID);
        OfferView GetHotelOfferDetails(int offerID);
        List<string> GetHotelOfferPictures(int offerID);
        List<AvailabilityTimeInterval> GetHotelOfferAvailability(int hotelID, int offerID, DateTime from, DateTime to);
    }
}
