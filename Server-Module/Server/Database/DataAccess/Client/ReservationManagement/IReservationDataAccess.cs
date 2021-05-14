using Server.Services.Client;
using System;
using System.Collections.Generic;
using Server.Database.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public interface IReservationDataAccess
    {
        List<int> GetOfferRoomIDs(int offerID);
        bool IsRoomAvailable(int roomID, DateTime from, DateTime to);
        int AddReservation(Reservation reservation);
        bool HasReservationBegun(int reservationID);
        void RemoveReservation(int ReservationID);
        List<ClientReservationDb> GetReservations(int userID);

        int? FindReservationAndGetOwner(int reservationID);
        int? FindOfferAndGetOwner(int offerID);
    }
}
