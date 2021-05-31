using AutoMapper;
using Server.Database.DataAccess.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.Client
{
    public class OfferSearchService : IOfferSearchService
    {
        private readonly IMapper _mapper;
        private readonly IOfferSearchDataAccess _offerSearchDataAccess;
        private readonly IDatabaseTransaction _transaction;

        public OfferSearchService(IMapper mapper, IOfferSearchDataAccess offerSearchDataAccess, IDatabaseTransaction transaction)
        {
            _mapper = mapper;
            _offerSearchDataAccess = offerSearchDataAccess;
            _transaction = transaction;
        }
        public IServiceResult GetHotelOffers(int hotelID, OfferFilter offerFilter, Paging paging)
        {
            if (paging == null)
            {
                throw new ArgumentNullException("paging");
            }
            if (offerFilter == null)
            {
                throw new ArgumentNullException("offerFilter");
            }

            if (!_offerSearchDataAccess.CheckHotelExistence(hotelID))
            {
                return new ServiceResult(HttpStatusCode.NotFound);
            }
            if (offerFilter.FromTime == null || offerFilter.ToTime == null)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("FromTime and ToTime properties are required."));
            }
            if (paging.PageNumber < 1 || paging.PageSize < 1)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));
            }
            if(offerFilter.FromTime.Value > offerFilter.ToTime.Value)
            {
                return new ServiceResult(
                   HttpStatusCode.BadRequest,
                   new ErrorView("From field cannot be greater than To field"));
            }
            if(offerFilter.MinGuests.HasValue && offerFilter.MinGuests < 0)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MinGuests must be a non-negative integer"));
            }
            if(offerFilter.CostMin.HasValue && offerFilter.CostMin < 0)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MinCost must be a non-negative integer"));
            }
            if (offerFilter.CostMax.HasValue && offerFilter.CostMax < 0)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MaxCost must be a non-negative integer"));
            }
            if (offerFilter.CostMin.HasValue && offerFilter.CostMax.HasValue && offerFilter.CostMin > offerFilter.CostMax)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MaxCost must be greater or equal to MinCost"));
            }

            return new ServiceResult(
                HttpStatusCode.OK,
                _offerSearchDataAccess.GetHotelOffers(hotelID, offerFilter, paging));
        }

        public IServiceResult GetHotelOfferDetails(int hotelID, int offerID)
        {
            if(!_offerSearchDataAccess.CheckHotelOfferExistence(hotelID, offerID))
            {
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Hotel with ID equal to {hotelID} does not exist or has no offer with ID equal to {offerID}"));
            }
            _transaction.BeginTransaction();
            OfferView offer = _offerSearchDataAccess.GetHotelOfferDetails(offerID);
            offer.OfferPictures = _offerSearchDataAccess.GetHotelOfferPictures(offerID);
            DateTime fromTime = DateTime.Now;
            fromTime = new DateTime(fromTime.Year, fromTime.Month, fromTime.Day);
            offer.AvailabilityTimeIntervals = _offerSearchDataAccess.GetHotelOfferAvailability(hotelID, offerID, fromTime, fromTime.AddMonths(6));
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK, offer);
        }

        public IServiceResult GetHotelOfferReviews(int hotelID, int offerID)
        {
            try 
            {
                return new ServiceResult(HttpStatusCode.OK, _offerSearchDataAccess.GetOfferReviews(hotelID, offerID));
            }catch(Exception e)
            {
                return new ServiceResult(HttpStatusCode.NotFound,new ErrorView(e.Message));
            }
        }

        public IServiceResult GetHotelReviews(int hotelID, int offerID, Paging paging)
        {
            int from = (paging.PageNumber - 1) * paging.PageSize;
            try
            {
                return new ServiceResult(HttpStatusCode.OK, _offerSearchDataAccess.GetHotelReviews(hotelID, from, paging.PageSize));
            }
            catch (Exception e)
            {
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView(e.Message));
            }
        }
    }
}
