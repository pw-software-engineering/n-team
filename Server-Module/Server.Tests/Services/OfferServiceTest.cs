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
            _dataAccessMock = new Mock<IDataAccess>();

            _offerService = new OfferService(_dataAccessMock.Object, _mapper);
        }
        private OfferService _offerService;
        private Mock<IDataAccess> _dataAccessMock;
        private IMapper _mapper;

        [Fact]
        public void Can_Add_Offer()
        {
            int hotelID = 1;
            OfferView offerView = new OfferView {offerTitle = "TestOfferTitle4", offerPreviewPicture = "TestOfferPreviewPicture4", isActive = true, costPerChild = 40, costPerAdult = 44, maxGuests = 4, description = "TestDescription4" };
            int offerID = 1;
            _dataAccessMock.Setup(da => da.AddOffer(It.IsAny<Offer>())).Returns(offerID);

            int offerIDTest = _offerService.AddOffer(offerView, hotelID);

            _dataAccessMock.Verify(da => da.AddOffer(It.IsAny<Offer>()));
            _dataAccessMock.Verify(da => da.AddOfferPictures(It.IsAny<List<string>>(), offerID), Times.Once());
            Assert.Equal(offerID, offerIDTest);
        }

        [Fact]
        public void Can_NotOwner_DeleteOffer()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            Action act = () =>_offerService.DeleteOffer(offerID, hotelID);

            Assert.Throws<NotOwnerException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void Can_NoOffer_DeleteOffer()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            Action act = () => _offerService.DeleteOffer(offerID, hotelID);

            Assert.Throws<NotFoundException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void Can_DeleteOffer()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            _offerService.DeleteOffer(offerID, hotelID);

            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.DeleteOffer(offerID), Times.Once());
        }
        [Fact]
        public void Can_NotOwner_GetOffer()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(2);

            Action act = () => _offerService.GetOffer(offerID, hotelID);

            Assert.Throws<NotOwnerException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void Can_NoOffer_GetOffer()
        {
            int hotelID = 1;
            int offerID = 1;
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            Action act = () => _offerService.GetOffer(offerID, hotelID);

            Assert.Throws<NotFoundException>(act);
            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
        }
        [Fact]
        public void Can_GetOffer()
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
        [Fact]
        public void Can_GetHotelOffers()
        {
            int hotelID = 1;
            List<OfferPreview> offerPreviews = new List<OfferPreview>();
            _dataAccessMock.Setup(da => da.GetHotelOffers(hotelID)).Returns(offerPreviews);

            List<OfferPreviewView> offerPreviewsTest = _offerService.GetHotelOffers(hotelID);

            _dataAccessMock.Verify(da => da.GetHotelOffers(hotelID), Times.Once());
            Assert.Equal(_mapper.Map<List<OfferPreviewView>>(offerPreviews), offerPreviewsTest);
        }
        [Fact]
        public void Can_NotOwner_UpdateOffer()
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
        public void Can_NoOffer_UpdateOffer()
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
        public void Can_UpdateOffer()
        {
            int hotelID = 1;
            int offerID = 1;
            OfferUpdateInfo offerUpdateInfo = new OfferUpdateInfo();
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);

            _offerService.UpdateOffer(offerID, hotelID, offerUpdateInfo);

            _dataAccessMock.Verify(da => da.FindOfferAndGetOwner(offerID), Times.Once());
            _dataAccessMock.Verify(da => da.UpdateOffer(offerID, offerUpdateInfo), Times.Once());
        }
        [Fact]
        public void Can_CheckExceptions()
        {
            int? ownerID = 1;
            int hotelID = 1;

            _offerService.CheckExceptions(ownerID, hotelID);
        }
        [Fact]
        public void Can_NoOffer_CheckExceptions()
        {
            int? ownerID = null;
            int hotelID = 1;

            Action act = () => _offerService.CheckExceptions(ownerID, hotelID);

            Assert.Throws<NotFoundException>(act);
        }
        [Fact]
        public void Can_NotOwner_CheckExceptions()
        {
            int? ownerID = 1;
            int hotelID = 2;

            Action act = () => _offerService.CheckExceptions(ownerID, hotelID);

            Assert.Throws<NotOwnerException>(act);
        }

    }
}
