using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
{
    public class RoomDataAccess : IRoomDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public RoomDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public bool DoesRoomAlreadyExist(int hotelID, string hotelRoomNumber)
        {
            return _dbContext.HotelRooms.Any(hr => hr.HotelRoomNumber == hotelRoomNumber && hr.HotelID == hotelID);
        }
        public int AddRoom(int hotelID, string hotelRoomNumber)
        {
            HotelRoomDb room = new HotelRoomDb
            {
                HotelID = hotelID,
                HotelRoomNumber = hotelRoomNumber
            };
            _dbContext.HotelRooms.Add(room);
            _dbContext.SaveChanges();
            return room.RoomID;
        }
        public List<HotelRoomView> GetRooms(int hotelID, Paging paging, string roomNumber = null)
        {
            if (roomNumber == null)
                return _mapper.Map<List<HotelRoomView>>(_dbContext.HotelRooms
                            .Where(hr => hr.HotelID == hotelID)
                            .Skip(paging.PageSize * (paging.PageNumber - 1))
                            .Take(paging.PageSize)
                            .ToList());

            return _mapper.Map<List<HotelRoomView>>(_dbContext.HotelRooms
                        .Where(hr => hr.HotelID == hotelID && hr.HotelRoomNumber == roomNumber)
                        .Skip(paging.PageSize * (paging.PageNumber - 1))
                        .Take(paging.PageSize)
                        .ToList());
        }
        public List<int> GetOfferIDsForRoom(int roomId)
        {
            return _dbContext.OfferHotelRooms.Where(ohr => ohr.RoomID == roomId).Select(ohr => ohr.OfferID).ToList();
        }
        public void DeleteRoom(int roomID)
        {
            HotelRoomDb room = _dbContext.HotelRooms.Find(roomID);
            _dbContext.HotelRooms.Remove(room);
            _dbContext.SaveChanges();
        }
        public int? FindRoomAndGetOwner(int roomID)
        {
            return _dbContext.HotelRooms.Find(roomID)?.HotelID;
        }

        public bool CheckAnyUnfinishedReservations(int roomID)
        {
            return _dbContext.ClientReservations.Any(cr => cr.RoomID == roomID && cr.ToTime > DateTime.Now);
        }

        public void UnpinRoomFromAnyOffers(int roomID)
        {
            List<OfferHotelRoomDb> offerHotelRooms = _dbContext.OfferHotelRooms
                                                        .Where(ohr => ohr.RoomID == roomID)
                                                        .ToList();
            _dbContext.OfferHotelRooms.RemoveRange(offerHotelRooms);
            _dbContext.SaveChanges();
        }

        public void RemoveRoomFromPastReservations(int roomID)
        {
            List<ClientReservationDb> roomPastReservations = _dbContext.ClientReservations
                                                                .Where(cr => cr.RoomID == roomID && cr.ToTime < DateTime.Now)
                                                                .ToList();
            foreach (ClientReservationDb reservation in roomPastReservations)
                reservation.RoomID = null;
            _dbContext.SaveChanges();
        }

        public void ChangeActivationMark(int roomID, bool activeState)
        {
            HotelRoomDb room = _dbContext.HotelRooms.Find(roomID);
            room.IsActive = activeState;
            _dbContext.SaveChanges();
        }
    }
}
