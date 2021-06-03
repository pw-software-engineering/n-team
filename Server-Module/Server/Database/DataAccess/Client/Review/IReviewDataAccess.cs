using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client.Review
{
    public interface IReviewDataAccess
    {
        public ReviewView GetReview(int reservationID);
        public int? FindReservationOwner(int reservationID);
        public void DeleteReview(int reservationID);
        public bool CheckReviewExistence(int reservationID);
        public int AddReview(int reservationID, ReviewUpdate reviewUpdate);
        public int EditReview(int reservationID, ReviewUpdate reviewUpdate);
        public bool IsReviewChangeAllowed(int reservationID);
    }
}
