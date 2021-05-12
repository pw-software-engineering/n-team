using Server.Database;
using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.ViewModels;
using Server.Database.DataAccess;
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
            _transaction.BeginTransaction();
            int offerID = _dataAccess.AddOffer(hotelID, offerInfo);
            _dataAccess.AddOfferPictures(offerID, offerInfo.Pictures);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK, new OfferIDView(offerID));
        }

        public IServiceResult DeleteOffer(int hotelID, int offerID)
        {
            IServiceResult response = CheckExistanceAndOwnership(hotelID, offerID);
            if (response != null)
                return response;

            if (_dataAccess.AreThereUnfinishedReservationsForOffer(offerID))
                return new ServiceResult(HttpStatusCode.Conflict, new ErrorView("There are still pending reservations for this offer"));

            _transaction.BeginTransaction();
            _dataAccess.UnpinRoomsFromOffer(offerID);
            _dataAccess.DeleteOffer(offerID);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK);
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
            if (response != null)
                return response;

            OfferView offerView = _mapper.Map<OfferView>(_dataAccess.GetOffer(offerID));
            offerView.Pictures = _dataAccess.GetOfferPictures(offerID);
            return new ServiceResult(HttpStatusCode.OK, offerView);
        }

        public IServiceResult UpdateOffer(int hotelID, int offerID, OfferInfoUpdate offerUpdateInfo)
        {
            IServiceResult response = CheckExistanceAndOwnership(hotelID, offerID);
            if (response != null)
                return response;

            _transaction.BeginTransaction();
            _dataAccess.UpdateOffer(offerID, offerUpdateInfo);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult CheckExistanceAndOwnership(int hotelID, int offerID)
        {
            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound);
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized);
            return null;
        }
    }
}
