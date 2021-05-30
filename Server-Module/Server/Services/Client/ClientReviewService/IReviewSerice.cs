using Server.Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Client.ClientReviewService
{
    interface IReviewSerice
    {
        public IServiceResult GetReview(int reservationID, int clientID);
        public IServiceResult PutReview(int reservationID, int clientID);
        public IServiceResult DeleteReview(int reservationID, int clientID);
    }
}
