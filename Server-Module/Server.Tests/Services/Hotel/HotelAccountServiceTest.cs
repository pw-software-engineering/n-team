using AutoMapper;
using Moq;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.Database.Models;
using Server.RequestModels.Hotel;
using Server.Services.Hotel;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Server.Tests.Services.Hotel
{
    public class HotelAccountServiceTest
    {
        #region SetUp
        private HotelAccountService hotelAccountService;
        private Mock<IHotelAccountDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transaction;
        private IMapper _mapper;

        public HotelAccountServiceTest()
        {

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new HotelAutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            _dataAccessMock = new Mock<IHotelAccountDataAccess>();
            _transaction = new Mock<IDatabaseTransaction>();
            hotelAccountService = new HotelAccountService(_dataAccessMock.Object,_transaction.Object);
        }
        #endregion

        #region GetInfo
        [Fact]
        public void GetHotelInfo_200()
        {
            int hotelID = 1;
            HotelInfoView hotelInfo = new HotelInfoView() 
            { 
                City = "City", 
                Country = "Country",
                HotelDesc = "Desc", 
                HotelName = "Name", 
                HotelPictures = null, 
                HotelPreviewPicture = "Picture" 
            };
            _dataAccessMock.Setup(x => x.GetHotelInfo(hotelID)).Returns(hotelInfo);
            _dataAccessMock.Setup(x => x.GetPictures(hotelID)).Returns(new List<string>());

            IServiceResult result = hotelAccountService.GetHotelInfo(hotelID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True((result.Result as HotelInfoView).HotelPictures is List<string>);
            Assert.Equal(hotelInfo.HotelDesc, (result.Result as HotelInfoView).HotelDesc);
        }
        #endregion

        #region UpdateInfo
        [Fact]
        public void UpdateInfo_NullHotelInfoUpdate_ThrowsArgumentNullException()
        {
            int hotelID = 1;

            Action action = () => hotelAccountService.UpdateHotelInfo(hotelID, null);

            Assert.Throws<ArgumentNullException>(action);

        }
        [Fact]
        public void UpdateInfo_NullHotelPictures_200()
        {
            int hotelID = 1;
            HotelInfoUpdate hotelInfo = new HotelInfoUpdate()
            {
                HotelDesc = "desc",
                HotelName = "name",
                HotelPictures = null,
                HotelPreviewPicture = "Picture"
            };

            IServiceResult result = hotelAccountService.UpdateHotelInfo(hotelID, hotelInfo);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _dataAccessMock.Verify(da => da.AddPictures(It.IsAny<List<string>>(), It.IsAny<int>()), Times.Never);
            _dataAccessMock.Verify(da => da.DeletePictures(It.IsAny<int>()), Times.Never);
        }
        #endregion
    }
}

