using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.RequestModels.Hotel;
using Server.Services.Hotel;
using Server.Services.Result;
using Server.ViewModels.Hotel;
using Xunit;

namespace Server.Tests.Services.Hotel
{
    public class OfferServiceTest
    {
        public OfferServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new HotelAutoMapperProfile());
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
        public void AddOffer_200()
        {
            int hotelID = 1;
            OfferInfo offerInfo = new OfferInfo 
            {
                OfferTitle = "TestOfferTitle4", 
                OfferPreviewPicture = "TestOfferPreviewPicture4", 
                IsActive = true, 
                CostPerChild = 40, 
                CostPerAdult = 44, 
                MaxGuests = 4, 
                Description = "TestDescription4",
                Pictures = null
            };
            int offerID = 1;
            _dataAccessMock.Setup(da => da.AddOffer(hotelID, It.IsAny<OfferInfo>())).Returns(offerID);

            IServiceResult response = _offerService.AddOffer(hotelID, offerInfo);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.AddOffer(It.IsAny<int>(), It.IsAny<OfferInfo>()), Times.Once);
            _dataAccessMock.Verify(da => da.AddOfferPictures(offerID, It.IsAny<List<string>>()), Times.Once());
            Assert.Equal(offerID, (response.Result as OfferIDView).OfferID);
        }
        #endregion

        #region DeleteOfferTests
        [Fact]
        public void DeleteOffer_NotOwner_401()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            IServiceResult response = _offerService.DeleteOffer(hotelID, offerID);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void DeleteOffer_NoOffer_404()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult response = _offerService.DeleteOffer(hotelID, offerID);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void DeleteOffer_200()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            IServiceResult response = _offerService.DeleteOffer(hotelID, offerID);

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

            IServiceResult response = _offerService.DeleteOffer(hotelID, offerID);

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

            IServiceResult response = _offerService.GetOffer(hotelID, offerID);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void GetOffer_NoOffer_404()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult response = _offerService.GetOffer(hotelID, offerID);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void GetOffer_200_OfferView()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferView offer = new OfferView();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.GetOffer(offerID)).Returns(offer);

            IServiceResult response = _offerService.GetOffer(hotelID, offerID);

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
            List<OfferPreviewView> offerPreviews = new List<OfferPreviewView>();
            _dataAccessMock.Setup(da => da.GetHotelOffers(hotelID, paging, null)).Returns(offerPreviews);

            IServiceResult response = _offerService.GetHotelOffers(hotelID, paging);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.GetHotelOffers(hotelID, paging, null), Times.Once());
            Assert.Equal(_mapper.Map<List<OfferPreviewView>>(offerPreviews), response.Result);
        }
        #endregion

        #region UpdateOfferTests
        [Fact]
        public void UpdateOffer_NotOwner_401()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferInfoUpdate offerInfoUpdate = new OfferInfoUpdate();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            IServiceResult response = _offerService.UpdateOffer(hotelID, offerID, offerInfoUpdate);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void UpdateOffer_NoOffer_404()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferInfoUpdate offerInfoUpdate = new OfferInfoUpdate();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult response = _offerService.UpdateOffer(hotelID, offerID, offerInfoUpdate);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void UpdateOffer_200_OfferIsUpdated()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferInfoUpdate offerUpdateInfo = new OfferInfoUpdate();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            IServiceResult response = _offerService.UpdateOffer(hotelID, offerID, offerUpdateInfo);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.UpdateOffer(offerID, offerUpdateInfo), Times.Once());
        }
        #endregion

        #region CheckExceptionsTests
        [Fact]
        public void CheckExistanceAndOwnership_ReturnsNull()
        {
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            IServiceResult result = _offerService.CheckExistanceAndOwnership(hotelID, offerID);

            Assert.Null(result);
        }
        [Fact]
        public void CheckExistanceAndOwnership_NoOffer_404()
        {
            int offerID = -1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?) null);

            IServiceResult response = _offerService.CheckExistanceAndOwnership(hotelID, offerID);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public void CheckExistanceAndOwnershi_NotOwner_401()
        {
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(3);

            IServiceResult response = _offerService.CheckExistanceAndOwnership(hotelID, offerID);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion
    }
}