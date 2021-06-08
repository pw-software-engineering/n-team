using Server.Database.DataAccess.Client.Review;
using Server.Database.DatabaseTransaction;
using Server.RequestModels.Client;
using Server.Services.Result;
using Server.ViewModels;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Services.Client.ClientReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewDataAccess _reviewDataAccess;
        private readonly IDatabaseTransaction _transaction;

        public ReviewService(IReviewDataAccess reviewDataAccess, IDatabaseTransaction transaction)
        {
            _reviewDataAccess = reviewDataAccess;
            _transaction = transaction;
        }

        public IServiceResult GetReview(int reservationID, int clientID)
        {
            IServiceResult result = CheckReservationExistenceAndOwnership(reservationID, clientID);
            if(!(result is null))
            {
                return result;
            }
            ReviewView reviewInfo = _reviewDataAccess.GetReview(reservationID);
            if(reviewInfo is null)
            {
                return new ServiceResult(
                    HttpStatusCode.NotFound, 
                    new ErrorView($"Reservation with ID equal to {reservationID} does not have a review"));
            }
            return new ServiceResult(HttpStatusCode.OK, reviewInfo);
        }

        public IServiceResult DeleteReview(int reservationID, int clientID)
        {
            using(IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                IServiceResult result = CheckReservationExistenceAndOwnership(reservationID, clientID);
                if (!(result is null))
                {
                    return result;
                }
                if (!_reviewDataAccess.CheckReviewExistence(reservationID))
                {
                    return new ServiceResult(
                        HttpStatusCode.NotFound,
                        new ErrorView($"Reservation with ID equal to {reservationID} does not have a review"));
                }
                _reviewDataAccess.DeleteReview(reservationID);
                transaction.CommitTransaction();
                return new ServiceResult(HttpStatusCode.OK);
            }
        }

        public IServiceResult PutReview(int reservationID, int clientID, ReviewUpdate reviewUpdate)
        {
            IServiceResult result = CheckReservationExistenceAndOwnership(reservationID, clientID);
            if (!(result is null))
            {
                return result;
            }
            if(!_reviewDataAccess.IsReviewChangeAllowed(reservationID))
            {
                return new ServiceResult(
                    HttpStatusCode.BadRequest,
                    new ErrorView($"Reviews can be changed/created during 30 days after the reservation has ended"));
            }
            int reviewID;
            using(IDatabaseTransaction transaction = _transaction.BeginTransaction())
            {
                if(!_reviewDataAccess.CheckReviewExistence(reservationID))
                {
                    reviewID = _reviewDataAccess.AddReview(reservationID, reviewUpdate);
                }
                else
                {
                    reviewID = _reviewDataAccess.EditReview(reservationID, reviewUpdate);
                }
                _transaction.CommitTransaction();
                return new ServiceResult(HttpStatusCode.OK, new ReviewIDView() { ReviewID = reviewID });
            }
        }

        public IServiceResult CheckReservationExistenceAndOwnership(int reservationID, int clientID)
        {
            int? ownerID = _reviewDataAccess.FindReservationOwner(reservationID);
            if (!ownerID.HasValue)
            {
                return new ServiceResult(
                    HttpStatusCode.NotFound,
                    new ErrorView($"Reservation with ID equal to {reservationID} does not exist"));
            }
            if (clientID != ownerID.Value)
            {
                return new ServiceResult(
                    HttpStatusCode.Forbidden,
                    new ErrorView($"Reservation with ID equal to {reservationID} does not belong to client with ID equal to {clientID}"));
            }
            return null;
        }
    }
}
