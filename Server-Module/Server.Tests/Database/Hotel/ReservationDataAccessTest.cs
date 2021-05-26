using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
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
    public class ReservationDataAccessTest : IDisposable
    {
        #region TestsSetup
        public ReservationDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer(configurationBuilder.GetConnectionString("ReservationDAHotelTest"))
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

            _dataAccess = new ReservationDataAccess(_mapper, _context);
        }
        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels ON");
                _context.Hotels.AddRange(
                    new HotelDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDescription = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" },
                    new HotelDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDescription = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2" },
                    new HotelDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDescription = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels OFF;");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients ON");
                _context.Clients.AddRange(
                    new ClientDb { ClientID = 1, Username = "TestUsername1", Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1", Password = "TestPassword1" },
                    new ClientDb { ClientID = 2, Username = "TestUsername2", Email = "TestEmail2", Name = "TestName2", Surname = "TestSurname2", Password = "TestPassword2" },
                    new ClientDb { ClientID = 3, Username = "TestUsername3", Email = "TestEmail3", Name = "TestName3", Surname = "TestSurname3", Password = "TestPassword3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRooms ON");
                _context.HotelRooms.AddRange(
                    new HotelRoomDb { RoomID = 1, HotelID = 2, HotelRoomNumber = "TestHotelRoomNumber1" },
                    new HotelRoomDb { RoomID = 2, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber2" },
                    new HotelRoomDb { RoomID = 3, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRooms OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Offers ON");
                _context.Offers.AddRange(
                    new OfferDb { OfferID = 1, HotelID = 2, OfferTitle = "TestOfferTitle1", OfferPreviewPicture = "TestOfferPreviewPicture1", IsActive = true, IsDeleted = false, CostPerChild = 10, CostPerAdult = 11, MaxGuests = 1, Description = "TestDescription1" },
                    new OfferDb { OfferID = 2, HotelID = 3, OfferTitle = "TestOfferTitle2", OfferPreviewPicture = "TestOfferPreviewPicture2", IsActive = true, IsDeleted = false, CostPerChild = 20, CostPerAdult = 22, MaxGuests = 2, Description = "TestDescription2" },
                    new OfferDb { OfferID = 3, HotelID = 3, OfferTitle = "TestOfferTitle3", OfferPreviewPicture = "TestOfferPreviewPicture3", IsActive = false, IsDeleted = true, CostPerChild = 30, CostPerAdult = 33, MaxGuests = 3, Description = "TestDescription3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Offers OFF");

                _context.OfferHotelRooms.AddRange(
                   new OfferHotelRoomDb { OfferID = 1, RoomID = 1 },
                   new OfferHotelRoomDb { OfferID = 2, RoomID = 2 },
                   new OfferHotelRoomDb { OfferID = 3, RoomID = 2 },
                   new OfferHotelRoomDb { OfferID = 3, RoomID = 3 });
                _context.SaveChanges();

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations ON");
                _context.ClientReservations.AddRange(
                    new ClientReservationDb { ReservationID = 1, OfferID = 3, ClientID = 2, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(3001, 1, 2) },
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = 1, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(3001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = 2, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                    new ClientReservationDb { ReservationID = 4, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(3001, 3, 3), ToTime = new DateTime(3001, 3, 6) });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations OFF");

                transaction.Commit();
            }
        }
        #endregion

        private ServerDbContext _context;
        private IMapper _mapper;
        private ReservationDataAccess _dataAccess;

        private void AssertReservationsListsEqual(List<ClientReservationDb> reservations, List<ReservationObjectView> testReservationObjects)
        {
            Assert.Equal(reservations.Count, testReservationObjects.Count);
            for (int i = 0; i < reservations.Count; i++)
            {
                Assert.Equal(reservations[i].ReservationID, testReservationObjects[i].Reservation.ReservationID);
                Assert.Equal(reservations[i].NumberOfAdults, testReservationObjects[i].Reservation.AdultsCount);
                Assert.Equal(reservations[i].NumberOfChildren, testReservationObjects[i].Reservation.ChildrenCount);
                Assert.Equal(reservations[i].OfferID, testReservationObjects[i].Reservation.OfferID);
                Assert.Equal(reservations[i].FromTime, testReservationObjects[i].Reservation.FromTime);
                Assert.Equal(reservations[i].ToTime, testReservationObjects[i].Reservation.ToTime);
            }
        }
        private void AssertClientsDetailsListEqual(List<ReservationObjectView> reservations)
        {
            foreach(ReservationObjectView reservation in reservations)
            {
                ClientView testedClient = reservation.Client;
                ClientDb client = _context.Clients.Find(testedClient.ClientID);

                Assert.NotNull(client);
                Assert.Equal(client.Name, testedClient.Name);
                Assert.Equal(client.Surname, testedClient.Surname);
            }
        }
        private void AssertRoomDetailsListEqual(List<ReservationObjectView> reservations)
        {
            foreach (ReservationObjectView reservation in reservations)
            {
                RoomView testedRoom = reservation.Room;
                HotelRoomDb room = _context.HotelRooms.Find(testedRoom.RoomID);

                Assert.NotNull(room);
                Assert.Equal(room.HotelRoomNumber, testedRoom.HotelRoomNumber);
            }
        }
        [Fact]
        public void FindRoomAndGetOwner_NoRoom_ReturnsNull()
        {
            int roomID = -1;

            int? owner = _dataAccess.FindRoomAndGetOwner(roomID);

            Assert.Null(owner);
        }
        [Fact]
        public void FindRoomAndGetOwner_ReturnsHotelOwnerID()
        {
            int roomID = 1;
            int ownerID = _context.HotelRooms.Find(roomID).HotelID;

            int? testedOwnerID = _dataAccess.FindRoomAndGetOwner(roomID);

            Assert.Equal(ownerID, testedOwnerID);
        }
        [Fact]
        public void GetReservations_AllReservationsAndRoomDefined_ReturnsListOfAllReservationsForRoom()
        {
            int hotelID = 3;
            int? roomID = 3;
            bool? currentOnly = false;
            Paging paging = new Paging() { PageSize = 10, PageNumber = 1 };

            List<ReservationObjectView> testReservationObjects = _dataAccess.GetReservations(hotelID, roomID, currentOnly, paging);
            List<ClientReservationDb> reservations = _context.ClientReservations.Where(cr => cr.HotelID == hotelID && 
                                                                                             cr.RoomID == roomID &&
                                                                                             cr.ToTime > DateTime.Now)
                                                                                .Skip((paging.PageNumber - 1) * paging.PageSize)
                                                                                .Take(paging.PageSize)
                                                                                .ToList();

            AssertReservationsListsEqual(reservations, testReservationObjects);
            AssertClientsDetailsListEqual(testReservationObjects);
            AssertRoomDetailsListEqual(testReservationObjects);
        }
        [Fact]
        public void GetReservations_CurrentOnlyReservationsAndRoomDefined_ReturnsListOfCurrentReservationsForRoom()
        {
            int hotelID = 3;
            int? roomID = 3;
            bool? currentOnly = true;
            Paging paging = new Paging() { PageSize = 10, PageNumber = 1 };

            List<ReservationObjectView> testReservationObjects = _dataAccess.GetReservations(hotelID, roomID, currentOnly, paging);
            List<ClientReservationDb> reservations = _context.ClientReservations.Where(cr => cr.HotelID == hotelID && cr.RoomID == roomID &&
                                                                                             cr.FromTime <= DateTime.Now && cr.ToTime > DateTime.Now)
                                                                                .Skip((paging.PageNumber - 1) * paging.PageSize)
                                                                                .Take(paging.PageSize)
                                                                                .ToList();

            AssertReservationsListsEqual(reservations, testReservationObjects);
            AssertClientsDetailsListEqual(testReservationObjects);
            AssertRoomDetailsListEqual(testReservationObjects);
        }
        [Fact]
        public void GetReservations_CurrentOnlyReservationsAndRoomNotDefined_ReturnsListOfCurrentReservations()
        {
            int hotelID = 3;
            bool? currentOnly = true;
            Paging paging = new Paging() { PageSize = 10, PageNumber = 1 };

            List<ReservationObjectView> testReservationObjects = _dataAccess.GetReservations(hotelID, null, currentOnly, paging);
            List<ClientReservationDb> reservations = _context.ClientReservations.Where(cr => cr.HotelID == hotelID &&
                                                                                             cr.FromTime <= DateTime.Now && 
                                                                                             cr.ToTime > DateTime.Now)
                                                                                .Skip((paging.PageNumber - 1) * paging.PageSize)
                                                                                .Take(paging.PageSize)
                                                                                .ToList();

            AssertReservationsListsEqual(reservations, testReservationObjects);
            AssertClientsDetailsListEqual(testReservationObjects);
            AssertRoomDetailsListEqual(testReservationObjects);
        }
        [Fact]
        public void GetReservations_AllReservationsAndRoomNotDefined_ReturnsListOfAllReservations()
        {
            int hotelID = 3;
            Paging paging = new Paging() { PageSize = 10, PageNumber = 1 };

            List<ReservationObjectView> testReservationObjects = _dataAccess.GetReservations(hotelID, null, null, paging);
            List<ClientReservationDb> reservations = _context.ClientReservations.Where(cr => cr.HotelID == hotelID && cr.ToTime > DateTime.Now)
                                                                                .Skip((paging.PageNumber - 1) * paging.PageSize)
                                                                                .Take(paging.PageSize)
                                                                                .ToList();

            AssertReservationsListsEqual(reservations, testReservationObjects);
            AssertClientsDetailsListEqual(testReservationObjects);
            AssertRoomDetailsListEqual(testReservationObjects);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
