using Moq;
using Server.Database.DataAccess.Client.Review;
using Server.Database.DatabaseTransaction;
using Server.RequestModels.Client;
using Server.Services.Client.ClientReviewService;
using Server.Services.Result;
using Server.ViewModels.Client;
using System;
using System.Net;
using Xunit;

namespace Server.Tests.Services.Client
{
    public class ReviewServiceTest
    {
        public ReviewServiceTest()
        {
            _dataAccessMock = new Mock<IReviewDataAccess>();
            _transactionMock = new Mock<IDatabaseTransaction>();
            _reviewService = new ReviewService(_dataAccessMock.Object, _transactionMock.Object);
        }

        private ReviewService _reviewService;
        private Mock<IReviewDataAccess> _dataAccessMock;
        private Mock<IDatabaseTransaction> _transactionMock;

        [Fact]
        public void GetReview_InvalidReservationID_404()
        {
            int reservationID = -1;
            int clientID = 1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns((int?)null);

            IServiceResult result = _reviewService.GetReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void GetReview_ProvideClientIDIsNotOwnerID_403()
        {
            int reservationID = 1;
            int clientID = -1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(1);

            IServiceResult result = _reviewService.GetReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }
        [Fact]
        public void GetReview_ReservationHasNoReview_404()
        {
            int reservationID = 1;
            int clientID = 1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(clientID);
            _dataAccessMock.Setup(da => da.GetReview(reservationID)).Returns((ReviewView)null);

            IServiceResult result = _reviewService.GetReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void GetReview_200()
        {
            int reservationID = 1;
            int clientID = 1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(clientID);
            _dataAccessMock.Setup(da => da.GetReview(reservationID)).Returns(new ReviewView());

            IServiceResult result = _reviewService.GetReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public void DeleteReview_InvalidReservationID_404()
        {
            int reservationID = -1;
            int clientID = 1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns((int?)null);

            IServiceResult result = _reviewService.DeleteReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void DeleteReview_ProvideClientIDIsNotOwnerID_403()
        {
            int reservationID = 1;
            int clientID = -1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(1);

            IServiceResult result = _reviewService.DeleteReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }
        [Fact]
        public void DeleteReview_ReviewDoesNotExist_404()
        {
            int reservationID = 1;
            int clientID = 1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(clientID);
            _dataAccessMock.Setup(da => da.DoesReviewExist(reservationID)).Returns(false);

            IServiceResult result = _reviewService.DeleteReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void DeleteReview_200()
        {
            int reservationID = 1;
            int clientID = 1;
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(clientID);
            _dataAccessMock.Setup(da => da.DoesReviewExist(reservationID)).Returns(true);

            IServiceResult result = _reviewService.DeleteReview(reservationID, clientID);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public void PutReview_ReviewUpdateIsNull_ThrowsArgumentNullException()
        {
            int reservationID = 1;
            int clientID = 1;
            ReviewUpdate reviewUpdate = null;

            Action action = () => _reviewService.PutReview(reservationID, clientID, reviewUpdate);

            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void PutReview_InvalidReservationID_404()
        {
            int reservationID = -1;
            int clientID = 1;
            ReviewUpdate reviewUpdate = new ReviewUpdate(); 
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns((int?)null);

            IServiceResult result = _reviewService.PutReview(reservationID, clientID, reviewUpdate);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public void PutReview_ProvideClientIDIsNotOwnerID_403()
        {
            int reservationID = 1;
            int clientID = -1;
            ReviewUpdate reviewUpdate = new ReviewUpdate();
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(1);

            IServiceResult result = _reviewService.PutReview(reservationID, clientID, reviewUpdate);

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }
        [Fact]
        public void PutReview_ReviewChangeIsNotAllowed_400()
        {
            int reservationID = 1;
            int clientID = 1;
            ReviewUpdate reviewUpdate = new ReviewUpdate();
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(clientID);
            _dataAccessMock.Setup(da => da.IsReviewChangeAllowed(reservationID)).Returns(false);

            IServiceResult result = _reviewService.PutReview(reservationID, clientID, reviewUpdate);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public void PutReview_200()
        {
            int reservationID = 1;
            int clientID = 1;
            ReviewUpdate reviewUpdate = new ReviewUpdate();
            _dataAccessMock.Setup(da => da.FindReservationOwner(reservationID)).Returns(clientID);
            _dataAccessMock.Setup(da => da.IsReviewChangeAllowed(reservationID)).Returns(true);

            IServiceResult result = _reviewService.PutReview(reservationID, clientID, reviewUpdate);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
