using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.Services.Hotel;
using Server.Services.Result;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Server.Tests.Services.Hotel
{
    public class ReservationServiceTest
    {
        public ReservationServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new HotelAutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _dataAccessMock = new Mock<IReservationDataAccess>();
            _transactionMock = new Mock<IDatabaseTransaction>();

            _reservationService = new ReservationService(_dataAccessMock.Object, _mapper, _transactionMock.Object);
        }
        private ReservationService _reservationService;
        private Mock<IReservationDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;
        private IMapper _mapper;

        [Fact]
        public void CheckRoomExistenceAndOwnership_RoomExistsAndOwner_ReturnsNull()
        {
            int hotelID = 1;
            int roomID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);

            IServiceResult result = _reservationService.CheckRoomExistanceAndOwnership(hotelID, roomID);

            Assert.Null(result);
            _dataAccessMock.Verify(da => da.FindRoomAndGetOwner(roomID), Times.Once);
        }
        [Fact]
        public void CheckRoomExistenceAndOwnership_RoomDoesNotExist_404()
        {
            int hotelID = 1;
            int roomID = -1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns((int?)null);

            IServiceResult result = _reservationService.CheckRoomExistanceAndOwnership(hotelID, roomID);

            Assert.Equal(HttpStatusCode.NotFound,result.StatusCode);
            _dataAccessMock.Verify(da => da.FindRoomAndGetOwner(roomID), Times.Once);
        }
        [Fact]
        public void CheckRoomExistenceAndOwnership_NotOwner_401()
        {
            int hotelID = -1;
            int roomID = 1;
            int ownerID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(ownerID);

            IServiceResult result = _reservationService.CheckRoomExistanceAndOwnership(hotelID, roomID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            _dataAccessMock.Verify(da => da.FindRoomAndGetOwner(roomID), Times.Once);
        }
        [Fact]
        public void GetReservations_InvalidPagingArguments_400()
        {
            int hotelID = 1;
            Paging paging = new Paging() { PageNumber = -1, PageSize = -2 };

            IServiceResult result = _reservationService.GetReservations(hotelID, null, null, paging);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void GetReservations_RoomSpecified_200()
        {
            int hotelID = 1;
            bool currentOnly = true;
            int roomID = 1;
            Paging paging = new Paging() { PageNumber = 1, PageSize = 10 };
            ReservationView reservation = new ReservationView()
            {
                ReservationID = 1,
                OfferID = 1,
                AdultsCount = 1,
                ChildrenCount = 1,
                FromTime = DateTime.Now,
                ToTime = DateTime.Now
            };
            ClientView client = new ClientView()
            {
                ClientID = 1,
                Name = "Name",
                Surname = "Surname"
            };
            RoomView room = new RoomView()
            {
                RoomID = 1,
                HotelRoomNumber = "RoomNumber"
            };
            ReservationObjectView reservationObject = new ReservationObjectView()
            {
                Reservation = reservation,
                Client = new ClientView()
                {
                    ClientID = client.ClientID
                },
                Room = new RoomView()
                {
                    RoomID = room.RoomID
                }
            };
            List<ReservationObjectView> reservationObjects = new List<ReservationObjectView>() { reservationObject };
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.GetReservations(hotelID, roomID, currentOnly, paging)).Returns(reservationObjects);
            _dataAccessMock.Setup(da => da.GetClientDetails(client.ClientID)).Returns(client);
            _dataAccessMock.Setup(da => da.GetRoomDetails(room.RoomID)).Returns(room);

            IServiceResult result = _reservationService.GetReservations(hotelID, currentOnly, roomID, paging);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);        
            Assert.Equal(reservation, (result.Result as List<ReservationObjectView>)[0].Reservation);
            Assert.Equal(client, (result.Result as List<ReservationObjectView>)[0].Client);
            Assert.Equal(room, (result.Result as List<ReservationObjectView>)[0].Room);
            _dataAccessMock.Verify(da => da.FindRoomAndGetOwner(roomID), Times.Once);
        }
        [Fact]
        public void GetReservations_RoomNotSpecified_200()
        {
            int hotelID = 1;
            bool currentOnly = true;
            Paging paging = new Paging() { PageNumber = 1, PageSize = 10 };
            ReservationView reservation = new ReservationView()
            {
                ReservationID = 1,
                OfferID = 1,
                AdultsCount = 1,
                ChildrenCount = 1,
                FromTime = DateTime.Now,
                ToTime = DateTime.Now
            };
            ClientView client = new ClientView()
            {
                ClientID = 1,
                Name = "Name",
                Surname = "Surname"
            };
            RoomView room = new RoomView()
            {
                RoomID = 1,
                HotelRoomNumber = "RoomNumber"
            };
            ReservationObjectView reservationObject = new ReservationObjectView()
            {
                Reservation = reservation,
                Client = new ClientView()
                {
                    ClientID = client.ClientID
                },
                Room = new RoomView()
                {
                    RoomID = room.RoomID
                }
            };
            List<ReservationObjectView> reservationObjects = new List<ReservationObjectView>() { reservationObject };
            _dataAccessMock.Setup(da => da.GetReservations(hotelID, null, currentOnly, paging)).Returns(reservationObjects);
            _dataAccessMock.Setup(da => da.GetClientDetails(client.ClientID)).Returns(client);
            _dataAccessMock.Setup(da => da.GetRoomDetails(room.RoomID)).Returns(room);

            IServiceResult result = _reservationService.GetReservations(hotelID, currentOnly, null, paging);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(reservation, (result.Result as List<ReservationObjectView>)[0].Reservation);
            Assert.Equal(client, (result.Result as List<ReservationObjectView>)[0].Client);
            Assert.Equal(room, (result.Result as List<ReservationObjectView>)[0].Room);
            _dataAccessMock.Verify(da => da.FindRoomAndGetOwner(It.IsAny<int>()), Times.Never);
        }
    }
}
