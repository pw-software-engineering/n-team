//using Moq;
//using Server.Database.DataAccess.Client.Review;
//using Server.Database.DatabaseTransaction;
//using Server.Services.Client.ClientReviewService;
//using Server.ViewModels;
//using Server.ViewModels.Client;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using Xunit;

//namespace Server.Tests.Services.Client
//{
//    public class ReviewServiceTest
//    {
//        public ReviewServiceTest()
//        {
//            _dataAccessMock = new Mock<IReviewDataAccess>();
//            _transactionMock = new Mock<IDatabaseTransaction>();
//            _reviewService = new ReviewService(_dataAccessMock.Object, _transactionMock.Object);
//        }

//        private ReviewService _reviewService;
//        private Mock<IReviewDataAccess> _dataAccessMock;
//        private Mock<IDatabaseTransaction> _transactionMock;


//        #region GetReview
//        [Fact]
//        public void GetReview_GoodTest()
//        {
//            var output = new ReviewInfo { content = "c", rating = 1, revewerUsername = "n", reviewID = 1, creationDate = DateTime.UtcNow };
//            _dataAccessMock.Setup(da => da.GetReview(1)).Returns(output);
//            var result = _reviewService.GetReview(1);
//            Assert.True(output.reviewID == ((ReviewInfo)(result.Result)).reviewID);
//        }
//        [Fact]
//        public void Getreview_BadReviewID()
//        {
//            var output = new ReviewInfo { content = "c", rating = 1, revewerUsername = "n", reviewID = 2, creationDate = DateTime.UtcNow };
//            _dataAccessMock.Setup(da => da.GetReview(It.IsAny<int>())).Returns(output);
//            _dataAccessMock.Setup(da => da.GetReview(1)).Throws(new Exception("no content"));
//            var result = _reviewService.GetReview(1);
//            Assert.True("no content" == ((ErrorView)(result.Result)).Error);
//        }
//        #endregion
//        #region DeleteReview
//        [Fact]
//        public void DeleteReview_NotAOwner()
//        {
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(1, It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(1, 1)).Returns(false);
//            var result =_reviewService.DeleteReview(1, 1);

//            Assert.True(HttpStatusCode.Forbidden == (result.StatusCode));
//        }

//        [Fact]
//        public void DeleteReview_BadID()
//        {
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(1, It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.DeleteReview(It.IsAny<int>())).Throws(new Exception("niet"));
//            var result = _reviewService.DeleteReview(1, 1); 

//            Assert.True(HttpStatusCode.NotFound == (result.StatusCode));
//        }
//        [Fact]
//        public void DeleteReview_GoodTest()
//        {
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(1, 1)).Returns(true);
//            var result = _reviewService.DeleteReview(1, 1);

//            Assert.True(HttpStatusCode.OK == (result.StatusCode));

//        }
//        #endregion
//        #region PutReview
//        [Fact]
//        public void PutReview_NotAOwner()
//        {
//            var updater = new ReviewUpdater { content = "new content", rating = 1 };
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(It.IsAny<int>(), It.IsAny<int>())).Returns(false);            
//            var result = _reviewService.PutReview(1, 1, updater);
//            Assert.True(HttpStatusCode.Forbidden == (result.StatusCode));
//        }
//        [Fact]
//        public void PutReview_GoodTestAdd()
//        {
//            var updater = new ReviewUpdater { content = "new content", rating = 1 };
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.AddNewReview(1, updater)).Returns(1);
//            _dataAccessMock.Setup(da => da.UpdateReview(1, updater)).Returns(2);
//            _dataAccessMock.Setup(da => da.IsAddingReviewToReservationEnabled(It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsDataValid(It.IsAny<ReviewUpdater>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsReviewExist(It.IsAny<int>())).Returns(false);
//            var result = _reviewService.PutReview(1, 1, updater);
//            Assert.True(HttpStatusCode.OK == (result.StatusCode) && 1 == ((ReviewID)(result.Result)).reviewID);
//        }
//        [Fact]
//        public void PutReview_GoodTestUpdate()
//        {
//            var updater = new ReviewUpdater { content = "new content", rating = 1 };
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.AddNewReview(1, updater)).Returns(1);
//            _dataAccessMock.Setup(da => da.UpdateReview(1, updater)).Returns(2);
//            _dataAccessMock.Setup(da => da.IsAddingReviewToReservationEnabled(It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsDataValid(It.IsAny<ReviewUpdater>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsReviewExist(It.IsAny<int>())).Returns(true);
//            var result = _reviewService.PutReview(1, 1, updater);
//            Assert.True(HttpStatusCode.OK == (result.StatusCode) && 2 == ((ReviewID)(result.Result)).reviewID);
//        }
//        [Fact]
//        public void PutReview_InvalidData()
//        {
//            var updater = new ReviewUpdater { content = "new content", rating = 0 };
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.AddNewReview(1, updater)).Returns(1);
//            _dataAccessMock.Setup(da => da.UpdateReview(1, updater)).Returns(2);
//            _dataAccessMock.Setup(da => da.IsAddingReviewToReservationEnabled(It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsDataValid(It.IsAny<ReviewUpdater>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsDataValid(updater)).Returns(false);
//            _dataAccessMock.Setup(da => da.IsReviewExist(It.IsAny<int>())).Returns(true);
//            var result = _reviewService.PutReview(1, 1, updater);
//            Assert.True(HttpStatusCode.BadRequest == (result.StatusCode));

//        }
//        [Fact]
//        public void PutReview_InvalidTime()
//        {
//            var updater = new ReviewUpdater { content = "new content", rating = 0 };
//            _dataAccessMock.Setup(da => da.IsClientTheOwnerOfReservation(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.AddNewReview(1, updater)).Returns(1);
//            _dataAccessMock.Setup(da => da.UpdateReview(1, updater)).Returns(2);
//            _dataAccessMock.Setup(da => da.IsAddingReviewToReservationEnabled(It.IsAny<int>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsDataValid(It.IsAny<ReviewUpdater>())).Returns(true);
//            _dataAccessMock.Setup(da => da.IsAddingReviewToReservationEnabled(1)).Returns(false);
//            _dataAccessMock.Setup(da => da.IsReviewExist(It.IsAny<int>())).Returns(true);
//            var result = _reviewService.PutReview(1, 1, updater);
//            Assert.True(HttpStatusCode.Forbidden == (result.StatusCode));

//        }
//        #endregion
//    }
//}
