using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.Services.Hotel;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Server.Tests.Services.Hotel
{
    public class RoomServiceTest
    {
        public RoomServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new HotelAutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _dataAccessMock = new Mock<IRoomDataAccess>();
            _transactionMock = new Mock<IDatabaseTransaction>();

            _roomService = new RoomService(_dataAccessMock.Object, _mapper, _transactionMock.Object);
        }
        private RoomService _roomService;
        private Mock<IRoomDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;
        private IMapper _mapper;
        [Fact]
        public void AddRoom_RoomAlreadyExists_409()
        {
            int hotelID = 1;
            string hotelRoomNumber = "RoomNumber";
            _dataAccessMock.Setup(da => da.DoesRoomAlreadyExist(hotelID, hotelRoomNumber)).Returns(true);

            IServiceResult result = _roomService.AddRoom(hotelID, hotelRoomNumber);

            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        }
        [Fact]
        public void AddRoom_200_RoomID()
        {
            int hotelID = 1;
            string hotelRoomNumber = "RoomNumber";
            int roomID = 1;
            _dataAccessMock.Setup(da => da.DoesRoomAlreadyExist(hotelID, hotelRoomNumber)).Returns(false);
            _dataAccessMock.Setup(da => da.AddRoom(hotelID, hotelRoomNumber)).Returns(roomID);

            IServiceResult result = _roomService.AddRoom(hotelID, hotelRoomNumber);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(roomID, (result.Result as RoomIDView).RoomID);
        }
        [Fact]
        public void DeleteRoom_200()
        {
            int hotelID = 1;
            int roomID = 1;
            int ownerID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);
            _dataAccessMock.Setup(da => da.CheckAnyUnfinishedReservations(roomID)).Returns(false);

            IServiceResult result = _roomService.DeleteRoom(hotelID, roomID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public void DeleteRoom_NoRoom_404()
        {
            int hotelID = 1;
            int roomID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns((int?)null);

            IServiceResult result = _roomService.DeleteRoom(hotelID, roomID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void DeleteRoom_NotOwner_401()
        {
            int hotelID = 1;
            int roomID = 1;
            int ownerID = 2;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);

            IServiceResult result = _roomService.DeleteRoom(hotelID, roomID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void DeleteRoom_PendingReservations_409()
        {
            int hotelID = 1;
            int roomID = 1;
            int ownerID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);
            _dataAccessMock.Setup(da => da.CheckAnyUnfinishedReservations(roomID)).Returns(true);

            IServiceResult result = _roomService.DeleteRoom(hotelID, roomID);

            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        }
        [Fact]
        public void GetHotelRooms_200_ListOfHotelRooms()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            List<HotelRoomView> rooms = new List<HotelRoomView>();
            _dataAccessMock.Setup(da => da.GetRooms(hotelID, paging, null)).Returns(rooms);

            IServiceResult result = _roomService.GetHotelRooms(hotelID, paging);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(_mapper.Map<List<HotelRoomView>>(rooms), result.Result);
        }
        [Fact]
        public void GetHotelRooms_InvalidPaging_400()
        {
            int hotelID = 1;
            Paging paging = new Paging(-1, -1);
            List<HotelRoomView> rooms = new List<HotelRoomView>();

            IServiceResult result = _roomService.GetHotelRooms(hotelID, paging);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnership_NotFound_404()
        {
            int roomID = -1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns((int?)null);

            IServiceResult result = _roomService.CheckExistanceAndOwnership(hotelID, roomID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnership_NotOwner_401()
        {
            int roomID = -1;
            int hotelID = 1;
            int ownerID = -1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);

            IServiceResult result = _roomService.CheckExistanceAndOwnership(hotelID, roomID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnership_ReturnsNull()
        {
            int roomID = 1;
            int hotelID = 1;
            int ownerID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);

            IServiceResult result = _roomService.CheckExistanceAndOwnership(hotelID, roomID);

            Assert.Null(result);
        }
    }
}
