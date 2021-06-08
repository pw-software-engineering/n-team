using Moq;
using Server.Database.DataAccess.Hotel;
using Server.Database.DatabaseTransaction;
using Server.RequestModels;
using Server.Services.Hotel;
using Server.Services.Result;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Server.Tests.Services.Hotel
{
    public class OfferRoomServiceTest
    {
        public OfferRoomServiceTest()
        {
            _dataAccessMock = new Mock<IOfferRoomDataAccess>();
            _transactionMock = new Mock<IDatabaseTransaction>();

            _offerRoomService = new OfferRoomService(_dataAccessMock.Object, _transactionMock.Object);
        }
        private OfferRoomService _offerRoomService;
        private Mock<IOfferRoomDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;

        [Fact]
        public void AddRoomToOffer_NoRoomWithGivenID_404()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns((int?)null);

            IServiceResult result = _offerRoomService.AddRoomToOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void AddRoomToOffer_NotOwnerOfRoomWithGivenID_401()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(-1);

            IServiceResult result = _offerRoomService.AddRoomToOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void AddRoomToOffer_NoOfferWithGivenID_404()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(roomID)).Returns((int?) null);

            IServiceResult result = _offerRoomService.AddRoomToOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void AddRoomToOffer_NotOwnerOfOfferWithGivenID_401()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(roomID)).Returns(-1);

            IServiceResult result = _offerRoomService.AddRoomToOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void AddRoomToOffer_RoomIsAlreadyAddedToOffer_400()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.IsRoomAlreadyAddedToOffer(roomID, offerID)).Returns(true);

            IServiceResult result = _offerRoomService.AddRoomToOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void AddRoomToOffer_200()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.DoesRoomHaveUnfinishedReservations(roomID, offerID)).Returns(false);

            IServiceResult result = _offerRoomService.AddRoomToOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public void GetOfferRooms_InvalidPagingArguments_400()
        {
            int offerID = 1;
            int hotelID = 1;
            string hotelRoomNumber = "hotel room number";
            Paging paging = new Paging()
            {
                PageNumber = 0,
                PageSize = -1
            };

            IServiceResult result = _offerRoomService.GetOfferRooms(offerID, hotelID, hotelRoomNumber, paging);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void GetOfferRooms_NoOfferWithGivenID_404()
        {
            int offerID = 1;
            int hotelID = 1;
            string hotelRoomNumber = "hotel room number";
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns((int?)null);

            IServiceResult result = _offerRoomService.GetOfferRooms(offerID, hotelID, hotelRoomNumber, paging);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void GetOfferRooms_NotOwnerOfOfferWithGivenID_401()
        {
            int offerID = 1;
            int hotelID = 1;
            string hotelRoomNumber = "hotel room number";
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(-1);

            IServiceResult result = _offerRoomService.GetOfferRooms(offerID, hotelID, hotelRoomNumber, paging);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void GetOfferRooms_NoRoomWithGivenID_404()
        {
            int offerID = 1;
            int hotelID = 1;
            string hotelRoomNumber = "hotel room number";
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(offerID)).Returns((int?) null);

            IServiceResult result = _offerRoomService.GetOfferRooms(offerID, hotelID, hotelRoomNumber, paging);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void GetOfferRooms_NotOwnerOfRoomWithGivenID_401()
        {
            int offerID = 1;
            int hotelID = 1;
            string hotelRoomNumber = "hotel room number";
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 1
            };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(hotelRoomNumber)).Returns(-1);

            IServiceResult result = _offerRoomService.GetOfferRooms(offerID, hotelID, hotelRoomNumber, paging);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void GetOfferRooms_HotelRoomNumberSpecified_200()
        {
            int offerID = 1;
            int hotelID = 1;
            int? ownerID = 1;
            string hotelRoomNumber = "hotel room number";
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 1
            };
            OfferRoomView hotelRoom = new OfferRoomView()
            {
                RoomID = 1,
                HotelRoomNumber = hotelRoomNumber,
                OfferID = new List<int>() { 1, 2, 3 }
            };
            List<OfferRoomView> hotelRooms = new List<OfferRoomView>() { hotelRoom };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(ownerID);
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(hotelRoomNumber)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.GetOfferRooms(offerID, paging, hotelRoomNumber)).Returns(hotelRooms);

            IServiceResult result = _offerRoomService.GetOfferRooms(offerID, hotelID, hotelRoomNumber, paging);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(hotelRooms, result.Result);
        }
        [Fact]
        public void GetOfferRooms_HotelRoomNumberNotSpecified_200()
        {
            int offerID = 1;
            int hotelID = 1;
            int? ownerID = 1;
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 1
            };
            OfferRoomView hotelRoom = new OfferRoomView()
            {
                RoomID = 1,
                HotelRoomNumber = "room number",
                OfferID = new List<int>() { 1, 2, 3 }
            };
            List<OfferRoomView> hotelRooms = new List<OfferRoomView>() { hotelRoom };
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(offerID)).Returns(ownerID);
            _dataAccessMock.Setup(da => da.GetOfferRooms(offerID, paging, null)).Returns(hotelRooms);

            IServiceResult result = _offerRoomService.GetOfferRooms(offerID, hotelID, null, paging);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(hotelRooms, result.Result);
        }
        [Fact]
        public void RemoveRoomFromOffer_NoRoomWithGivenID_404()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns((int?)null);

            IServiceResult result = _offerRoomService.RemoveRoomFromOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void RemoveRoomFromOffer_NotOwnerOfRoomWithGivenID_401()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(-1);

            IServiceResult result = _offerRoomService.RemoveRoomFromOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void RemoveRoomFromOffer_NoOfferWithGivenID_404()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(roomID)).Returns((int?)null);

            IServiceResult result = _offerRoomService.RemoveRoomFromOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void RemoveRoomFromOffer_NotOwnerOfOfferWithGivenID_401()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(roomID)).Returns(-1);

            IServiceResult result = _offerRoomService.RemoveRoomFromOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
        [Fact]
        public void RemoveRoomFromOffer_RoomHasUnfinishedReservtions_400()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.DoesRoomHaveUnfinishedReservations(roomID, offerID)).Returns(true);

            IServiceResult result = _offerRoomService.RemoveRoomFromOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void RemoveRoomFromOffer_200()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.IsRoomAlreadyAddedToOffer(roomID, offerID)).Returns(true);
            _dataAccessMock.Setup(da => da.DoesRoomHaveUnfinishedReservations(roomID, offerID)).Returns(false);

            IServiceResult result = _offerRoomService.RemoveRoomFromOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public void RemoveRoomFromOffer_RoomNotAddedToOffer_400()
        {
            int roomID = 1;
            int offerID = 1;
            int hotelID = 1;
            _dataAccessMock.Setup(da => da.FindRoomAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.FindOfferAndGetOwner(roomID)).Returns(hotelID);
            _dataAccessMock.Setup(da => da.IsRoomAlreadyAddedToOffer(roomID, offerID)).Returns(false);

            IServiceResult result = _offerRoomService.RemoveRoomFromOffer(roomID, offerID, hotelID);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
