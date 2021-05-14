using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
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
        public int? FindRoomAndGetOwner(int roomID)
        {
            return _dbContext.HotelRooms.Find(roomID)?.HotelID;
        }

        public List<ReservationObjectView> GetReservations(int hotelID, int? roomID, bool currentOnly, Paging paging)
        {
            List<ClientReservationDb> reservations = _dbContext.ClientReservations.Where(cr => cr.HotelID == hotelID).ToList();

            if (currentOnly)
                reservations = reservations.Where(r => r.FromTime <= DateTime.Now &&
                                                       r.ToTime > DateTime.Now).ToList();
            if (roomID.HasValue)
                reservations = reservations.Where(r => r.RoomID == roomID).ToList();
            reservations = reservations.Skip((paging.PageNumber - 1) * paging.PageSize)
                                       .Take(paging.PageSize)
                                       .ToList();

            List<ReservationObjectView> reservationViews = new List<ReservationObjectView>();
            foreach(ClientReservationDb reservation in reservations)
            {
                ReservationObjectView reservationView = new ReservationObjectView();
                reservationView.Reservation = _mapper.Map<ReservationView>(reservation);
                reservationView.Room.RoomID = reservation.RoomID.Value;
                reservationView.Client.ClientID = reservationView.Client.ClientID;
                reservationViews.Add(reservationView);
            }
            return reservationViews;
        }

        public ClientView GetClientDetails(int clientID)
        {
            return _mapper.Map<ClientView>(_dbContext.Clients.Find(clientID));
        }

        public RoomView GetRoomDetails(int roomID)
        {
            return _mapper.Map<RoomView>(_dbContext.HotelRooms.Find(roomID));
        }
    }
}
