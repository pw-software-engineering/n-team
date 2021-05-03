using Moq;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using Server.Services.HotelAccountService;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Server.Tests.Services
{
    public class HotelAccountServiceTest
    {
        private HotelAccountService hotelAccountService;
        private Mock<IHotelAccountDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transaction;

        public HotelAccountServiceTest()
        {
            _dataAccessMock = new Mock<IHotelAccountDataAccess>();
            _transaction = new Mock<IDatabaseTransaction>();
            hotelAccountService = new HotelAccountService(_dataAccessMock.Object,_transaction.Object);
        }
        [Fact]
        public void GodGetingTest()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { city = "city", country = "contry", hotelDesc = "desc", hotelName = "name", hotelPictures = null, hotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.GetInfo(It.IsAny<int>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.GetInfo(hotelID)).Returns(hotel);
            
            var h = hotelAccountService.GetInfo(hotelID);

            Assert.True(h == hotel);
        }
        [Fact]
        public void BadIDGetingTest()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { city = "city", country = "contry", hotelDesc = "desc", hotelName = "name", hotelPictures = null, hotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.GetInfo(It.IsAny<int>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.GetInfo(hotelID)).Returns(hotel);
            
            Assert.Throws<Exception>(() => hotelAccountService.GetInfo(hotelID + 1));
            
        }
        
        [Fact]
        public void BadIDUpdateingTest()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { city = "city", country = "contry", hotelDesc = "desc", hotelName = "name", hotelPictures = null, hotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(),It.IsAny<HotelUpdateInfo>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(hotelID, hotel));

            Assert.Throws<Exception>(() => hotelAccountService.UpdateInfo(hotelID + 1,hotel));

        }
        [Fact]
        public void NullUpdateingTest()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { city = "city", country = "contry", hotelDesc = "desc", hotelName = "name", hotelPictures = null, hotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(), null)).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(hotelID, hotel));

            Assert.Throws<Exception>(() => hotelAccountService.UpdateInfo(hotelID, null));

        }
        [Fact]
        public void GoodUpdateTest()
        {
            int hotelID = 1;

            var hotel = new HotelGetInfo() { city = "city", country = "contry", hotelDesc = "desc", hotelName = "name", hotelPictures = null, hotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(), null)).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(), It.IsAny<HotelUpdateInfo>()));
            hotelAccountService.UpdateInfo(1, hotel);
        }

    }
}

