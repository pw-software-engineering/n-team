using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client.Review
{
    public interface IReviewDataAccess
    {
        public ReviewInfo GetReview(int reservationID);
        public int AddNewReview(int reservationID, ReviewUpdater reviewUpdater);
        public bool IsReviewExist(int reservationID);
        public int UpdateReview(int reservationID, ReviewUpdater reviewUpdater);
        public void DeleteReview(int reservationID);
        public bool IsClientTheOwnerOfReservation(int reservationID, int clientID);
        public bool IsAddingReviewToReservationEnabled(int reservationID);
        public bool IsDataValid(ReviewUpdater reviewUpdater);
    }
}
