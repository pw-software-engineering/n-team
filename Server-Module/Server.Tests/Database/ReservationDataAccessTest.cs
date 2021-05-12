﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DataAccess.ReservationsManagement;
using Server.Database.Models;
using Server.Models;
using Server.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database
{
    public class ReservationDataAccessTest : IDisposable
    {
        #region TestsSetup
        public ReservationDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=ServerDbTestsReservation;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ServerDbContext(builder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            if (!_context.HotelInfos.Any())
                Seed();

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            _dataAccess = new ReservationDataAccess(_mapper, _context);
        }
        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelInfos ON");
                _context.HotelInfos.AddRange(
                    new HotelInfoDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDescription = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" },
                    new HotelInfoDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDescription = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2" },
                    new HotelInfoDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDescription = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelInfos OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Offers ON");
                _context.Offers.AddRange(
                    new OfferDb { OfferID = 1, HotelID = 2, OfferTitle = "TestOfferTitle1", OfferPreviewPicture = "TestOfferPreviewPicture1", IsActive = true, IsDeleted = false, CostPerChild = 10, CostPerAdult = 11, MaxGuests = 1, Description = "TestDescription1" },
                    new OfferDb { OfferID = 2, HotelID = 3, OfferTitle = "TestOfferTitle2", OfferPreviewPicture = "TestOfferPreviewPicture2", IsActive = true, IsDeleted = false, CostPerChild = 20, CostPerAdult = 22, MaxGuests = 2, Description = "TestDescription2" },
                    new OfferDb { OfferID = 3, HotelID = 3, OfferTitle = "TestOfferTitle3", OfferPreviewPicture = "TestOfferPreviewPicture3", IsActive = false, IsDeleted = true, CostPerChild = 30, CostPerAdult = 33, MaxGuests = 3, Description = "TestDescription3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Offers OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT OfferPictures ON");
                _context.OfferPictures.AddRange(
                    new OfferPictureDb { PictureID = 1, OfferID = 2, Picture = "TestPicture1" },
                    new OfferPictureDb { PictureID = 2, OfferID = 3, Picture = "TestPicture2" },
                    new OfferPictureDb { PictureID = 3, OfferID = 3, Picture = "TestPicture3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT OfferPictures OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRooms ON");
                _context.HotelRooms.AddRange(
                    new HotelRoomDb { RoomID = 1, HotelID = 2, HotelRoomNumber = "TestHotelRoomNumber1" },
                    new HotelRoomDb { RoomID = 2, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber2" },
                    new HotelRoomDb { RoomID = 3, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRooms OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients ON");
                _context.Clients.AddRange(
                    new ClientDb { ClientID = 1, Username = "TestUsername1", Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1", Password = "TestPassword1" },
                    new ClientDb { ClientID = 2, Username = "TestUsername2", Email = "TestEmail2", Name = "TestName2", Surname = "TestSurname2", Password = "TestPassword2" },
                    new ClientDb { ClientID = 3, Username = "TestUsername3", Email = "TestEmail3", Name = "TestName3", Surname = "TestSurname3", Password = "TestPassword3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations ON");
                _context.ClientReservations.AddRange(
                    new ClientReservationDb { ReservationID = 1, OfferID = 2, ClientID = 2, HotelID = 2, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 1, 2) },
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(3001, 3, 3), ToTime = new DateTime(3001, 3, 6) });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT OfferHotelRooms ON");
                _context.OfferHotelRooms.AddRange(
                    new OfferHotelRoomDb { OfferID = 1, RoomID = 1 },
                    new OfferHotelRoomDb { OfferID = 2, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 3 });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT OfferHotelRooms OFF");

                transaction.Commit();
            }
        }
        #endregion

        private ServerDbContext _context;
        private IMapper _mapper;
        private ReservationDataAccess _dataAccess;

        [Fact]
        public void FindOfferAndGetOwner_NoOffer_ReturnNull()
        {
            int offerID = -1;

            int? owner = _dataAccess.FindOfferAndGetOwner(offerID);

            Assert.Null(owner);
        }
        [Fact]
        public void FindOfferAndGetOwner_ReturnOwnerHotelID()
        {
            int offerID = 1;
            int ownerID = 2;

            int? ownerTest = _dataAccess.FindOfferAndGetOwner(offerID);

            Assert.Equal(ownerID, ownerTest);
        }
        [Fact]
        public void FindReservationAndGetOwner_ReturnOwnerClientID()
        {
            int reservationID = 1;
            int ownerID = 2;

            int? ownerTest = _dataAccess.FindReservationAndGetOwner(reservationID);

            Assert.Equal(ownerID, ownerTest);
        }
        [Fact]
        public void FindReservationAndGetOwner_NoReservation_ReturnNull()
        {
            int reservationID = -1;

            int? ownerTest = _dataAccess.FindReservationAndGetOwner(reservationID);

            Assert.Null(ownerTest);
        }
        [Fact]
        public void IsRoomAvailable_RoomAvailable_ReturnTrue()
        {
            int roomID = 2;
            DateTime from = new DateTime(4001, 2, 1);
            DateTime to = new DateTime(4001, 3, 1);

            bool isAvalaible = _dataAccess.IsRoomAvailable(roomID, from, to);

            Assert.True(isAvalaible);
        }
        [Fact]
        public void IsRoomAvailable_RoomNotAvailable_ReturnFalse()
        {
            int roomID = 2;
            DateTime from = new DateTime(2001, 1, 1);
            DateTime to = new DateTime(2001, 1, 2);

            bool isAvalaible = _dataAccess.IsRoomAvailable(roomID, from, to);

            Assert.False(isAvalaible);
        }
        [Fact]
        public void AddReservation_ReservationIsAdded()
        {
            Reservation reservation = new Reservation()
            {
                ClientID = null,
                FromTime = DateTime.Now,
                ToTime = DateTime.Now,
                HotelID = 2,
                OfferID = 1,
                RoomID = 1,
                NumberOfAdults = 2,
                NumberOfChildren = 2,
                ReviewID = null
            };
            int numberOfReservations = _context.ClientReservations.Count();

            _dataAccess.AddReservation(reservation);
            ClientReservationDb addedReservation = _context.ClientReservations.Find(numberOfReservations + 1);

            Assert.Equal(reservation.ClientID, addedReservation.ClientID);
            Assert.Equal(reservation.FromTime, addedReservation.FromTime);
            Assert.Equal(reservation.ToTime, addedReservation.ToTime);
            Assert.Equal(reservation.HotelID, addedReservation.HotelID);
            Assert.Equal(reservation.OfferID, addedReservation.OfferID);
            Assert.Equal(reservation.RoomID, addedReservation.RoomID);
            Assert.Equal(reservation.NumberOfAdults, addedReservation.NumberOfAdults);
            Assert.Equal(reservation.NumberOfChildren, addedReservation.NumberOfChildren);
            Assert.Equal(reservation.ReviewID, addedReservation.ReviewID);
        }
        [Fact]
        public void GetOfferRoomIDs_ReturnListOfOfferRoomIDs()
        {
            int offerID = 3;

            List<int> testedRoomIDs = _dataAccess.GetOfferRoomIDs(offerID);
            List<int> roomIDs = _context.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID)
                                                        .Select(ohr => ohr.RoomID)
                                                        .ToList();

            Assert.Equal(roomIDs.Count, testedRoomIDs.Count);
            for (int i = 0; i < roomIDs.Count; i++)
                Assert.Equal(roomIDs[i], testedRoomIDs[i]);
        }
        [Fact]
        public void HasReservationBegun_ReservationHasBegun_ReturnTrue()
        {
            int reservationID = 1;

            bool hasBegun = _dataAccess.HasReservationBegun(reservationID);

            Assert.True(hasBegun);
        }
        [Fact]
        public void HasReservationBegun_ReservationHasNotBegun_ReturnFalse()
        {
            int reservationID = 3;

            bool hasBegun = _dataAccess.HasReservationBegun(reservationID);

            Assert.False(hasBegun);
        }
        [Fact]
        public void RemoveReservation_ReservationIsRemoved()
        {
            int reservationID = 3;

            _dataAccess.RemoveReservation(reservationID);

            Assert.Null(_context.ClientReservations.Find(reservationID));
        }
        public void Dispose()
        {
            //_context.Database.EnsureDeleted();
        }
    }
}