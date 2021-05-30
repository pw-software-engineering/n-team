using Server.Services.Result;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Client.ClientReviewService
{
    public interface IReviewSerice
    {
        public IServiceResult GetReview(int reservationID, int clientID);
        public IServiceResult PutReview(int reservationID, int clientID,ReviewUpdater reviewUpdater);
        public IServiceResult DeleteReview(int reservationID, int clientID);
    }
}
