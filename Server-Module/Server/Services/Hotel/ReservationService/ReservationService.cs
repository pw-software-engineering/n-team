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
        public IServiceResult GetReservations(int hotelID, Paging paging, bool currentOnly, int? roomID)
        {
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Invalid paging arguments"));

            if(roomID.HasValue)
            {
                IServiceResult result = CheckExistanceAndOwnership(hotelID, roomID.Value);
                if (result != null)
                    return result;
            }

            _transaction.BeginTransaction();
            List<ReservationObjectView> reservationObjects = _dataAccess.GetReservations(hotelID, roomID, currentOnly, paging);
            foreach(ReservationObjectView reservationObject in reservationObjects)
            {
                reservationObject.Client = _dataAccess.GetClientDetails(reservationObject.Client.ClientID);
                reservationObject.Room = _dataAccess.GetRoomDetails(reservationObject.Room.RoomID);
            }
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK, reservationObjects);
        }
        public IServiceResult CheckExistanceAndOwnership(int hotelID, int roomID)
        {
            int? ownerID = _dataAccess.FindRoomAndGetOwner(roomID);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Room with ID equal to {roomID} does not exist"));
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized);
            return null;
        }
    }
}
