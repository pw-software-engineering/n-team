using Server.Database.DataAccess.Client;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Net;

namespace Server.Services.Client
{
    public class OfferSearchService : IOfferSearchService
    {
        private readonly IOfferSearchDataAccess _offerSearchDataAccess;

        public OfferSearchService(IOfferSearchDataAccess offerSearchDataAccess)
        {
            _offerSearchDataAccess = offerSearchDataAccess;
        }
        public IServiceResult GetHotelOffers(int hotelID, OfferFilter offerFilter, Paging paging)
        {
            if (paging is null)
                throw new ArgumentNullException("paging");
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));
            if (offerFilter is null)
                throw new ArgumentNullException("offerFilter");

            if (!_offerSearchDataAccess.CheckHotelExistence(hotelID))
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Hotel with ID equal to {hotelID} does not exist"));

            if (offerFilter.FromTime == null || offerFilter.ToTime == null)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("FromTime and ToTime properties are required."));
            if(offerFilter.FromTime.Value > offerFilter.ToTime.Value)
                return new ServiceResult(
                   HttpStatusCode.BadRequest,
                   new ErrorView("From field cannot be greater than To field"));
            if(offerFilter.MinGuests.HasValue && offerFilter.MinGuests < 0)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MinGuests must be a non-negative integer"));

            if(offerFilter.CostMin.HasValue && offerFilter.CostMin < 0)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MinCost must be a non-negative integer"));

            if (offerFilter.CostMax.HasValue && offerFilter.CostMax < 0)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MaxCost must be a non-negative integer"));

            if (offerFilter.CostMin.HasValue && offerFilter.CostMax.HasValue && offerFilter.CostMin > offerFilter.CostMax)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MaxCost must be greater or equal to MinCost"));

            return new ServiceResult(
                HttpStatusCode.OK,
                _offerSearchDataAccess.GetHotelOffers(hotelID, offerFilter, paging));
        }

        public IServiceResult GetHotelOfferDetails(int hotelID, int offerID)
        {
            if(!_offerSearchDataAccess.CheckHotelOfferExistence(hotelID, offerID))
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Hotel with ID equal to {hotelID} does not exist or has no offer with ID equal to {offerID}"));

            OfferView offer = _offerSearchDataAccess.GetHotelOfferDetails(offerID);
            offer.OfferPictures = _offerSearchDataAccess.GetHotelOfferPictures(offerID);
            DateTime fromTime = DateTime.Now;
            fromTime = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day).AddDays(1);
            offer.AvailabilityTimeIntervals = _offerSearchDataAccess.GetHotelOfferAvailability(hotelID, offerID, fromTime, fromTime.AddMonths(6));

            return new ServiceResult(HttpStatusCode.OK, offer);
        }

        public IServiceResult GetHotelOfferReviews(int hotelID, int offerID, Paging paging)
        {
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));
            if(!_offerSearchDataAccess.CheckHotelOfferExistence(hotelID, offerID))
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Hotel with ID equal to {hotelID} does not exist or has no offer with ID equal to {offerID}"));

            return new ServiceResult(HttpStatusCode.OK, _offerSearchDataAccess.GetOfferReviews(hotelID, offerID, paging));   
        }
    }
}
