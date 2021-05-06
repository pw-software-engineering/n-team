using Moq;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using Server.Services.HotelAccountService;
using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Server.Tests.Services
{
    public class HotelAccountServiceTest
    {
        #region SetUp
        private HotelAccountService hotelAccountService;
        private Mock<IHotelAccountDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transaction;

        public HotelAccountServiceTest()
        {
            _dataAccessMock = new Mock<IHotelAccountDataAccess>();
            _transaction = new Mock<IDatabaseTransaction>();
            hotelAccountService = new HotelAccountService(_dataAccessMock.Object,_transaction.Object);
        }
        #endregion

        #region GetInfo
        [Fact]
        public void GetInfo_GoodTest_Returns200AndInfo()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };
            
            _dataAccessMock.Setup(x => x.GetInfo(It.IsAny<int>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.GetInfo(hotelID)).Returns(hotel);
            
            var h = hotelAccountService.GetInfo(hotelID);

            Assert.True((HotelGetInfo)h.Result == hotel&& h.StatusCode==System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public void GetInfo_BadId_Returns404()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.GetInfo(It.IsAny<int>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.GetInfo(hotelID)).Returns(hotel);
            var result = hotelAccountService.GetInfo(hotelID + 1);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound );
            
        }
        #endregion

        #region UpdateInfo
        [Fact]
        public void UpdateInfo_BadId_Returns404()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(),It.IsAny<HotelUpdateInfo>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(hotelID, hotel));
            var result = hotelAccountService.UpdateInfo(hotelID+1, hotel);

            Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);

        }
        [Fact]
        public void UpdateInfo_ParametrIsNull_Returns404()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(), null)).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(hotelID, hotel));

            var result = hotelAccountService.UpdateInfo(hotelID, null);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound && ((Error)result.Result).error == "Exception of type 'System.Exception' was thrown.");

        }
        [Fact]
        public void UpdateInfo_GoodTest_Returns200()
        {
            int hotelID = 1;

            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(), null)).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<int>(), It.IsAny<HotelUpdateInfo>()));
            var result = hotelAccountService.UpdateInfo(hotelID, hotel);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.OK );

        }
        #endregion
    }
}

