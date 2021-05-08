using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DatabaseTransaction;
using Server.Database.Models;
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
        IMapper _mapper;

        public HotelAccountServiceTest()
        {

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            _dataAccessMock = new Mock<IHotelAccountDataAccess>();
            _transaction = new Mock<IDatabaseTransaction>();
            hotelAccountService = new HotelAccountService(_dataAccessMock.Object,_transaction.Object, _mapper);
        }
        #endregion

        #region GetInfo
        [Fact]
        public void GetInfo_GoodTest_Returns200AndInfo()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.GetInfo(It.IsAny<int>())).Throws(new Exception());
            var p = _mapper.Map<HotelInfoDb>(hotel);
            _dataAccessMock.Setup(x => x.GetInfo(hotelID)).Returns(p);
            _dataAccessMock.Setup(x => x.FindPictres(hotelID)).Returns(new List<string>());
            var h = hotelAccountService.GetInfo(hotelID);

            Assert.True(h.StatusCode==System.Net.HttpStatusCode.OK);
            Assert.True(((HotelGetInfo)h.Result).HotelPictures is List<string>);
            Assert.True(((HotelGetInfo)h.Result).HotelDesc == hotel.HotelDesc);
        }

        [Fact]
        public void GetInfo_BadId_Returns404()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.GetInfo(It.IsAny<int>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.GetInfo(hotelID)).Returns(_mapper.Map<HotelInfoDb>(hotel));
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

            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<HotelInfoDb>())).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(_mapper.Map<HotelInfoDb>(hotel)));
            var result = hotelAccountService.UpdateInfo(hotelID+1, hotel);

            Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);

        }
        [Fact]
        public void UpdateInfo_ParametrIsNull_Returns404()
        {
            int hotelID = 1;
            
            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };

            _dataAccessMock.Setup(x => x.UpdateInfo(null)).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(_mapper.Map<HotelInfoDb>(hotel)));
            
            var result = hotelAccountService.UpdateInfo(hotelID, null);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.NotFound);

        }
        [Fact]
        public void UpdateInfo_GoodTestPicturesNull_Returns200()
        {
            int hotelID = 1;

            var hotel = new HotelGetInfo() { City = "City", Country = "contry", HotelDesc = "desc", HotelName = "name", HotelPictures = null, HotelPreviewPicture = "zdjencie" };
            
            _dataAccessMock.Setup(x => x.UpdateInfo( null)).Throws(new Exception());
            _dataAccessMock.Setup(x => x.UpdateInfo(It.IsAny<HotelInfoDb>()));
            _dataAccessMock.Setup(x => x.DeletePicteres(It.IsAny<HotelInfoDb>())).Throws(new Exception());
            var result = hotelAccountService.UpdateInfo(hotelID, hotel);
            Assert.True(result.StatusCode == System.Net.HttpStatusCode.OK );

        }
        #endregion
    }
}

