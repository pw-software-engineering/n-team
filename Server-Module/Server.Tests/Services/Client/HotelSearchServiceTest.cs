﻿using Moq;
using Server.Database.DataAccess.Client;
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
    public class HotelSearchServiceTest
    {
        public HotelSearchServiceTest()
        {           
            _hotelSearchDataAccessMock = new Mock<IHotelSearchDataAccess>();

            _hotelSearchService = new HotelSearchService(_hotelSearchDataAccessMock.Object);
        }
        private HotelSearchService _hotelSearchService;
        private Mock<IHotelSearchDataAccess> _hotelSearchDataAccessMock;

        #region GetHotels
        [Fact]
        public void GetHotels_InvalidPaging_400_NonNullErrorObject()
        {
            Paging paging = new Paging(-1, 10);

            IServiceResult serviceResult = _hotelSearchService.GetHotels(new HotelFilter(), paging);

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is ErrorView);
        }

        [Fact]
        public void GetHotels_NullFilterObject_ThrowsArgumentNullxception()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                IServiceResult serviceResult = _hotelSearchService.GetHotels(null, new Paging());
            });
        }

        [Fact]
        public void GetHotels_ValidFilterAndPaging_200()
        {
            Paging paging = new Paging();
            HotelFilter hotelFilter = new HotelFilter();
            HotelPreviewView hotelPreview = new HotelPreviewView()
            {
                HotelID = 1,
                City = "TestCity",
                Country = "TestCountry",
                HotelName = "TestHotelName",
                PreviewPicture = "TestPreviewPicture"
            };
            _hotelSearchDataAccessMock.Setup(da => da.GetHotels(hotelFilter, paging)).Returns(new List<HotelPreviewView>()
            {
                hotelPreview,
                hotelPreview,
                hotelPreview
            });

            IServiceResult serviceResult = _hotelSearchService.GetHotels(hotelFilter, paging);

            Assert.Equal(HttpStatusCode.OK, serviceResult.StatusCode);
            Assert.Equal(3, (serviceResult.Result as List<HotelPreviewView>).Count);
            foreach(HotelPreviewView preview in serviceResult.Result as List<HotelPreviewView>)
            {
                Assert.Equal(hotelPreview.HotelID, preview.HotelID);
                Assert.Equal(hotelPreview.City, preview.City);
                Assert.Equal(hotelPreview.Country, preview.Country);
                Assert.Equal(hotelPreview.HotelName, preview.HotelName);
                Assert.Equal(hotelPreview.PreviewPicture, preview.PreviewPicture);
            }
        }
        #endregion

        #region GetHotelDetails
        [Fact]
        public void GetHotelDetails_NonExistentID_404()
        {
            int hotelID = -1;
            _hotelSearchDataAccessMock.Setup(da => da.GetHotelDetails(hotelID)).Returns(null as HotelView);

            IServiceResult serviceResult = _hotelSearchService.GetHotelDetails(hotelID);

            Assert.Equal(HttpStatusCode.NotFound, serviceResult.StatusCode);
        }

        [Fact]
        public void GetHotelDetails_ValidID_200()
        {
            int hotelID = 1;
            List<string> hotelPictures = new List<string>()
            {
                "TestPicture1",
                "TestPicture2"
            };
            HotelView hotel = new HotelView()
            {
                Country = "TestCountry",
                City = "TestCity",
                HotelDescription = "TestDescription",
                HotelName = "TestHotelName",
                HotelPictures = hotelPictures
            };
            _hotelSearchDataAccessMock.Setup(da => da.GetHotelDetails(hotelID)).Returns(hotel);
            _hotelSearchDataAccessMock.Setup(da => da.GetHotelPictures(hotelID)).Returns(hotelPictures);

            IServiceResult serviceResult = _hotelSearchService.GetHotelDetails(hotelID);
            HotelView hotelView = serviceResult.Result as HotelView;

            Assert.Equal(HttpStatusCode.OK, serviceResult.StatusCode);
            Assert.Equal(hotel.Country, hotelView.Country);
            Assert.Equal(hotel.City, hotelView.City);
            Assert.Equal(hotel.HotelDescription, hotelView.HotelDescription);
            Assert.Equal(hotel.HotelName, hotelView.HotelName);
            Assert.Equal(hotel.HotelPictures.Count, hotelView.HotelPictures.Count);
            for(int i = 0; i < hotelView.HotelPictures.Count; i++)
            {
                Assert.Equal(hotel.HotelPictures[i], hotelView.HotelPictures[i]);
            }
        }
        #endregion
        [Fact]
        public void GetHotelReviews_InvalidPagingArguments_400()
        {
            Paging paging = new Paging()
            {
                PageNumber = -1,
                PageSize = 2
            };
            int hotelID = 1;

            IServiceResult result = _hotelSearchService.GetHotelReviews(hotelID, paging);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void GetHotelReviews_HotelDoesNotExist_404()
        {
            Paging paging = new Paging();
            int hotelID = -1;
            _hotelSearchDataAccessMock.Setup(da => da.DoesHotelExist(hotelID)).Returns(false);

            IServiceResult result = _hotelSearchService.GetHotelReviews(hotelID, paging);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void GetHotelReviews_200()
        {
            Paging paging = new Paging();
            int hotelID = 1;
            _hotelSearchDataAccessMock.Setup(da => da.DoesHotelExist(hotelID)).Returns(true);

            IServiceResult result = _hotelSearchService.GetHotelReviews(hotelID, paging);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
