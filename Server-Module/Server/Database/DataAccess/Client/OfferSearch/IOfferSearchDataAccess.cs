using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;

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

        List<ReviewInfo> GetOfferReviews(int hotelID, int offerID);

        List<ReviewInfo> GetHotelReviews(int hotelID, int from, int take);
    }
}
