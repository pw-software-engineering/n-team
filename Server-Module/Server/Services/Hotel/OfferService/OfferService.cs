using System;
using System.Collections.Generic;
using Server.ViewModels;
using AutoMapper;
using Server.Services.Result;
using Server.RequestModels;
using System.Net;
using Server.Database.DatabaseTransaction;
using Server.Database.DataAccess.Hotel;
using Server.RequestModels.Hotel;
using Server.ViewModels.Hotel;

namespace Server.Services.Hotel
{   
    public class OfferService : IOfferService
    {
        private readonly IOfferDataAccess _dataAccess;
        private readonly IMapper _mapper;
        private readonly IDatabaseTransaction _transaction;
        public OfferService(IOfferDataAccess dataAccess, IMapper mapper, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
            _transaction = transaction;
        }
        public IServiceResult AddOffer(int hotelID, OfferInfo offerInfo)
        {
            if(offerInfo is null)
                throw new ArgumentNullException("offerInfo");

            if(!offerInfo.MaxGuests.HasValue || offerInfo.MaxGuests.Value <= 0)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("MaxGuests property is required and must contain a positive integer value"));

            if(!offerInfo.CostPerAdult.HasValue || offerInfo.CostPerAdult.Value < 0)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("CostPerAdult property is required and must contain a positive real number"));

            if(!offerInfo.CostPerChild.HasValue || offerInfo.CostPerChild.Value < 0)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("CostPerChild property is required and must contain a positive real number"));

            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                int offerID = _dataAccess.AddOffer(hotelID, offerInfo);
                _dataAccess.AddOfferPictures(offerID, offerInfo.Pictures);
                _transaction.CommitTransaction();

                return new ServiceResult(HttpStatusCode.OK, new OfferIDView(offerID));
            }
        }

        public IServiceResult DeleteOffer(int hotelID, int offerID)
        {
            IServiceResult response = CheckExistanceAndOwnership(hotelID, offerID);
            if (!(response is null))
                return response;

            if (_dataAccess.AreThereUnfinishedReservationsForOffer(offerID))
                return new ServiceResult(HttpStatusCode.Conflict, new ErrorView($"There are still pending reservations for offer with ID equal to {offerID}"));

            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                _dataAccess.UnpinRoomsFromOffer(offerID);
                _dataAccess.DeleteOffer(offerID);
                _transaction.CommitTransaction();

                return new ServiceResult(HttpStatusCode.OK);
            }
        }

        public IServiceResult GetHotelOffers(int hotelID, Paging paging, bool? isActive = null)
        {
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Invalid paging arguments"));

            List<OfferPreviewView> offersPreviews = _mapper.Map<List<OfferPreviewView>>(_dataAccess.GetHotelOffers(hotelID, paging, isActive));
            return new ServiceResult(HttpStatusCode.OK, offersPreviews);
        }

        public IServiceResult GetOffer(int hotelID, int offerID)
        {
            IServiceResult response = CheckExistanceAndOwnership(hotelID, offerID);
            if (!(response is null))
                return response;

            OfferView offerView = _dataAccess.GetOffer(offerID);
            offerView.Pictures = _dataAccess.GetOfferPictures(offerID);
            return new ServiceResult(HttpStatusCode.OK, offerView);
        }

        public IServiceResult UpdateOffer(int hotelID, int offerID, OfferInfoUpdate offerUpdateInfo)
        {
            IServiceResult response = CheckExistanceAndOwnership(hotelID, offerID);
            if (!(response is null))
                return response;

            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                _dataAccess.UpdateOffer(offerID, offerUpdateInfo);
                _transaction.CommitTransaction();

                return new ServiceResult(HttpStatusCode.OK);
            }
        }

        public IServiceResult CheckExistanceAndOwnership(int hotelID, int offerID)
        {
            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);
            if (!ownerID.HasValue)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Offer with ID equal to {offerID} does not exist"));
            if (ownerID.Value != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView($"Offer with ID equal to {offerID} does not belong to hotel with ID equal to {hotelID}"));
            return null;
        }
    }
}
