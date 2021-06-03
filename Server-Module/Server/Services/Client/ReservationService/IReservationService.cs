using Server.RequestModels;
using Server.RequestModels.Client;
using Server.Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Client
{
    public interface IReservationService
    {
        #region /client/reservations
        public IServiceResult GetReservations(int userID, Paging paging);
        #endregion

        #region /client/reservations/{reservationID}
        public IServiceResult CancelReservation(int reservationID, int userID);
        #endregion

        #region /hotels/{hotelID}/offers/{offerID}/reservations
        public IServiceResult AddReservation(int hotelID, int offerID, int userID, ReservationInfo reservation);
        #endregion
    }
}
