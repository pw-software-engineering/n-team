using AutoMapper;
using Server.Database.DataAccess.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
            IServiceResult response = CheckOfferExistanceAndOwnership(offerID, hotelID);
            if (response != null)
                return response;

            _transaction.BeginTransaction();
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

            _transaction.RollbackTransaction();
            return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Offer is not available in chosen time interval"));
        }

        public IServiceResult CancelReservation(int reservationID, int userID)
        {
            IServiceResult response = CheckReservationExistanceAndOwnership(reservationID, userID);
            if (response != null)
                return response;

            if (_dataAccess.HasReservationBegun(reservationID))
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Reservation is currently underway or already completed"));

            _transaction.BeginTransaction();
            _dataAccess.RemoveReservation(reservationID);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK);
        }
        public IServiceResult CheckReservationExistanceAndOwnership(int reservationID, int userID)
        {
            int? ownerID = _dataAccess.FindReservationAndGetOwner(reservationID);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound);
            if (ownerID != userID)
                return new ServiceResult(HttpStatusCode.Unauthorized);
            return null;
        }
        public IServiceResult CheckOfferExistanceAndOwnership(int offerID, int userID)
        {
            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);
            if (ownerID == null || ownerID != userID)
                return new ServiceResult(HttpStatusCode.NotFound);
            return null;
        }
    }
}
