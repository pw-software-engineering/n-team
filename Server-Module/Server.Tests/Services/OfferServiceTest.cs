using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using Server.Models;
using Server.RequestModels;
using Server.Services.OfferService;
using Server.Services.Result;
using Server.ViewModels;
using Xunit;

namespace Server.Tests.Services
{
    public class OfferServiceTest
    {
        public OfferServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _dataAccessMock = new Mock<IOfferDataAccess>();
            _transactionMock = new Mock<IDatabaseTransaction>();

            _offerService = new OfferService(_dataAccessMock.Object, _mapper, _transactionMock.Object);
        }
        private OfferService _offerService;
        private Mock<IOfferDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;
        private IMapper _mapper;

        #region AddOfferTests
        [Fact]
        public void AddOffer_200_OfferID()
        {
            int hotelID = 1;
            OfferView offerView = new OfferView 
            {
                offerTitle = "TestOfferTitle4", 
                offerPreviewPicture = "TestOfferPreviewPicture4", 
                isActive = true, 
                costPerChild = 40, 
                costPerAdult = 44, 
                maxGuests = 4, 
                description = "TestDescription4" 
            };
            int offerID = 1;
            _dataAccessMock.Setup(da => da.AddOffer(It.IsAny<Offer>())).Returns(offerID);

            IServiceResult response = _offerService.AddOffer(offerView, hotelID);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.AddOffer(It.IsAny<Offer>()));
            _dataAccessMock.Verify(da => da.AddOfferPictures(It.IsAny<List<string>>(), offerID), Times.Once());
            Assert.Equal(offerID, ((OfferID)response.Result).offerID);
        }
        #endregion

        #region DeleteOfferTests
        [Fact]
        public void DeleteOffer_NotOwner_401()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            IServiceResult response = _offerService.DeleteOffer(offerID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void DeleteOffer_NoOffer_404()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult response = _offerService.DeleteOffer(offerID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void DeleteOffer_200()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            IServiceResult response = _offerService.DeleteOffer(offerID, hotelID);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.DeleteOffer(offerID), Times.Once());
        }
        [Fact]
        public void DeleteOffer_PendingReservations_409()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.AreThereUnfinishedReservationsForOffer(offerID)).Returns(true);

            IServiceResult response = _offerService.DeleteOffer(offerID, hotelID);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.AreThereUnfinishedReservationsForOffer(offerID), Times.Once());
        }
        #endregion

        #region GetOfferTests
        [Fact]
        public void GetOffer_NotOwner_401()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            IServiceResult response = _offerService.GetOffer(offerID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void GetOffer_NoOffer_404()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult response = _offerService.GetOffer(offerID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void GetOffer_200_Offer()
        {
            int hotelID = 1;
            int offerID = 1;
            Offer offer = new Offer();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.GetOffer(offerID)).Returns(offer);

            IServiceResult response = _offerService.GetOffer(offerID, hotelID);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.GetOffer(offerID), Times.Once());
        }
        #endregion

        #region GetHotelOffersTests
        [Fact]
        public void GetHotelOffers_200_ListOfHotelOffers()
        {
            int hotelID = 3;
            Paging paging = new Paging();
            List<OfferPreview> offerPreviews = new List<OfferPreview>();
            _dataAccessMock.Setup(da => da.GetHotelOffers(paging, hotelID, null)).Returns(offerPreviews);

            IServiceResult response = _offerService.GetHotelOffers(paging, hotelID);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.GetHotelOffers(paging, hotelID, null), Times.Once());
            Assert.Equal(_mapper.Map<List<OfferPreviewView>>(offerPreviews), response.Result);
        }
        #endregion

        #region UpdateOfferTests
        [Fact]
        public void UpdateOffer_NotOwner_401()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferUpdateInfo offerUpdateInfo = new OfferUpdateInfo();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            IServiceResult response = _offerService.UpdateOffer(offerID, hotelID, offerUpdateInfo);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void UpdateOffer_NoOffer_404()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferUpdateInfo offerUpdateInfo = new OfferUpdateInfo();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult response = _offerService.UpdateOffer(offerID, hotelID, offerUpdateInfo);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void UpdateOffer_200_OfferIsUpdated()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferUpdateInfo offerUpdateInfo = new OfferUpdateInfo();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            IServiceResult response = _offerService.UpdateOffer(offerID, hotelID, offerUpdateInfo);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.UpdateOffer(offerID, offerUpdateInfo), Times.Once());
        }
        #endregion

        #region CheckExceptionsTests
        [Fact]
        public void CheckExistanceAndOwnership_200()
        {
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            IServiceResult response = _offerService.CheckExistanceAndOwnership(offerID, hotelID);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnership_NoOffer_404()
        {
            int offerID = -1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?) null);

            IServiceResult response = _offerService.CheckExistanceAndOwnership(offerID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnershi_NotOwner_401()
        {
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(3);

            IServiceResult response = _offerService.CheckExistanceAndOwnership(offerID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion
    }
}