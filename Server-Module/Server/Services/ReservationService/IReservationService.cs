using Server.Services.Result;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.ReservationService
{
    public interface IReservationService
    {
        #region /reservations/{reservationID}
        IServiceResult CancelReservation(int reservationID, int userID);
        #endregion

        #region /hotels/{hotelID}/offers/{offerID}/reservations
        IServiceResult AddReservation(int hotelID, int offerID, int userID, ReservationView reservation);
        #endregion
    }
}
