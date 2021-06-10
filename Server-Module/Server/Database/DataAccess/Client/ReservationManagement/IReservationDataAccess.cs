using Server.Services.Client;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using Server.RequestModels;

namespace Server.Database.DataAccess.Client
{
    public interface IReservationDataAccess
    {
        List<int> GetOfferRoomIDs(int offerID);
        bool IsRoomAvailable(int roomID, DateTime from, DateTime to);
        int AddReservation(Reservation reservation);
        bool HasReservationBegun(int reservationID);
        void RemoveReservation(int ReservationID);
        List<ReservationData> GetReservations(int userID, Paging paging);

        int? FindReservationAndGetOwner(int reservationID);
        int? FindOfferAndGetOwner(int offerID);
    }
}
