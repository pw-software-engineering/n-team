using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Models;
using Server.Services.Response;
using Server.Services.RoomService;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Server.Tests.Services
{
    public class RoomServiceTest
    {
        public RoomServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _dataAccessMock = new Mock<IRoomDataAccess>();

            _roomService = new RoomService(_dataAccessMock.Object, _mapper);
        }
        private RoomService _roomService;
        private Mock<IRoomDataAccess> _dataAccessMock;
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
            Assert.Equal(roomID, result.ResponseBody.GetType().GetProperty("roomID").GetValue(result.ResponseBody));
        }
        [Fact]
        public void DeleteRoom_200()
        {
            int hotelID = 1;
            int roomID = 1;
            int ownerID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);
            _dataAccessMock.Setup(da => da.DoesRoomHaveAnyUnfinishedReservations(roomID)).Returns(false);

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
            _dataAccessMock.Setup(da => da.DoesRoomHaveAnyUnfinishedReservations(roomID)).Returns(true);

            IServiceResult result = _roomService.DeleteRoom(hotelID, roomID);

            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        }
        [Fact]
        public void GetHotelRooms_200_ListOfHotelRooms()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            List<HotelRoom> rooms = new List<HotelRoom>();
            _dataAccessMock.Setup(da => da.GetRooms(paging, hotelID, null)).Returns(rooms);

            IServiceResult result = _roomService.GetHotelRooms(paging, hotelID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(_mapper.Map<List<HotelRoomView>>(rooms), result.ResponseBody);
        }
        [Fact]
        public void GetHotelRooms_InvalidPaging_400()
        {
            int hotelID = 1;
            Paging paging = new Paging(-1, -1);
            List<HotelRoom> rooms = new List<HotelRoom>();

            IServiceResult result = _roomService.GetHotelRooms(paging, hotelID);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnership_NotFound_404()
        {
            int roomID = -1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns((int?)null);

            IServiceResult result = _roomService.CheckExistanceAndOwnership(roomID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnership_NotOwner_401()
        {
            int roomID = -1;
            int hotelID = 1;
            int ownerID = -1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);

            IServiceResult result = _roomService.CheckExistanceAndOwnership(roomID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnership_200()
        {
            int roomID = 1;
            int hotelID = 1;
            int ownerID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);

            IServiceResult result = _roomService.CheckExistanceAndOwnership(roomID, hotelID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
