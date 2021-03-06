﻿using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<ReservationObjectView> GetReservations(int hotelID, int? roomID, bool? currentOnly, Paging paging)
        {
            List<ClientReservationDb> reservations = _dbContext.ClientReservations.Where(cr => cr.HotelID == hotelID &&
                                                                                               cr.ToTime > DateTime.Now).ToList();

            if (currentOnly.HasValue && currentOnly.Value)
                reservations = reservations.Where(r => r.FromTime <= DateTime.Now).ToList();
            if (roomID.HasValue)
                reservations = reservations.Where(r => r.RoomID == roomID).ToList();

            reservations = reservations.OrderByDescending(r => r.ReservationID)
                                       .Skip((paging.PageNumber - 1) * paging.PageSize)
                                       .Take(paging.PageSize)
                                       .ToList();

            List<ReservationObjectView> reservationViews = new List<ReservationObjectView>();
            foreach(ClientReservationDb reservation in reservations)
            {
                ReservationObjectView reservationView = new ReservationObjectView();
                reservationView.Reservation = _mapper.Map<ReservationView>(reservation);
                reservationView.Room = GetRoomDetails(reservation.RoomID.Value);
                reservationView.Client = GetClientDetails(reservation.ClientID);                
                reservationViews.Add(reservationView);
            }
            return reservationViews;
        }

        private ClientView GetClientDetails(int clientID)
        {
            return _mapper.Map<ClientView>(_dbContext.Clients.Find(clientID));
        }

        private RoomView GetRoomDetails(int roomID)
        {
            return _mapper.Map<RoomView>(_dbContext.HotelRooms.Find(roomID));
        }
    }
}
