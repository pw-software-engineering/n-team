using AutoMapper;
using Server.Database.Models;
using Server.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client
{
    public class ReservationDataAccess : IReservationDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public ReservationDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public int AddReservation(Reservation reservation)
        {
            ClientReservationDb reservationDb = _mapper.Map<ClientReservationDb>(reservation);
            _dbContext.ClientReservations.Add(reservationDb);
            _dbContext.SaveChanges();
            return reservationDb.ReservationID;
        }

        public bool IsRoomAvailable(int roomID, DateTime from, DateTime to)
        {

            return !_dbContext.ClientReservations.Any(cr => cr.RoomID == roomID &&
                                                            ((cr.FromTime >= from && cr.FromTime < to) ||
                                                            (cr.ToTime > from && cr.ToTime <= to)));                                                           
        }

        public int? FindOfferAndGetOwner(int offerID)
        {
            return _dbContext.Offers.Find(offerID)?.HotelID;
        }

        public int? FindReservationAndGetOwner(int reservationID)
        {
            return _dbContext.ClientReservations.Find(reservationID)?.ClientID;
        }

        public List<int> GetOfferRoomIDs(int offerID)
        {
            return _dbContext.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID)
                                             .Select(ohr => ohr.RoomID)
                                             .ToList();
        }

        public bool HasReservationBegun(int reservationID)
        {
            return _dbContext.ClientReservations.Find(reservationID).FromTime < DateTime.Now;
        }

        public void RemoveReservation(int reservationID)
        {
            ClientReservationDb reservationDb = _dbContext.ClientReservations.Find(reservationID);
            _dbContext.ClientReservations.Remove(reservationDb);
            _dbContext.SaveChanges();
        }
    }
}
