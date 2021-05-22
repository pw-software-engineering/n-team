using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using Server.RequestModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Hotel
{
    public class OfferRoomDataAccess : IOfferRoomDataAccess
    {
        private ServerDbContext _dbContext;
        private IMapper _mapper;
        public OfferRoomDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public void AddRoomToOffer(int roomID, int offerID)
        {
            _dbContext.OfferHotelRooms.Add(new OfferHotelRoomDb()
            {
                OfferID = offerID,
                RoomID = roomID
            });
            _dbContext.SaveChanges();
        }

        public bool DoesRoomHaveUnfinishedReservations(int roomID, int offerID)
        {
            return _dbContext.ClientReservations.Any(cr => cr.RoomID == roomID &&
                                                           cr.OfferID == offerID &&
                                                           cr.ToTime > DateTime.Now);
        }

        public int? FindOfferAndGetOwner(int offerID)
        {
            return _dbContext.Offers.Find(offerID)?.HotelID;
        }

        public int? FindRoomAndGetOwner(string hotelRoomNumber)
        {
            return _dbContext.HotelRooms.FirstOrDefault(hr => hr.HotelRoomNumber == hotelRoomNumber)?.HotelID;
        }

        public int? FindRoomAndGetOwner(int roomID)
        {
            return _dbContext.HotelRooms.Find(roomID)?.HotelID;
        }

        public List<OfferRoomView> GetOfferRooms(int offerID, Paging paging, string hotelRoomNumber = null)
        {
            List<OfferRoomView> roomViews = new List<OfferRoomView>();
            List<HotelRoomDb> rooms = _dbContext.HotelRooms.Include(hr => hr.OfferHotelRooms)
                                                           .Where(hr => hr.OfferHotelRooms.Any(ohr => ohr.OfferID == offerID))
                                                           .Skip((paging.PageNumber-1)*paging.PageSize)
                                                           .Take(paging.PageSize)
                                                           .ToList();
            if (hotelRoomNumber != null)
            {
                HotelRoomDb room = rooms.First(room => room.HotelRoomNumber == hotelRoomNumber);
                rooms = new List<HotelRoomDb>() {room};
            }

            foreach(HotelRoomDb room in rooms)
            {
                OfferRoomView roomView = new OfferRoomView();
                roomView.RoomID = room.RoomID;
                roomView.HotelRoomNumber = room.HotelRoomNumber;
                roomView.OfferID = room.OfferHotelRooms.Select(ohr => ohr.OfferID).ToList();
                roomViews.Add(roomView);
            }
            return roomViews;
        }

        public bool IsRoomAlreadyAddedToOffer(int roomID, int offerID)
        {
            return _dbContext.OfferHotelRooms.Any(ohr => ohr.OfferID == offerID && ohr.RoomID == roomID);
        }

        public void UnpinRoomFromOffer(int roomID, int offerID)
        {
            OfferHotelRoomDb room = _dbContext.OfferHotelRooms.First(ohr => ohr.OfferID == offerID && ohr.RoomID == roomID);
            _dbContext.Remove(room);
            _dbContext.SaveChanges();
        }
    }
}
