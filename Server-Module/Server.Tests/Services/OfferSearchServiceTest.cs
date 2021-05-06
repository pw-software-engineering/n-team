using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess.OfferSearch;
using Server.Database.DatabaseTransaction;
using Server.Models;
using Server.RequestModels;
using Server.Services.OfferSearchService;
using Server.Services.Result;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Server.Tests.Services
{
    public class OfferSearchServiceTest
    {
        public OfferSearchServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _offerSearchDataAccessMock = new Mock<IOfferSearchDataAccess>();
            _offerSearchDataAccessMock.Setup(da => da.CheckHotelExistence(-1)).Returns(false);
            _offerSearchDataAccessMock.Setup(da => da.CheckHotelExistence(1)).Returns(true);
            _offerSearchDataAccessMock.Setup(da => da.CheckHotelOfferExistence(1, 4)).Returns(false);
            _offerSearchDataAccessMock.Setup(da => da.CheckHotelOfferExistence(1, 1)).Returns(true);
            _transactionMock = new Mock<IDatabaseTransaction>();

            _offerSearchService = new OfferSearchService(_mapper, _offerSearchDataAccessMock.Object, _transactionMock.Object);
        }
        private OfferSearchService _offerSearchService;
        private Mock<IOfferSearchDataAccess> _offerSearchDataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;
        private IMapper _mapper;

        #region GetHotelOffers
        [Fact]
        public void GetHotelOffers_NullPagingOrNullOfferFilter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _offerSearchService.GetHotelOffers(-1, new Paging(), null));
            Assert.Throws<ArgumentNullException>(() => _offerSearchService.GetHotelOffers(-1, null, new OfferFilter()));
            Assert.Throws<ArgumentNullException>(() => _offerSearchService.GetHotelOffers(-1, null, null));
        }

        [Fact]
        public void GetHotelOffers_NonExistentHotel_404()
        {
            int hotelID = -1;

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, new Paging(), new OfferFilter());

            Assert.Equal(HttpStatusCode.NotFound, serviceResult.StatusCode);
        }

        [Fact]
        public void GetHotelOffers_InvalidPaging_400()
        {
            int hotelID = 1;
            Paging paging = new Paging(-1, -1);

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, paging, new OfferFilter());

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is Error);
        }

        [Fact]
        public void GetHotelOffers_ValidPagingAndFromGreaterThanTo_400()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(2000, 10, 10),
                ToTime = new DateTime(1999, 10, 10)
            };

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, paging, offerFilter);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is Error);
        }

        [Fact]
        public void GetHotelOffers_ValidPagingAndNegativeMinGuests_400()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(2000, 10, 10),
                ToTime = new DateTime(2010, 10, 10),
                MinGuests = -1
            };

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, paging, offerFilter);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is Error);
        }

        [Fact]
        public void GetHotelOffers_ValidPagingAndFromAndToButNegativeMinCostOrMaxCost_400()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            OfferFilter offerFilterMinCost = new OfferFilter()
            {
                FromTime = new DateTime(2000, 10, 10),
                ToTime = new DateTime(2010, 10, 10),
                CostMin = -1
            };
            OfferFilter offerFilterMaxCost = new OfferFilter()
            {
                FromTime = new DateTime(2000, 10, 10),
                ToTime = new DateTime(2010, 10, 10),
                CostMax = -1
            };

            IServiceResult serviceResultMinCost = _offerSearchService.GetHotelOffers(hotelID, paging, offerFilterMinCost);
            IServiceResult serviceResultMaxCost = _offerSearchService.GetHotelOffers(hotelID, paging, offerFilterMaxCost);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResultMinCost.StatusCode);
            Assert.True(serviceResultMinCost.Result is Error);
            Assert.Equal(HttpStatusCode.BadRequest, serviceResultMaxCost.StatusCode);
            Assert.True(serviceResultMaxCost.Result is Error);
        }

        [Fact]
        public void GetHotelOffers_ValidPagingAndFromAndToButMinCostGreaterThanMaxCost_400()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(2000, 10, 10),
                ToTime = new DateTime(2010, 10, 10),
                CostMin = 10,
                CostMax = 5
            };

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, paging, offerFilter);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is Error);
        }

        [Fact]
        public void GetHotelOffers_ValidPagingAndOfferFilter_200()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(2000, 10, 10),
                ToTime = new DateTime(2010, 10, 10),
                CostMin = 5,
                CostMax = 10,
                MinGuests = 0
            };
            ClientOfferPreview offerPreview = new ClientOfferPreview()
            {
                OfferID = 1,
                CostPerAdult = 10.0,
                CostPerChild = 7.3,
                MaxGuests = 4,
                OfferTitle = "TestTitle",
                OfferPreviewPicture = "TestPicture"
            };
            List<ClientOfferPreview> offerPreviews = new List<ClientOfferPreview>()
            {
                offerPreview,
                offerPreview,
                offerPreview
            };
            _offerSearchDataAccessMock.Setup(da => da.GetHotelOffers(hotelID, paging, offerFilter)).Returns(offerPreviews);

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, paging, offerFilter);
            List<ClientOfferPreview> clientOfferPreviews = serviceResult.Result as List<ClientOfferPreview>;

            Assert.Equal(HttpStatusCode.OK, serviceResult.StatusCode);
            Assert.Equal(offerPreviews.Count, clientOfferPreviews.Count);
            for(int i = 0; i < clientOfferPreviews.Count; i++)
            {
                Assert.Equal(offerPreview.CostPerAdult, clientOfferPreviews[i].CostPerAdult);
                Assert.Equal(offerPreview.CostPerChild, clientOfferPreviews[i].CostPerChild);
                Assert.Equal(offerPreview.OfferID, clientOfferPreviews[i].OfferID);
                Assert.Equal(offerPreview.MaxGuests, clientOfferPreviews[i].MaxGuests);
                Assert.Equal(offerPreview.OfferTitle, clientOfferPreviews[i].OfferTitle);
                Assert.Equal(offerPreview.OfferPreviewPicture, clientOfferPreviews[i].OfferPreviewPicture);
            }
        }
        #endregion

        #region GetHotelOfferDetails
        [Fact]
        public void GetHotelOfferDetails_NonExistentHotelOrOffer_404()
        {
            int hotelID = 1;
            int offerID = 4;

            IServiceResult serviceResult = _offerSearchService.GetHotelOfferDetails(hotelID, offerID);

            Assert.Equal(HttpStatusCode.NotFound, serviceResult.StatusCode);
        }

        [Fact]
        public void GetHotelOfferDetails_ValidHotelIDAndOfferID_200()
        {
            int hotelID = 1;
            int offerID = 1;
            List<string> offerPictures = new List<string>()
            {
                "TestPicture1",
                "TestPicture2"
            };
            ClientOffer clientOffer = new ClientOffer()
            {
                IsDeleted = false,
                IsActive = true,
                OfferDescription = "TestDescription",
                CostPerAdult = 10.3,
                CostPerChild = 7.7,
                MaxGuests = 8,
                OfferTitle = "TestTitle"
            };
            _offerSearchDataAccessMock.Setup(da => da.GetHotelOfferPictures(offerID)).Returns(offerPictures);
            _offerSearchDataAccessMock.Setup(da => da.GetHotelOfferDetails(offerID)).Returns(clientOffer);

            IServiceResult serviceResult = _offerSearchService.GetHotelOfferDetails(hotelID, offerID);
            ClientOffer resultClientOffer = serviceResult.Result as ClientOffer;

            Assert.Equal(HttpStatusCode.OK, serviceResult.StatusCode);
            Assert.Equal(clientOffer.IsDeleted, resultClientOffer.IsDeleted);
            Assert.Equal(clientOffer.IsActive, resultClientOffer.IsActive);
            Assert.Equal(clientOffer.OfferDescription, resultClientOffer.OfferDescription);
            Assert.Equal(clientOffer.CostPerAdult, resultClientOffer.CostPerAdult);
            Assert.Equal(clientOffer.CostPerChild, resultClientOffer.CostPerChild);
            Assert.Equal(clientOffer.MaxGuests, resultClientOffer.MaxGuests);
            Assert.Equal(clientOffer.OfferTitle, resultClientOffer.OfferTitle);
            Assert.Equal(offerPictures.Count, resultClientOffer.OfferPictures.Count);
            for(int i = 0; i < offerPictures.Count; i++)
            {
                Assert.Equal(offerPictures[i], resultClientOffer.OfferPictures[i]);
            }            
        }
        #endregion
    }
}
