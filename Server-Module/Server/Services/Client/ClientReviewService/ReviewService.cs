using Server.Database.DataAccess.Client.Review;
using Server.Database.DatabaseTransaction;
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
    public class ReviewService : IReviewSerice
    {
        private readonly IReviewDataAccess _reviewDataAccess;
        private readonly IDatabaseTransaction _transaction;

        public ReviewService(IReviewDataAccess reviewDataAccess,IDatabaseTransaction transaction)
        {
            _reviewDataAccess = reviewDataAccess;
            _transaction = transaction;
        }
        
        public IServiceResult DeleteReview(int reservationID,int clientID)
        {
            _transaction.BeginTransaction();
            try
            {
                if(!_reviewDataAccess.IsClientTheOwnerOfReservation(reservationID,clientID))
                {
                    _transaction.RollbackTransaction();
                    return new ServiceResult(HttpStatusCode.Forbidden, new ErrorView("Not the owner"));
                }

                _reviewDataAccess.DeleteReview(reservationID);

            }catch(Exception e)
            {
                _transaction.RollbackTransaction();
                return new ServiceResult(HttpStatusCode.NotFound,new ErrorView(e.Message));
            }

            _transaction.CommitTransaction();
            return new ServiceResult(HttpStatusCode.OK);
        }

        public IServiceResult GetReview(int reservationID)
        {
            ReviewInfo result;
            _transaction.BeginTransaction();
            try
            {

                result = _reviewDataAccess.GetReview(reservationID);

            }
            catch (Exception e)
            {
                _transaction.RollbackTransaction();
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView(e.Message));
            }

            _transaction.CommitTransaction();
            return new ServiceResult(HttpStatusCode.OK,result);
        }

        public IServiceResult PutReview(int reservationID, int clientID,ReviewUpdater reviewUpdater)
        {
            ReviewID reviewID = new ReviewID();
            _transaction.BeginTransaction();
            try
            {
                if (!_reviewDataAccess.IsClientTheOwnerOfReservation(reservationID, clientID))
                {
                    _transaction.RollbackTransaction();
                    return new ServiceResult(HttpStatusCode.Forbidden, new ErrorView("Not the owner"));
                }
                if(_reviewDataAccess.IsReviewExist(reservationID))
                {
                    reviewID.reviewID = _reviewDataAccess.UpdateReview(reservationID, reviewUpdater);
                }
                else
                {
                    reviewID.reviewID = _reviewDataAccess.AddNewReview(reservationID, reviewUpdater);
                }
                

            }
            catch (Exception e)
            {
                _transaction.RollbackTransaction();
                return new ServiceResult(HttpStatusCode.NotFound, new ErrorView(e.Message));
            }

            _transaction.CommitTransaction();
            return new ServiceResult(HttpStatusCode.OK, reviewID);
        }
    }
}
