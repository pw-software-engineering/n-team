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

        public void DeleteRoom(int roomID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.HotelRooms
                    .Remove(_dbContext.HotelRooms.Find(roomID));
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public List<HotelRoom> GetRooms(Paging paging, int hotelID)
        {
            List<HotelRoom> hotelRooms = _mapper.Map<List<HotelRoom>>(_dbContext.HotelRooms
                                            .Where(hr => hr.HotelID == hotelID)
                                            .Skip(paging.pageSize * (paging.pageNumber - 1))
                                            .Take(paging.pageSize)
                                            .ToList());
            GetOffersForRooms(hotelRooms);
            return hotelRooms;
        }
        public List<HotelRoom> GetRoomsWithRoomNumber(Paging paging, int hotelID, string roomNumber)
        {
            List<HotelRoom> hotelRooms = _mapper.Map<List<HotelRoom>>(_dbContext.HotelRooms
                                            .Where(hr => hr.HotelID == hotelID && hr.HotelRoomNumber == roomNumber)
                                            .Skip(paging.pageSize * (paging.pageNumber - 1))
                                            .Take(paging.pageSize)
                                            .ToList());
            GetOffersForRooms(hotelRooms);
            return hotelRooms;
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
        public int? FindRoomAndGetOwner(int roomID)
        {
            List<int> owners = _dbContext.HotelRooms
                                .Where(hr => hr.RoomID == roomID)
                                .Select(hr => hr.HotelID).ToList();
            if (owners.Count == 0)
                return null;
            return owners[0];
        }
    }
}
