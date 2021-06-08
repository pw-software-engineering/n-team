using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess.Client;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Client;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Server.Tests.Services.Client
{
    public class OfferSearchServiceTest
    {
        public OfferSearchServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new ClientAutoMapperProfile());
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
            Assert.Throws<ArgumentNullException>(() => _offerSearchService.GetHotelOffers(-1, null, new Paging()));
            Assert.Throws<ArgumentNullException>(() => _offerSearchService.GetHotelOffers(-1, new OfferFilter(), null));
            Assert.Throws<ArgumentNullException>(() => _offerSearchService.GetHotelOffers(-1, null, null));
        }

        [Fact]
        public void GetHotelOffers_NonExistentHotel_404()
        {
            int hotelID = -1;

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, new OfferFilter(), new Paging());

            Assert.Equal(HttpStatusCode.NotFound, serviceResult.StatusCode);
        }

        [Fact]
        public void GetHotelOffers_InvalidPaging_400()
        {
            int hotelID = 1;
            Paging paging = new Paging(-1, -1);

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, new OfferFilter(), paging);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is ErrorView);
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

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, offerFilter, paging);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is ErrorView);
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

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, offerFilter, paging);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is ErrorView);
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

            IServiceResult serviceResultMinCost = _offerSearchService.GetHotelOffers(hotelID, offerFilterMinCost, paging);
            IServiceResult serviceResultMaxCost = _offerSearchService.GetHotelOffers(hotelID, offerFilterMaxCost, paging);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResultMinCost.StatusCode);
            Assert.True(serviceResultMinCost.Result is ErrorView);
            Assert.Equal(HttpStatusCode.BadRequest, serviceResultMaxCost.StatusCode);
            Assert.True(serviceResultMaxCost.Result is ErrorView);
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

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, offerFilter, paging);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is ErrorView);
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
            OfferPreviewView offerPreview = new OfferPreviewView()
            {
                OfferID = 1,
                CostPerAdult = 10.0,
                CostPerChild = 7.3,
                MaxGuests = 4,
                OfferTitle = "TestTitle",
                OfferPreviewPicture = "TestPicture"
            };
            List<OfferPreviewView> offerPreviews = new List<OfferPreviewView>()
            {
                offerPreview,
                offerPreview,
                offerPreview
            };
            _offerSearchDataAccessMock.Setup(da => da.GetHotelOffers(hotelID, offerFilter, paging)).Returns(offerPreviews);

            IServiceResult serviceResult = _offerSearchService.GetHotelOffers(hotelID, offerFilter, paging);
            List<OfferPreviewView> clientOfferPreviews = serviceResult.Result as List<OfferPreviewView>;

            Assert.Equal(HttpStatusCode.OK, serviceResult.StatusCode);
            Assert.Equal(offerPreviews.Count, clientOfferPreviews.Count);
            for (int i = 0; i < clientOfferPreviews.Count; i++)
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
            OfferView clientOffer = new OfferView()
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
            OfferView resultClientOffer = serviceResult.Result as OfferView;

            Assert.Equal(HttpStatusCode.OK, serviceResult.StatusCode);
            Assert.Equal(clientOffer.IsDeleted, resultClientOffer.IsDeleted);
            Assert.Equal(clientOffer.IsActive, resultClientOffer.IsActive);
            Assert.Equal(clientOffer.OfferDescription, resultClientOffer.OfferDescription);
            Assert.Equal(clientOffer.CostPerAdult, resultClientOffer.CostPerAdult);
            Assert.Equal(clientOffer.CostPerChild, resultClientOffer.CostPerChild);
            Assert.Equal(clientOffer.MaxGuests, resultClientOffer.MaxGuests);
            Assert.Equal(clientOffer.OfferTitle, resultClientOffer.OfferTitle);
            Assert.Equal(offerPictures.Count, resultClientOffer.OfferPictures.Count);
            for (int i = 0; i < offerPictures.Count; i++)
            {
                Assert.Equal(offerPictures[i], resultClientOffer.OfferPictures[i]);
            }
        }
        #endregion

        #region GetHotelOfferReviews
        [Fact]
        public void GetHotelOfferReviews_GoodTest()
        {
            List<ReviewView> list = new List<ReviewView>();
            list.Add(new ReviewView
            {
                Content = "con",
                CreationDate = DateTime.Now,
                Rating = 7,
                ReviewerUsername = "Jezus",
                ReviewID = 1
            });
            _offerSearchDataAccessMock.Setup(x => x.GetOfferReviews(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            _offerSearchDataAccessMock.Setup(x => x.GetOfferReviews(1, 1)).Returns(list);
            var result = _offerSearchService.GetHotelOfferReviews(1, 1);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            List<ReviewView> reviewInfos = (List<ReviewView>)result.Result;
            Assert.True(reviewInfos.Count == 1);
            Assert.True(reviewInfos[0].ReviewID == 1);
        }
        [Fact]
        public void GetHotelOfferReviews_ErrorTest()
        {
            _offerSearchDataAccessMock.Setup(x => x.GetOfferReviews(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            var result = _offerSearchService.GetHotelOfferReviews(1, 2);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
        }
        #endregion
        #region GetHotelReviews
        [Fact]
        public void GetHotelReviews_GoodTest()
        {
            List<ReviewView> list = new List<ReviewView>();
            list.Add(new ReviewView
            {
                Content = "con",
                CreationDate = DateTime.Now,
                Rating = 7,
                ReviewerUsername = "Jezus",
                ReviewID = 1
            });
            _offerSearchDataAccessMock.Setup(x => x.GetHotelReviews(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            _offerSearchDataAccessMock.Setup(x => x.GetHotelReviews(1, 0, 1)).Returns(list);
            var result = _offerSearchService.GetHotelReviews(1, 1, new Paging { PageNumber = 1, PageSize = 1 });
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            List<ReviewView> reviewInfos = (List<ReviewView>)result.Result;
            Assert.True(reviewInfos.Count == 1);
            Assert.True(reviewInfos[0].ReviewID == 1);
        }
        [Fact]
        public void GetHotelReviews_ErrorTest()
        {
            _offerSearchDataAccessMock.Setup(x => x.GetHotelReviews(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception());
            var result = _offerSearchService.GetHotelReviews(1, 2, new Paging { PageNumber = 1, PageSize = 1 });
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
        }
        #endregion
    }
}
