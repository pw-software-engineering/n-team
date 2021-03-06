﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess.Client;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Server.Tests.Database.Client
{
    public class OfferSearchDataAccessTest : IDisposable
    {
        #region TestsSetup
        public OfferSearchDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer(configurationBuilder.GetConnectionString("OfferSearchDAClientTest"))
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ServerDbContext(builder.Options, false);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            Seed();

            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new ClientAutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            _dataAccess = new OfferSearchDataAccess(_mapper, _context);
        }
        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients ON");
                _context.Clients.AddRange(
                    new ClientDb { ClientID = 1, Username = "TestUsername1", Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1", Password = "TestPassword1" },
                    new ClientDb { ClientID = 2, Username = "TestUsername2", Email = "TestEmail2", Name = "TestName2", Surname = "TestSurname2", Password = "TestPassword2" },
                    new ClientDb { ClientID = 3, Username = "TestUsername3", Email = "TestEmail3", Name = "TestName3", Surname = "TestSurname3", Password = "TestPassword3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Clients OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels ON");
                _context.Hotels.AddRange(
                    new HotelDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDescription = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" },
                    new HotelDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDescription = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2" },
                    new HotelDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDescription = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Offers ON");
                _context.Offers.AddRange(
                    new OfferDb { OfferID = 1, HotelID = 2, OfferTitle = "TestOfferTitle1", OfferPreviewPicture = "TestOfferPreviewPicture1", IsActive = true, IsDeleted = false, CostPerChild = 10, CostPerAdult = 11, MaxGuests = 1, Description = "TestDescription1" },
                    new OfferDb { OfferID = 2, HotelID = 3, OfferTitle = "TestOfferTitle2", OfferPreviewPicture = "TestOfferPreviewPicture2", IsActive = true, IsDeleted = false, CostPerChild = 20, CostPerAdult = 22, MaxGuests = 2, Description = "TestDescription2" },
                    new OfferDb { OfferID = 3, HotelID = 3, OfferTitle = "TestOfferTitle3", OfferPreviewPicture = "TestOfferPreviewPicture3", IsActive = false, IsDeleted = true, CostPerChild = 30, CostPerAdult = 33, MaxGuests = 3, Description = "TestDescription3" },
                    new OfferDb { OfferID = 4, HotelID = 3, OfferTitle = "TestOfferTitle4", OfferPreviewPicture = "TestOfferPreviewPicture4", IsActive = true, IsDeleted = false, CostPerChild = 200, CostPerAdult = 220, MaxGuests = 10, Description = "TestDescription4" },
                    new OfferDb { OfferID = 5, HotelID = 3, OfferTitle = "TestOfferTitle5", OfferPreviewPicture = "TestOfferPreviewPicture5", IsActive = true, IsDeleted = false, CostPerChild = 80, CostPerAdult = 120, MaxGuests = 7, Description = "TestDescription5" },
                    new OfferDb { OfferID = 6, HotelID = 3, OfferTitle = "TestOfferTitle6", OfferPreviewPicture = "TestOfferPreviewPicture6", IsActive = true, IsDeleted = false, CostPerChild = 2000, CostPerAdult = 4920, MaxGuests = 30, Description = "TestDescription6" });
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
                    new HotelRoomDb { RoomID = 1, HotelID = 2, IsActive = true, HotelRoomNumber = "TestHotelRoomNumber1" },
                    new HotelRoomDb { RoomID = 2, HotelID = 3, IsActive = true, HotelRoomNumber = "TestHotelRoomNumber2" },
                    new HotelRoomDb { RoomID = 3, HotelID = 3, IsActive = true, HotelRoomNumber = "TestHotelRoomNumber3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRooms OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations ON");
                _context.ClientReservations.AddRange(
                    new ClientReservationDb { ReservationID = 1, OfferID = 2, ClientID = 1, HotelID = 2, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 1, 2) },
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = 2, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(3001, 3, 3), ToTime = new DateTime(3001, 3, 6) });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations OFF");

                _context.OfferHotelRooms.AddRange(
                    new OfferHotelRoomDb { OfferID = 1, RoomID = 1 },
                    new OfferHotelRoomDb { OfferID = 2, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 4, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 5, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 6, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 3 });
                _context.SaveChanges();

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReviews ON");
                _context.ClientReviews.AddRange(
                    new ClientReviewDb { ReviewID = 1, ClientID = 1, HotelID = 2, Content = "hotel2", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 1, OfferID = 2 },
                    new ClientReviewDb { ReviewID = 2, ClientID = 2, HotelID = 3, Content = "hotel3", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 2, OfferID = 3 },
                    new ClientReviewDb { ReviewID = 3, ClientID = 3, HotelID = 3, Content = "hotel3", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 3, OfferID = 3 }
                    );
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReviews OFF");

                transaction.Commit();
            }
        }
        #endregion

        private ServerDbContext _context;
        private IMapper _mapper;
        private OfferSearchDataAccess _dataAccess;
        [Fact]
        public void CheckHotelExistence_NonExistentHotelID_ReturnsFalse()
        {
            int hotelID = 4;

            bool result = _dataAccess.CheckHotelExistence(hotelID);

            Assert.False(result);
        }

        [Fact]
        public void CheckHotelExistence_ValidHotelID_ReturnsTrue()
        {
            int[] hotelIDs = new int[] { 1, 2, 3 };
            bool[] results = new bool[hotelIDs.Length];
            
            for(int i = 0; i < hotelIDs.Length; i++)
                results[i] = _dataAccess.CheckHotelExistence(hotelIDs[i]);

            for (int i = 0; i < hotelIDs.Length; i++)
                Assert.True(results[i]);
        }

        [Fact]
        public void CheckHotelOfferAvailability_AvailableInterval_ReturnsTrue()
        {
            int offerID = 2;
            DateTime from = new DateTime(4000, 10, 10);
            DateTime to = new DateTime(4000, 10, 11);

            bool result = _dataAccess.CheckHotelOfferAvailability(offerID, from, to);

            Assert.True(result);
        }

        [Fact]
        public void CheckHotelOfferAvailability_UnavailableInterval_ReturnsFalse()
        {
            int offerID = 2;
            DateTime from = new DateTime(2001, 1, 1);
            DateTime to = new DateTime(2001, 1, 2);

            bool result = _dataAccess.CheckHotelOfferAvailability(offerID, from, to);

            Assert.False(result);
        }
        [Fact]
        public void GetHotelOffers_PagingNullOrFilterNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _dataAccess.GetHotelOffers(-1, null, new Paging()));
            Assert.Throws<ArgumentNullException>(() => _dataAccess.GetHotelOffers(-1, new OfferFilter(), null));
            Assert.Throws<ArgumentNullException>(() => _dataAccess.GetHotelOffers(-1, null, null));
        }

        [Fact]
        public void GetHotelOffers_AvailableIntervalNoFilter_ReturnsActiveAndNotDeletedOffers()
        {
            int hotelID = 3;
            Paging paging = new Paging(number: 1, size: 1000);
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(4000, 10, 10),
                ToTime = new DateTime(4000, 10, 11)
            };

            List<OfferPreviewView> clientOfferPreviews = _dataAccess.GetHotelOffers(hotelID, offerFilter, paging);
            List<OfferDb> allOffers = _context.Offers.Where(o => o.IsActive && !o.IsDeleted && o.HotelID == hotelID).ToList();

            Assert.Equal(allOffers.Count, clientOfferPreviews.Count);
            foreach(OfferPreviewView preview in clientOfferPreviews)
            {
                Assert.Contains(allOffers, odb => odb.OfferID == preview.OfferID);
            }
        }

        [Fact]
        public void GetHotelOffers_AvailableIntervalMaxCostFilter_ReturnsValidFilteredOffers()
        {
            int hotelID = 3;
            Paging paging = new Paging(number: 1, size: 1000);
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(4000, 10, 10),
                ToTime = new DateTime(4000, 10, 11),
                CostMax = 100
            };

            List<OfferPreviewView> clientOfferPreviews = _dataAccess.GetHotelOffers(hotelID, offerFilter, paging);
            List<OfferDb> allOffers = _context.Offers.Where(o => 
                o.IsActive && 
                !o.IsDeleted && 
                o.HotelID == hotelID &&
                o.CostPerChild <= offerFilter.CostMax &&
                o.CostPerAdult <= offerFilter.CostMax).ToList();

            Assert.Equal(allOffers.Count, clientOfferPreviews.Count);
            foreach (OfferPreviewView preview in clientOfferPreviews)
            {
                Assert.Contains(allOffers, odb => odb.OfferID == preview.OfferID);
            }
        }

        [Fact]
        public void GetHotelOffers_AvailableIntervalMinCostFilter_ReturnsValidFilteredOffers()
        {
            int hotelID = 3;
            Paging paging = new Paging(number: 1, size: 1000);
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(4000, 10, 10),
                ToTime = new DateTime(4000, 10, 11),
                CostMin = 100
            };

            List<OfferPreviewView> clientOfferPreviews = _dataAccess.GetHotelOffers(hotelID, offerFilter, paging);
            List<OfferDb> allOffers = _context.Offers.Where(o =>
                o.IsActive &&
                !o.IsDeleted &&
                o.HotelID == hotelID &&
                o.CostPerChild >= offerFilter.CostMin &&
                o.CostPerAdult >= offerFilter.CostMin).ToList();

            Assert.Equal(allOffers.Count, clientOfferPreviews.Count);
            foreach (OfferPreviewView preview in clientOfferPreviews)
            {
                Assert.Contains(allOffers, odb => odb.OfferID == preview.OfferID);
            }
        }

        [Fact]
        public void GetHotelOffers_AvailableIntervalMinGuestsFilter_ReturnsValidFilteredOffers()
        {
            int hotelID = 3;
            Paging paging = new Paging(number: 1, size: 1000);
            OfferFilter offerFilter = new OfferFilter()
            {
                FromTime = new DateTime(4000, 10, 10),
                ToTime = new DateTime(4000, 10, 11),
                MinGuests = 10
            };

            List<OfferPreviewView> clientOfferPreviews = _dataAccess.GetHotelOffers(hotelID, offerFilter, paging);
            List<OfferDb> allOffers = _context.Offers.Where(o =>
                o.IsActive &&
                !o.IsDeleted &&
                o.HotelID == hotelID &&
                o.MaxGuests >= offerFilter.MinGuests).ToList();

            Assert.Equal(allOffers.Count, clientOfferPreviews.Count);
            foreach (OfferPreviewView preview in clientOfferPreviews)
            {
                Assert.Contains(allOffers, odb => odb.OfferID == preview.OfferID);
            }
        }

        [Fact]
        public void CheckHotelOfferExistence_NonExistentHotelOrOffer_ReturnsFalse()
        {
            int hotelID = 10;
            int offerID = 21;

            bool result = _dataAccess.CheckHotelOfferExistence(hotelID, offerID);

            Assert.False(result);
        }

        [Fact]
        public void CheckHotelOfferExistence_ExistentHotelButInvalidOfferOrNoOwnership_ReturnsFalse()
        {
            int hotelID = 3;
            int offerID = 1;
            int offerIDNonExistent = 12;

            bool resultExistent = _dataAccess.CheckHotelOfferExistence(hotelID, offerID);
            bool resultNonExistent = _dataAccess.CheckHotelOfferExistence(hotelID, offerIDNonExistent);

            Assert.False(resultExistent);
            Assert.False(resultNonExistent);
        }

        [Fact]
        public void CheckHotelOfferExistence_ValidHotelIDAndOfferID_ReturnsTrue()
        {
            int hotelID = 3;
            int offerID = 3;

            bool result = _dataAccess.CheckHotelOfferExistence(hotelID, offerID);

            Assert.True(result);
        }

        [Fact]
        public void GetHotelOfferDetails_NonExistentOffer_ReturnsNull()
        {
            int offerID = -1;

            OfferView clientOffer = _dataAccess.GetHotelOfferDetails(offerID);

            Assert.Null(clientOffer);
        }

        [Fact]
        public void GetHotelOfferDetails_ValidOffer_ReturnsOfferDetailsWithoutPictures()
        {
            int offerID = 3;

            OfferView clientOffer = _dataAccess.GetHotelOfferDetails(offerID);
            OfferDb offerDb = _context.Offers.Find(offerID);

            Assert.Equal(offerDb.OfferID, clientOffer.OfferID);
        }

        [Fact]
        public void GetHotelOfferPictures_NonExistentOffer_ReturnsEmptyList()
        {
            int offerID = -1;

            List<string> offerPictures = _dataAccess.GetHotelOfferPictures(offerID);

            Assert.Empty(offerPictures);
        }

        [Fact]
        public void GetHotelOfferPictures_ValidOffer_ReturnsAllOfferPictures()
        {
            int offerID = 3;

            List<string> offerPictures = _dataAccess.GetHotelOfferPictures(offerID);
            List<string> offerPicturesDb = _context.OfferPictures.Where(p => p.OfferID == offerID).Select(pdb => pdb.Picture).ToList();
            offerPictures.Sort();
            offerPicturesDb.Sort();

            Assert.Equal(offerPicturesDb.Count, offerPictures.Count);
            for(int i = 0; i < offerPictures.Count; i++)
            {
                Assert.Equal(offerPicturesDb[i], offerPictures[i]);
            }
        }

        [Fact]
        public void GetHotelOfferAvailability_FromAndToInsideReservationPeriod_ReturnsEmptyAvailabilityList()
        {
            int hotelID = 3;
            int offerID = 2;

            List<AvailabilityTimeInterval> timeIntervals = _dataAccess.GetHotelOfferAvailability(hotelID, offerID, new DateTime(2001, 1, 1), new DateTime(2001, 1, 2));

            Assert.Empty(timeIntervals);
        }

        [Fact]
        public void GetHotelOfferAvailability_TwoReservationsExclusivelyInsideFromAndToPerioD_ReturnsAvailabilityListWith3TimeIntervals()
        {
            int hotelID = 3;
            int offerID = 2;
            List<AvailabilityTimeInterval> expectedTimeIntervals = new List<AvailabilityTimeInterval>()
            {
                new AvailabilityTimeInterval(new DateTime(2000, 1, 1), new DateTime(2001, 1, 1).AddDays(-1)),
                new AvailabilityTimeInterval(new DateTime(2001, 1, 2).AddDays(1), new DateTime(2001, 2, 2).AddDays(-1)),
                new AvailabilityTimeInterval(new DateTime(3001, 2, 4).AddDays(1), new DateTime(3002, 1, 1))
            };

            List<AvailabilityTimeInterval> timeIntervals = _dataAccess.GetHotelOfferAvailability(hotelID, offerID, new DateTime(2000, 1, 1), new DateTime(3002, 1, 1));

            Assert.Equal(3, timeIntervals.Count);
            for(int i = 0; i < timeIntervals.Count; i++)
            {
                Assert.Equal(expectedTimeIntervals[i], timeIntervals[i]);
            }
        }

        [Fact]
        public void GetOfferReviews_ReturnsListOfOfferReviews()
        {
            int hotelID = 1;
            int offerID = 1;
            Paging paging = new Paging();

            List<ReviewView> testedReviews = _dataAccess.GetOfferReviews(hotelID, offerID, paging);
            List<ClientReviewDb> reviews = _context.ClientReviews
                                                   .Where(cr => cr.OfferID == offerID)
                                                   .OrderByDescending(cr => cr.ReviewID)
                                                   .Skip((paging.PageNumber - 1) * paging.PageSize)
                                                   .Take(paging.PageSize)
                                                   .ToList();

            Assert.Equal(testedReviews.Count, reviews.Count);
            for (int i = 0; i < testedReviews.Count; i++)
                Assert.Equal(reviews[i].ReviewID, testedReviews[i].ReviewID);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
