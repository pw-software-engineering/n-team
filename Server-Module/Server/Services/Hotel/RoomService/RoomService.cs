using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System.Collections.Generic;
using System.Net;

namespace Server.Services.Hotel
{
    public class RoomService : IRoomService
    {
        private readonly IRoomDataAccess _dataAccess;
        private readonly IDatabaseTransaction _transaction;
        public RoomService(IRoomDataAccess dataAccess, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _transaction = transaction;
        }
        public IServiceResult AddRoom(int hotelID, string hotelRoomNumber)
        {
            if (hotelRoomNumber is null)
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("hotelRoomNumber property is required"));

            if (_dataAccess.DoesRoomAlreadyExist(hotelID, hotelRoomNumber))
                return new ServiceResult(HttpStatusCode.Conflict, new ErrorView($"Room with RoomNumber equal to {hotelRoomNumber} already exists"));

            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                int roomID = _dataAccess.AddRoom(hotelID, hotelRoomNumber);
                _transaction.CommitTransaction();
                return new ServiceResult(HttpStatusCode.OK, new RoomIDView(roomID));
            }
        }

        public IServiceResult DeleteRoom(int hotelID, int roomID)
        {
            IServiceResult response = CheckExistanceAndOwnership(hotelID, roomID);
            if (response != null)
                return response;

            using (IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                if (_dataAccess.CheckAnyUnfinishedReservations(roomID))
                {
                    _dataAccess.ChangeActivationMark(roomID, false);
                    _transaction.CommitTransaction();
                    return new ServiceResult(HttpStatusCode.Conflict, new ErrorView($"There are still pending reservations for room with ID equal to {roomID}"));
                }

                _dataAccess.UnpinRoomFromAnyOffers(roomID);
                _dataAccess.RemoveRoomFromPastReservations(roomID);
                _dataAccess.DeleteRoom(roomID);
                _transaction.CommitTransaction();

                return new ServiceResult(HttpStatusCode.OK);
            }
        }

        public IServiceResult GetHotelRooms(int hotelID, Paging paging, string hotelRoomNumber = null)
        {
            if (paging.PageNumber < 1 || paging.PageSize < 1)
                return new ServiceResult(HttpStatusCode.BadRequest, new ErrorView("Invalid paging arguments"));

            List<HotelRoomView> rooms = _dataAccess.GetRooms(hotelID, paging, hotelRoomNumber);
            foreach(HotelRoomView room in rooms)
                room.OfferID = _dataAccess.GetOfferIDsForRoom(room.RoomID);

            return new ServiceResult(HttpStatusCode.OK, rooms);
        }
        public IServiceResult CheckExistanceAndOwnership(int hotelID, int roomID)
        {
            int? ownerID = _dataAccess.FindRoomAndGetOwner(roomID);
            if (!ownerID.HasValue)
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView($"Room with ID equal to {roomID} does not exist"));
            if (ownerID.Value != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized, new ErrorView($"Room with ID equal to {roomID} does not belong to hotel with ID equal to {hotelID}"));
            return null;
        }
    }
}
