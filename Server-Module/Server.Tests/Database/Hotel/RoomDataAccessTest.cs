using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Hotel;
using Server.Database.Models;
using Server.RequestModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database.Hotel
{
    public class RoomDataAccessTest : IDisposable
    {
        #region TestsSetup
        public RoomDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer(configurationBuilder.GetConnectionString("RoomDAHotelTest"))
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ServerDbContext(builder.Options, false);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            Seed();

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new HotelAutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            _dataAccess = new RoomDataAccess(_mapper, _context);
        }
        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients ON");
                _context.Clients.AddRange(
                    new ClientDb { ClientID = 1, Username = "TestUsername1", Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1", Password = "TestPassword1" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels ON");
                _context.Hotels.AddRange(
                    new HotelDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDescription = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" },
                    new HotelDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDescription = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2" },
                    new HotelDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDescription = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels OFF;");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRooms ON");
                _context.HotelRooms.AddRange(
                    new HotelRoomDb { RoomID = 1, HotelID = 2, HotelRoomNumber = "TestHotelRoomNumber1" },
                    new HotelRoomDb { RoomID = 2, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber2" },
                    new HotelRoomDb { RoomID = 3, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber3" },
                    new HotelRoomDb { RoomID = 4, HotelID = 1, HotelRoomNumber = "TestHotelRoomNumber4" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRooms OFF;");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Offers ON");
                _context.Offers.AddRange(
                    new OfferDb { OfferID = 1, HotelID = 2, OfferTitle = "TestOfferTitle1", OfferPreviewPicture = "TestOfferPreviewPicture1", IsActive = true, IsDeleted = false, CostPerChild = 10, CostPerAdult = 11, MaxGuests = 1, Description = "TestDescription1" },
                    new OfferDb { OfferID = 2, HotelID = 3, OfferTitle = "TestOfferTitle2", OfferPreviewPicture = "TestOfferPreviewPicture2", IsActive = true, IsDeleted = false, CostPerChild = 20, CostPerAdult = 22, MaxGuests = 2, Description = "TestDescription2" },
                    new OfferDb { OfferID = 3, HotelID = 3, OfferTitle = "TestOfferTitle3", OfferPreviewPicture = "TestOfferPreviewPicture3", IsActive = false, IsDeleted = true, CostPerChild = 30, CostPerAdult = 33, MaxGuests = 3, Description = "TestDescription3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Offers OFF;");

                _context.OfferHotelRooms.AddRange(
                    new OfferHotelRoomDb { OfferID = 1, RoomID = 1 },
                    new OfferHotelRoomDb { OfferID = 2, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 3 });
                _context.SaveChanges();

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations ON");
                _context.ClientReservations.AddRange(
                    new ClientReservationDb { ReservationID = 1, OfferID = 2, ClientID = 1, HotelID = 2, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 1, 2) },
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = 1, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = 1, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(2001, 3, 3), ToTime = new DateTime(2001, 3, 6) });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations OFF;");

                _context.SaveChanges();
                transaction.Commit();
            }
        }
        #endregion

        private ServerDbContext _context;
        private IMapper _mapper;
        private RoomDataAccess _dataAccess;

        [Fact]
        public void AddRoom_RoomIsAdded()
        {
            int hotelID = 1;
            string roomNumber = "TestHotelRoomNumber";

            int roomID = _dataAccess.AddRoom(hotelID, roomNumber);
            HotelRoomDb roomTest = _context.HotelRooms.Find(roomID);

            Assert.NotNull(roomTest);
            Assert.Equal(hotelID, roomTest.HotelID);
            Assert.Equal(roomNumber, roomTest.HotelRoomNumber);
        }
        [Fact]
        public void DeleteRoom_NotAddedToAnyOffer_RoomIsDeleted()
        {
            int roomID = 4;

            _dataAccess.DeleteRoom(roomID);

            Assert.Null(_context.HotelRooms.Find(roomID));
        }
        [Fact]
        public void GetOffersForRooms_SingleRoom_OffersAreAddedForRoom()
        {
            List<HotelRoomView> hotelRooms = new List<HotelRoomView>()
            {
                new HotelRoomView{ RoomID = 2}
            };

            foreach(HotelRoomView room in hotelRooms)
                room.OfferID = _dataAccess.GetOfferIDsForRoom(room.RoomID);
            List<int> offers = _context.OfferHotelRooms
                                .Where(ohr => ohr.RoomID == hotelRooms[0].RoomID)
                                .Select(ohr => ohr.OfferID)
                                .ToList();

            Assert.Equal(offers.Count, hotelRooms[0].OfferID.Count);
            for (int i = 0; i < offers.Count; i++)
                Assert.Equal(offers[i], hotelRooms[0].OfferID[i]);
        }
        [Fact]
        public void GetOffersForRooms_MultipleRooms_OffersAreAddedForEachRoom()
        {
            List<HotelRoomView> hotelRooms = new List<HotelRoomView>()
            {
                 new HotelRoomView{ RoomID = 1},
                 new HotelRoomView{ RoomID = 2},
                 new HotelRoomView{ RoomID = 3},
            };

            foreach(HotelRoomView room in hotelRooms)
                room.OfferID = _dataAccess.GetOfferIDsForRoom(room.RoomID);

            for (int i = 0; i < hotelRooms.Count; i++)
                Assert.Equal(_context.OfferHotelRooms.Where(ohr => ohr.RoomID == hotelRooms[i].RoomID).Count(), hotelRooms[i].OfferID.Count);
        }
        [Fact]
        public void GetRooms_ReturnsListOfHotelRoomObjects()
        {
            int hotelID = 3;
            Paging paging = new Paging();

            List<HotelRoomView> hotelRoomsTest = _dataAccess.GetRooms(hotelID, paging);
            List<HotelRoomDb> hotelRooms = _context.HotelRooms
                                               .Where(hr => hr.HotelID == hotelID)
                                               .OrderByDescending(hr => hr.RoomID)
                                               .Skip((paging.PageNumber-1)*paging.PageSize)
                                               .Take(paging.PageSize)
                                               .ToList();

            Assert.Equal(hotelRooms.Count, hotelRoomsTest.Count);
            for (int i = 0; i < hotelRooms.Count; i++)
            {
                Assert.Equal(hotelRooms[i].RoomID, hotelRoomsTest[i].RoomID);
                Assert.Equal(hotelRooms[i].HotelRoomNumber, hotelRoomsTest[i].HotelRoomNumber);
            }
        }
        [Fact]
        public void GetRoomsWithRoomNumber_ReturnsListOfHotelRoomObjects()
        {
            int hotelID = 3;
            string roomNumber = "TestHotelRoomNumber3";
            Paging paging = new Paging();

            List<HotelRoomView> hotelRoomsTest = _dataAccess.GetRooms(hotelID, paging, roomNumber);
            List<HotelRoomDb> hotelRooms = _context.HotelRooms
                                                    .Where(hr => hr.HotelID == hotelID && hr.HotelRoomNumber == roomNumber)
                                                    .OrderByDescending(hr => hr.RoomID)
                                                    .Skip((paging.PageNumber - 1) * paging.PageSize)
                                                    .Take(paging.PageSize)
                                                    .ToList();

            Assert.Equal(hotelRooms.Count, hotelRoomsTest.Count);
            for (int i = 0; i < hotelRooms.Count; i++)
            {
                Assert.Equal(hotelRooms[i].RoomID, hotelRoomsTest[i].RoomID);
                Assert.Equal(hotelRooms[i].HotelRoomNumber, hotelRoomsTest[i].HotelRoomNumber);
            }
        }
        [Fact]
        public void FindRoomAndGetOwner_NoRoom_ReturnsNull()
        {
            int roomID = -1;

            int? ownerID = _dataAccess.FindRoomAndGetOwner(roomID);

            Assert.Null(ownerID);
        }
        [Fact]
        public void DoesRoomHaveAnyPendingReservations_HaveReservations_ReturnsTrue()
        {
            int roomID = 2;

            bool hasReservations = _dataAccess.CheckAnyUnfinishedReservations(roomID);

            Assert.True(hasReservations);
        }
        [Fact]
        public void DoesRoomHaveAnyPendingReservations_DoesNotHaveReservations_ReturnsFalse()
        {
            int roomID = 1;

            bool hasReservations = _dataAccess.CheckAnyUnfinishedReservations(roomID);

            Assert.False(hasReservations);
        }
        [Fact]
        public void UnpinFromAnyOffers_AllRelatedOfferHotelRoomEntititiesAreDeleted()
        {
            int roomID = 2;

            _dataAccess.UnpinRoomFromAnyOffers(roomID);

            Assert.False(_context.OfferHotelRooms.Where(ohr => ohr.RoomID == roomID).Any());
        }
        [Fact]
        public void RemoveRoomFromPastReservations_InAllRelatedReservationsRoomIsSetToNull()
        {
            int roomID = 2;

            _dataAccess.RemoveRoomFromPastReservations(roomID);

            Assert.False(_context.ClientReservations.Where(cr => cr.RoomID == roomID && cr.ToTime < DateTime.Now).Any());
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
