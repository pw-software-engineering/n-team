using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;
using Server.Services.Result;
using Server.ViewModels;
using Xunit;

namespace Server.Tests.Services.Client
{
    public class ReservationServiceTest
    {
        public ReservationServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new ClientAutoMapperProfile());
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
        public void AddReservation_NoOffer_404()
        {
            int hotelID = 1;
            int offerID = -1;
            int userID = 1;
            ReservationInfo reservation = new ReservationInfo()
            {
                From = DateTime.Now,
                To = DateTime.Now,
                NumberOfChildren = 1,
                NumberOfAdults = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult result = _reservationService.AddReservation(hotelID, offerID, userID, reservation);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void AddReservation_ProvidedHotelIDIsNotOwnerID_404()
        {
            int hotelID = -1;
            int offerID = 1;
            int userID = 1;
            ReservationInfo reservation = new ReservationInfo()
            {
                From = DateTime.Now,
                To = DateTime.Now,
                NumberOfChildren = 1,
                NumberOfAdults = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(1);

            IServiceResult result = _reservationService.AddReservation(hotelID, offerID, userID, reservation);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void AddReservation_NoRoomIsAvailable_400()
        {
            int hotelID = 1;
            int offerID = 1;
            int userID = 1;
            ReservationInfo reservation = new ReservationInfo()
            {
                From = DateTime.Now,
                To = DateTime.Now,
                NumberOfChildren = 1,
                NumberOfAdults = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(1);
            List<int> roomIDs = new List<int>() { 1 };
            _dataAccessMock.Setup(da => da.GetOfferRoomIDs(offerID)).Returns(roomIDs);
            _dataAccessMock.Setup(da => da.IsRoomAvailable(It.IsAny<int>(), reservation.From, reservation.To)).Returns(false);

            IServiceResult result = _reservationService.AddReservation(hotelID, offerID, userID, reservation);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void AddReservation_200()
        {
            int hotelID = 1;
            int offerID = 1;
            int userID = 1;
            ReservationInfo reservation = new ReservationInfo()
            {
                From = DateTime.Now,
                To = DateTime.Now,
                NumberOfChildren = 1,
                NumberOfAdults = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(userID);
            List<int> roomIDs = new List<int>() { 1 };
            _dataAccessMock.Setup(da => da.GetOfferRoomIDs(offerID)).Returns(roomIDs);
            _dataAccessMock.Setup(da => da.IsRoomAvailable(roomIDs[0], reservation.From, reservation.To)).Returns(true);

            IServiceResult result = _reservationService.AddReservation(hotelID, offerID, userID, reservation);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public void CancelReservation_NoReservation_404()
        {
            int reservationID = -1;
            int userID = 1;
            _dataAccessMock.Setup(da => da.FindReservationAndGetOwner(reservationID)).Returns((int?)null);

            IServiceResult result = _reservationService.CancelReservation(reservationID, userID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void CancelReservation_NotOwner_401()
        {
            int reservationID = 1;
            int userID = -1;
            _dataAccessMock.Setup(da => da.FindReservationAndGetOwner(reservationID)).Returns(1);

            IServiceResult result = _reservationService.CancelReservation(reservationID, userID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void CancelReservation_ReservationHasBegun_400()
        {
            int reservationID = 1;
            int userID = 1;
            _dataAccessMock.Setup(da => da.FindReservationAndGetOwner(reservationID)).Returns(userID);
            _dataAccessMock.Setup(da => da.HasReservationBegun(reservationID)).Returns(true);

            IServiceResult result = _reservationService.CancelReservation(reservationID, userID);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void CancelReservation_200()
        {
            int reservationID = 1;
            int userID = 1;
            _dataAccessMock.Setup(da => da.FindReservationAndGetOwner(reservationID)).Returns(userID);
            _dataAccessMock.Setup(da => da.HasReservationBegun(reservationID)).Returns(false);

            IServiceResult result = _reservationService.CancelReservation(reservationID, userID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}

