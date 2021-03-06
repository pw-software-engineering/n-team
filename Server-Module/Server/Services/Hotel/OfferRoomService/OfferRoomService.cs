﻿using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System.Collections.Generic;
using System.Net;

namespace Server.Services.Hotel
{
    public class OfferRoomService : IOfferRoomService
    {
        private readonly IOfferRoomDataAccess _dataAccess;
        private readonly IDatabaseTransaction _transaction;
        public OfferRoomService(IOfferRoomDataAccess dataAccess, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _transaction = transaction;
        }
        public IServiceResult AddRoomToOffer(int roomID, int offerID, int hotelID)
        {
            IServiceResult result = CheckRoomExistanceAndOwnership(roomID, hotelID);
            if (!(result is null))
                return result;

            result = CheckOfferExistanceAndOwnership(offerID, hotelID);
            if (!(result is null))
                return result;

            if (_dataAccess.IsRoomAlreadyAddedToOffer(roomID, offerID))
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView($"Room with ID equal to {roomID} is already added to offer with ID equal to {offerID}"));

            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                _dataAccess.AddRoomToOffer(roomID, offerID);
                _transaction.CommitTransaction();
            }

            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult GetOfferRooms(int offerID, int hotelID, string hotelRoomNumber, Paging paging)
        {
            if (paging.PageSize < 1 || paging.PageNumber < 1)
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Invalid paging arguments"));

            IServiceResult result = CheckOfferExistanceAndOwnership(offerID, hotelID);
            if (!(result is null))
                return result;

            if (hotelRoomNumber != null)
            {
                result = CheckRoomExistanceAndOwnership(hotelRoomNumber, hotelID);
                if (result != null)
                    return result;
            }

            List<OfferRoomView> offerRoomViews = _dataAccess.GetOfferRooms(offerID, paging, hotelRoomNumber);

            return new ServiceResult(HttpStatusCode.OK, offerRoomViews);
        }

        public IServiceResult RemoveRoomFromOffer(int roomID, int offerID, int hotelID)
        {
            IServiceResult result = CheckRoomExistanceAndOwnership(roomID, hotelID);
            if (!(result is null))
                return result;

            result = CheckOfferExistanceAndOwnership(offerID, hotelID);
            if (!(result is null))
                return result;

            if (!_dataAccess.IsRoomAlreadyAddedToOffer(roomID, offerID))
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView($"Room with ID equal to {roomID} is not added to offer with ID equal to {offerID}"));
            if (_dataAccess.DoesRoomHaveUnfinishedReservations(roomID, offerID))
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView($"Room with ID equal to {roomID} has unfinished reservations"));
           
            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                _dataAccess.UnpinRoomFromOffer(roomID, offerID);
                _transaction.CommitTransaction();

                return new ServiceResult(HttpStatusCode.OK);
            }
        }

        public IServiceResult CheckRoomExistanceAndOwnership(string hotelRoomNumber, int hotelID)
        {
            int? ownerID = _dataAccess.FindRoomAndGetOwner(hotelRoomNumber);
            if (ownerID is null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Room with RoomNumber equal to {hotelRoomNumber} does not exist"));
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView($"Room with RoomNumber equal to {hotelRoomNumber} does not belong to hotel with ID equal to {hotelID}"));
            return null;
        }
        public IServiceResult CheckRoomExistanceAndOwnership(int roomID, int hotelID)
        {
            int? ownerID = _dataAccess.FindRoomAndGetOwner(roomID);
            if (ownerID is null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Room with ID equal to {roomID} does not exist"));
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView($"Room with ID equal to {roomID} does not belong to hotel with ID equal to {hotelID}"));
            return null;
        }
        public IServiceResult CheckOfferExistanceAndOwnership(int offerID, int hotelID)
        {
            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);
            if (ownerID is null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Offer with ID equal to {offerID} does not exist"));
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView($"Offer with ID equal to {offerID} does not belong to hotel with ID equal to {hotelID}"));
            return null;
        }
    }
}
