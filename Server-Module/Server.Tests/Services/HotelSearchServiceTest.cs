using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database.DataAccess;
using Server.Models;
using Server.RequestModels;
using Server.Services.HotelSearchService;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Server.Tests.Services
{
    public class HotelSearchServiceTest
    {
        public HotelSearchServiceTest()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();
            _hotelSearchDataAccessMock = new Mock<IHotelSearchDataAccess>();

            _hotelSearchService = new HotelSearchService(_hotelSearchDataAccessMock.Object, _mapper);
        }
        private HotelSearchService _hotelSearchService;
        private Mock<IHotelSearchDataAccess> _hotelSearchDataAccessMock;
        private IMapper _mapper;

        #region GetHotels
        [Fact]
        public void GetHotels_InvalidPaging_400_NonNullErrorObject()
        {
            Paging paging = new Paging(-1, 10);

            IServiceResult serviceResult = _hotelSearchService.GetHotels(paging, new HotelFilter());

            Assert.Equal(HttpStatusCode.BadRequest, serviceResult.StatusCode);
            Assert.True(serviceResult.Result is Error);
        }

        [Fact]
        public void GetHotels_NullFilterObject_ThrowsArgumentNullxception()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                IServiceResult serviceResult = _hotelSearchService.GetHotels(new Paging(), null);
            });
        }

        [Fact]
        public void GetHotels_ValidFilterAndPaging_200()
        {
            Paging paging = new Paging();
            HotelFilter hotelFilter = new HotelFilter();
            HotelPreview hotelPreview = new HotelPreview()
            {
                HotelID = 1,
                City = "TestCity",
                Country = "TestCountry",
                HotelName = "TestHotelName",
                PreviewPicture = "TestPreviewPicture"
            };
            _hotelSearchDataAccessMock.Setup(da => da.GetHotels(paging, hotelFilter)).Returns(new List<HotelPreview>()
            {
                hotelPreview,
                hotelPreview,
                hotelPreview
            });

            IServiceResult serviceResult = _hotelSearchService.GetHotels(paging, hotelFilter);

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
            _hotelSearchDataAccessMock.Setup(da => da.GetHotelDetails(hotelID)).Returns((Hotel)null);

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
            Hotel hotel = new Hotel()
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
    }
}
