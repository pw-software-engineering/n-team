using AutoMapper;
using Server.Database.DataAccess.OfferSearch;
using Server.Database.DatabaseTransaction;
using Server.Models;
using Server.RequestModels;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.OfferSearchService
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
        public IServiceResult GetHotelOffers(int hotelID, Paging paging, OfferFilter offerFilter)
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
            if (paging.pageNumber < 1 || paging.pageSize < 1)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new Error("Invalid paging arguments"));
            }
            if(offerFilter.MinGuests.HasValue && offerFilter.MinGuests < 0)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new Error("MinGuests must be a non-negative integer"));
            }
            if(offerFilter.MinCost.HasValue && offerFilter.MinCost < 0)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new Error("MinCost must be a non-negative integer"));
            }
            if (offerFilter.MinCost.HasValue && offerFilter.MaxCost < 0)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new Error("MaxCost must be a non-negative integer"));
            }
            if (offerFilter.MinCost.HasValue && offerFilter.MaxCost.HasValue && offerFilter.MinCost > offerFilter.MaxCost)
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new Error("MaxCost must be greater or equal to MinCost"));
            }

            return new ServiceResult(
                HttpStatusCode.OK,
                _offerSearchDataAccess.GetHotelOffers(hotelID, paging, offerFilter));
        }

        public IServiceResult GetHotelOfferDetails(int hotelID, int offerID)
        {
            if(!_offerSearchDataAccess.CheckHotelOfferExistence(hotelID, offerID))
            {
                return new ServiceResult(HttpStatusCode.NotFound);
            }
            _transaction.BeginTransaction();
            ClientOffer offer = _offerSearchDataAccess.GetHotelOfferDetails(offerID);
            offer.OfferPictures = _offerSearchDataAccess.GetHotelOfferPictures(offerID);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK, offer);
        }
    }
}
