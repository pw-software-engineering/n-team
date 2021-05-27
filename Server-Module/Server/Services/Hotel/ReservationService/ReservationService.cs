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
        public IServiceResult GetReservations(int hotelID, bool? currentOnly, int? roomID, Paging paging)
        {
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Invalid paging arguments"));

            if(roomID.HasValue)
            {
                IServiceResult result = CheckRoomExistanceAndOwnership(hotelID, roomID.Value);
                if (result != null)
                    return result;
            }

            _transaction.BeginTransaction();
            List<ReservationObjectView> reservationObjects = _dataAccess.GetReservations(hotelID, roomID, currentOnly, paging);
            _transaction.CommitTransaction();

            return new ServiceResult(HttpStatusCode.OK, reservationObjects);
        }
        public IServiceResult CheckRoomExistanceAndOwnership(int hotelID, int roomID)
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
