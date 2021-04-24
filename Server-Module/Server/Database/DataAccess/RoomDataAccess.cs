using AutoMapper;
using Server.Database.Models;
using Server.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess
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
            return _dbContext.HotelRooms.Where(hr => hr.HotelRoomNumber == hotelRoomNumber && hr.HotelID == hotelID).Any();
        }
        public int AddRoom(int hotelID, string hotelRoomNumber)
        {
            HotelRoomDb room = new HotelRoomDb { HotelID = hotelID, HotelRoomNumber = hotelRoomNumber };
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.HotelRooms.Add(room);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            return room.RoomID;
        }
        public List<HotelRoom> GetRooms(Paging paging, int hotelID)
        {
            return _mapper.Map<List<HotelRoom>>(_dbContext.HotelRooms
                    .Where(hr => hr.HotelID == hotelID)
                    .Skip(paging.pageSize * (paging.pageNumber - 1))
                    .Take(paging.pageSize)
                    .ToList());
        }
        public List<HotelRoom> GetRoomsWithRoomNumber(Paging paging, int hotelID, string roomNumber)
        {
            return _mapper.Map<List<HotelRoom>>(_dbContext.HotelRooms
                    .Where(hr => hr.HotelID == hotelID && hr.HotelRoomNumber == roomNumber)
                    .Skip(paging.pageSize * (paging.pageNumber - 1))
                    .Take(paging.pageSize)
                    .ToList());
        }
        public void GetOffersForRooms(List<HotelRoom> hotelRooms)
        {
            foreach (HotelRoom room in hotelRooms)
            {
                room.OfferID = _dbContext.OfferHotelRooms
                                    .Where(ohr => ohr.RoomID == room.RoomID)
                                    .Select(ohr => ohr.OfferID)
                                    .ToList();
            }
        }
        public void DeleteRoom(int roomID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                HotelRoomDb room = _dbContext.HotelRooms.Find(roomID);
                _dbContext.HotelRooms
                    .Remove(room);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }
        public int? FindRoomAndGetOwner(int roomID)
        {
            List<int> owners = _dbContext.HotelRooms
                                .Where(hr => hr.RoomID == roomID)
                                .Select(hr => hr.HotelID).ToList();
            if (owners.Count == 0)
                return null;
            return owners[0];
        }

        public bool DoesRoomHaveAnyUnfinishedReservations(int roomID)
        {
            return _dbContext.ClientReservations.Where(cr => cr.RoomID == roomID && cr.ToTime > DateTime.Now).Count() != 0;
        }

        public void UnpinRoomFromAnyOffers(int roomID)
        {
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                List<OfferHotelRoomDb> offerHotelRooms = _dbContext.OfferHotelRooms
                                                            .Where(ohr => ohr.RoomID == roomID)
                                                            .ToList();
                _dbContext.OfferHotelRooms.RemoveRange(offerHotelRooms);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public void RemoveRoomFromPastReservations(int roomID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                List<ClientReservationDb> roomPastReservations = _dbContext.ClientReservations
                                                                    .Where(cr => cr.RoomID == roomID && cr.ToTime < DateTime.Now)
                                                                    .ToList();
                foreach (ClientReservationDb reservation in roomPastReservations)
                    reservation.RoomID = null;
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }
    }
}
