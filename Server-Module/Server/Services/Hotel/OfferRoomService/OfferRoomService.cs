using AutoMapper;
using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.Hotel
{
    public class OfferRoomService : IOfferRoomService
    {
        private readonly IOfferRoomDataAccess _dataAccess;
        private readonly IMapper _mapper;
        private readonly IDatabaseTransaction _transaction;
        public OfferRoomService(IOfferRoomDataAccess dataAccess, IMapper mapper, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
            _transaction = transaction;
        }
        public IServiceResult AddRoomToOffer(int roomID, int offerID, int hotelID)
        {
            IServiceResult result = CheckRoomExistanceAndOwnership(roomID, hotelID);
            if (result != null)
                return result;

            result = CheckOfferExistanceAndOwnership(offerID, hotelID);
            if (result != null)
                return result;

            _transaction.BeginTransaction();
            _dataAccess.AddRoomToOffer(roomID, offerID);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult GetOfferRooms(int offerID, int hotelID, string hotelRoomNumber, Paging paging)
        {
            if (paging.PageSize < 0 || paging.PageNumber < 0)
                return new ServiceResult(HttpStatusCode.BadRequest);

            IServiceResult result = CheckOfferExistanceAndOwnership(offerID, hotelID);
            if (result != null)
                return result;

            result = CheckRoomExistanceAndOwnership(hotelRoomNumber, hotelID);
            if (result != null)
                return result;

            _transaction.BeginTransaction();
            List<OfferRoomView> offerRoomViews = _dataAccess.GetOfferRooms(offerID, hotelRoomNumber, paging);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK, offerRoomViews);
        }

        public IServiceResult RemoveRoomFromOffer(int roomID, int offerID, int hotelID)
        {
            IServiceResult result = CheckRoomExistanceAndOwnership(roomID, hotelID);
            if (result != null)
                return result;

            result = CheckOfferExistanceAndOwnership(offerID, hotelID);
            if (result != null)
                return result;

            _transaction.BeginTransaction();
            _dataAccess.UnpinRoomFromOffer(roomID, offerID);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult CheckRoomExistanceAndOwnership(string hotelRoomNumber, int hotelID)
        {
            int? ownerID = _dataAccess.FindRoomAndGetOwner(hotelRoomNumber);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView("Room with given ID does not exist"));
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView("Room does not belong to this hotel"));
            return null;
        }
        public IServiceResult CheckRoomExistanceAndOwnership(int roomID, int hotelID)
        {
            int? ownerID = _dataAccess.FindRoomAndGetOwner(roomID);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView("Room with given ID does not exist"));
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView("Room does not belong to this hotel"));
            return null;
        }
        public IServiceResult CheckOfferExistanceAndOwnership(int offerID, int hotelID)
        {
            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView("Offer with given ID does not exist"));
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView("Offer does not belong to this hotel"));
            return null;
        }

    }
}
