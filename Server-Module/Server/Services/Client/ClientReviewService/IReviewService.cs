using Server.RequestModels.Client;
using Server.Services.Result;

namespace Server.Services.Client.ClientReviewService
{
    public interface IReviewService
    {
        public IServiceResult GetReview(int reservationID, int clientID);
        public IServiceResult PutReview(int reservationID, int clientID, ReviewUpdate reviewUpdate);
        public IServiceResult DeleteReview(int reservationID, int clientID);
    }
}
