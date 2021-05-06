using AutoMapper;
using Server.Database.DataAccess;
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

namespace Server.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IRoomDataAccess _dataAccess;
        private readonly IMapper _mapper;
        private readonly IDatabaseTransaction _transaction;
        public RoomService(IRoomDataAccess dataAccess, IMapper mapper, IDatabaseTransaction transaction)
        {
            _dataAccess = dataAccess;
            _mapper = mapper;
            _transaction = transaction;
        }
        public IServiceResult AddRoom(int hotelID, string hotelRoomNumber)
        {
            if (_dataAccess.DoesRoomAlreadyExist(hotelID, hotelRoomNumber))
                return new ServiceResult(HttpStatusCode.Conflict, new Error("Room with given number already exists"));

            _transaction.BeginTransaction();
            int roomID = _dataAccess.AddRoom(hotelID, hotelRoomNumber);
            _transaction.CommitTransaction();
            return new ServiceResult(HttpStatusCode.OK, new RoomID(roomID));
        }

        public IServiceResult DeleteRoom(int hotelID, int roomID)
        {
            IServiceResult response = CheckExistanceAndOwnership(roomID, hotelID);
            if (response.StatusCode != HttpStatusCode.OK)
                return response;

            _transaction.BeginTransaction();
            if (_dataAccess.DoesRoomHaveAnyUnfinishedReservations(roomID))
            {
                _dataAccess.ChangeActivationMark(roomID, false);
                _transaction.CommitTransaction();
                return new ServiceResult(HttpStatusCode.Conflict, new Error("There are still pending reservations for this room"));
            }

            _dataAccess.UnpinRoomFromAnyOffers(roomID);
            _dataAccess.RemoveRoomFromPastReservations(roomID);
            _dataAccess.DeleteRoom(roomID);
            _transaction.CommitTransaction();
            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult GetHotelRooms(Paging paging, int hotelID, string hotelRoomNumber = null)
        {
            if (paging.pageNumber < 1 || paging.pageSize < 1)
                return new ServiceResult(HttpStatusCode.BadRequest, new Error("Invalid paging arguments"));

            List<HotelRoom> rooms = _dataAccess.GetRooms(paging, hotelID, hotelRoomNumber);
            _dataAccess.GetOffersForRooms(rooms);
            return new ServiceResult(HttpStatusCode.OK, _mapper.Map<List<HotelRoomView>>(rooms));
        }
        public IServiceResult CheckExistanceAndOwnership(int roomID, int hotelID)
        {
            int? ownerID = _dataAccess.FindRoomAndGetOwner(roomID);
            if (ownerID == null)
                return new ServiceResult(HttpStatusCode.NotFound);
            if (ownerID != hotelID)
                return new ServiceResult(HttpStatusCode.Unauthorized);
            return new ServiceResult(HttpStatusCode.OK);
        }
    }
}
