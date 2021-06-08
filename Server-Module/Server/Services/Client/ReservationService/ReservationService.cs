using AutoMapper;
using Server.Database.DataAccess.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;

namespace Server.Services.Client
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationDataAccess _dataAccess;
        private readonly IMapper _mapper;
        private readonly IDatabaseTransaction _transaction;
        public ReservationService(IReservationDataAccess dataAccess, IMapper mapper, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
            _transaction = transaction;
        }
        public IServiceResult AddReservation(int hotelID, int offerID, int userID, ReservationInfo reservationInfo)
        {
            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                IServiceResult response = CheckOfferExistanceAndOwnership(offerID, hotelID);
                if (!(response is null))
                    return response;

                DateTime tomorrow = DateTime.Now;
                tomorrow = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day).AddDays(1);
                if (reservationInfo.From < tomorrow)
                    return new ServiceResult(
                        HttpStatusCode.BadRequest,
                        new ErrorView("Cannot create a reservation that begins earlier than tomorrow"));

                if (reservationInfo.From > reservationInfo.To)
                    return new ServiceResult(
                        HttpStatusCode.BadRequest,
                        new ErrorView("FromTime cannot be greater than ToTime"));

                List<int> roomIDs = _dataAccess.GetOfferRoomIDs(offerID);
                foreach (int roomID in roomIDs)
                {
                    if (_dataAccess.IsRoomAvailable(roomID, reservationInfo.From, reservationInfo.To))
                    {
                        Reservation reservation = _mapper.Map<Reservation>(reservationInfo);
                        reservation.ClientID = userID;
                        reservation.HotelID = hotelID;
                        reservation.OfferID = offerID;
                        reservation.RoomID = roomID;

                        _dataAccess.AddReservation(reservation);
                        _transaction.CommitTransaction();
                        return new ServiceResult(HttpStatusCode.OK);
                    }
                }

                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Offer is not available in chosen time interval"));
            }
        }

        public IServiceResult CancelReservation(int reservationID, int userID)
        {
            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                IServiceResult response = CheckReservationExistanceAndOwnership(reservationID, userID);
                if (!(response is null))
                    return response;

                if (_dataAccess.HasReservationBegun(reservationID))
                    return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Reservation is currently underway or already completed"));

                _dataAccess.RemoveReservation(reservationID);

                return new ServiceResult(HttpStatusCode.OK);
            }
        }

        public IServiceResult GetReservations(int userID, Paging paging)
		{
            if(paging is null)
                throw new ArgumentNullException("paging");
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView("Invalid paging arguments"));

            return new ServiceResult(HttpStatusCode.OK, _dataAccess.GetReservations(userID, paging));
		}

        public IServiceResult CheckReservationExistanceAndOwnership(int reservationID, int userID)
        {
            int? ownerID = _dataAccess.FindReservationAndGetOwner(reservationID); 
            if (!ownerID.HasValue)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Reservation with ID equal to {reservationID} does not exist"));
            if (ownerID.Value != userID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView("You are not owner of requested reservation"));
            return null;
        }
        public IServiceResult CheckOfferExistanceAndOwnership(int offerID, int hotelID)
        {
            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);
            if (!ownerID.HasValue) 
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Hotel with ID equal to {hotelID} does not exist"));
            if (ownerID.Value != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView($"Hotel with ID equal to {hotelID} has no offer with ID equal to {offerID}"));
            return null;
        }
    }
}
