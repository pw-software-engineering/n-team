using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Exceptions;
using Server.Models;
using Server.Services.OfferService;
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

            _offerService = new OfferService(_dataAccessMock.Object, _mapper);
        }
        private OfferService _offerService;
        private Mock<IOfferDataAccess> _dataAccessMock;
        private IMapper _mapper;

        #region AddOfferTests
        [Fact]
        public void AddOffer_OfferIsAdded()
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

            int offerIDTest = _offerService.AddOffer(offerView, hotelID);

            _dataAccessMock.Verify(da => da.AddOffer(It.IsAny<Offer>()));
            _dataAccessMock.Verify(da => da.AddOfferPictures(It.IsAny<List<string>>(), offerID), Times.Once());
            Assert.Equal(offerID, offerIDTest);
        }
        #endregion

        #region DeleteOfferTests
        [Fact]
        public void DeleteOffer_NotOwner_ThrowsNotOwnerException()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            Action act = () =>_offerService.DeleteOffer(offerID, hotelID);

            Assert.Throws<NotOwnerException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void DeleteOffer_NoOffer_ThrowsNotFoundException()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            Action act = () => _offerService.DeleteOffer(offerID, hotelID);

            Assert.Throws<NotFoundException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void DeleteOffer_OfferIsDeleted()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            _offerService.DeleteOffer(offerID, hotelID);

            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.DeleteOffer(offerID), Times.Once());
        }
        #endregion

        #region GetOfferTests
        [Fact]
        public void GetOffer_NotOwner_ThrowsNotOwnerException()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            Action act = () => _offerService.GetOffer(offerID, hotelID);

            Assert.Throws<NotOwnerException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void GetOffer_NoOffer_ThrowsNotFoundException()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            Action act = () => _offerService.GetOffer(offerID, hotelID);

            Assert.Throws<NotFoundException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void GetOffer_ReturnsOfferViewObject()
        {
            int hotelID = 1;
            int offerID = 1;
            Offer offer = new Offer();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.GetOffer(offerID)).Returns(offer);

            OfferView offerTest = _offerService.GetOffer(offerID, hotelID);

            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.GetOffer(offerID), Times.Once());
        }
        #endregion

        #region GetHotelOffersTests
        [Fact]
        public void GetHotelOffers_ReturnsListOfOfferPreviewViewsObjects()
        {
            int hotelID = 1;
            List<OfferPreview> offerPreviews = new List<OfferPreview>();
            _dataAccessMock.Setup(da => da.GetHotelOffers(hotelID)).Returns(offerPreviews);

            List<OfferPreviewView> offerPreviewsTest = _offerService.GetHotelOffers(hotelID);

            _dataAccessMock.Verify(da => da.GetHotelOffers(hotelID), Times.Once());
            Assert.Equal(_mapper.Map<List<OfferPreviewView>>(offerPreviews), offerPreviewsTest);
        }
        #endregion

        #region UpdateOfferTests
        [Fact]
        public void UpdateOffer_NotOwner_ThrowsNotOwnerException()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferUpdateInfo offerUpdateInfo = new OfferUpdateInfo();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            Action act = () => _offerService.UpdateOffer(offerID, hotelID, offerUpdateInfo);

            Assert.Throws<NotOwnerException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void UpdateOffer_NoOffer_ThrowsNotFoundException()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferUpdateInfo offerUpdateInfo = new OfferUpdateInfo();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            Action act = () => _offerService.UpdateOffer(offerID, hotelID, offerUpdateInfo);

            Assert.Throws<NotFoundException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void UpdateOffer_OfferIsUpdated()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferUpdateInfo offerUpdateInfo = new OfferUpdateInfo();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            _offerService.UpdateOffer(offerID, hotelID, offerUpdateInfo);

            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.UpdateOffer(offerID, offerUpdateInfo), Times.Once());
        }
        #endregion

        #region CheckExceptionsTests
        [Fact]
        public void CheckExceptions_NoExceptionsAreThrown()
        {
            int? ownerID = 1;
            int hotelID = 1;

            _offerService.CheckExceptions(ownerID, hotelID);
        }
        [Fact]
        public void CheckExceptions_NoOffer_ThrowsNotFoundException()
        {
            int? ownerID = null;
            int hotelID = 1;

            Action act = () => _offerService.CheckExceptions(ownerID, hotelID);

            Assert.Throws<NotFoundException>(act);
        }
        [Fact]
        public void CheckExceptions_ThrowsNotOwnerException()
        {
            int? ownerID = 1;
            int hotelID = 2;

            Action act = () => _offerService.CheckExceptions(ownerID, hotelID);

            Assert.Throws<NotOwnerException>(act);
        }
        #endregion
    }
}