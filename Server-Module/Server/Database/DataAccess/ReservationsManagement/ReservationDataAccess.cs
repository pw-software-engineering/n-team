using AutoMapper;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.ReservationsManagement
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
        public void AddReservation(Reservation reservation)
        {
            ClientReservationDb reservationDb = _mapper.Map<ClientReservationDb>(reservation);
            _dbContext.ClientReservations.Add(reservationDb);
            _dbContext.SaveChanges();
        }

        public bool IsRoomAvailable(int roomID, DateTime from, DateTime to)
        {

            return !_dbContext.ClientReservations.Any(cr => cr.RoomID == roomID &&
                                                            ((cr.FromTime >= from && cr.FromTime < to) ||
                                                            (cr.ToTime > from && cr.ToTime <= to)));                                                           
        }

        public int? FindOfferAndGetOwner(int offerID)
        {
            return _dbContext.Offers.Where(o => o.OfferID == offerID)
                                    .Select(o => (int?)o.HotelID)
                                    .FirstOrDefault();
        }

        public int? FindReservationAndGetOwner(int reservationID)
        {
            return _dbContext.ClientReservations.Where(cr => cr.ReservationID == reservationID)
                                                .Select(cr => (int?)cr.ClientID)
                                                .FirstOrDefault();
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
