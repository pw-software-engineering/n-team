using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess.Hotel;
using Server.Database.Models;
using Server.RequestModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database.Hotel
{
    public class OfferRoomDataAccessTest : IDisposable
    {
        #region TestsSetup
        public OfferRoomDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=ServerDbTestsOfferRooms;Trusted_Connection=True;MultipleActiveResultSets=true")
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

            _dataAccess = new OfferRoomDataAccess(_mapper, _context);
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
                    new ClientReservationDb { ReservationID = 1, OfferID = 2, ClientID = null, HotelID = 2, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 1, 2) },
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = null, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = null, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(2001, 3, 3), ToTime = new DateTime(2001, 3, 6) });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations OFF;");

                _context.SaveChanges();
                transaction.Commit();
            }
        }
        #endregion

        private ServerDbContext _context;
        private IMapper _mapper;
        private OfferRoomDataAccess _dataAccess;

        [Fact]
        public void AddRoomToOffer_RoomIsAddedToOffer()
        {
            int roomID = 2;
            int offerID = 1;

            _dataAccess.AddRoomToOffer(roomID, offerID);

            Assert.True(_context.OfferHotelRooms.Any(ohr => ohr.OfferID == offerID && ohr.RoomID == roomID));
        }
        [Fact]
        public void DoesRoomHaveUnfinishedReservations_HaveUnfinishedReservations_ReturnsTrue()
        {
            int roomID = 2;
            int offerID = 3;

            bool haveUnfinishedReservations = _dataAccess.DoesRoomHaveUnfinishedReservations(roomID, offerID);

            Assert.True(haveUnfinishedReservations);
        }
        [Fact]
        public void DoesRoomHaveUnfinishedReservations_DoesNotHaveUnfinishedReservations_ReturnsFalse()
        {
            int roomID = 3;
            int offerID = 3;

            bool haveUnfinishedReservations = _dataAccess.DoesRoomHaveUnfinishedReservations(roomID, offerID);

            Assert.False(haveUnfinishedReservations);
        }
        [Fact]
        public void FindOfferAndGetOwner_NoOfferWithGivenID_ReturnsNull()
        {
            int offerID = -1;

            int? ownerID = _dataAccess.FindOfferAndGetOwner(offerID);

            Assert.Null(ownerID);
        }
        [Fact]
        public void FindOfferAndGetOwner_ReturnsOwnerID()
        {
            int offerID = 1;
            int ownerID = 2;

            int? testOwnerID = _dataAccess.FindOfferAndGetOwner(offerID);

            Assert.Equal(ownerID, testOwnerID);
        }
        [Fact]
        public void FindRoomAndGetOwner_NoOfferWithGivenID_ReturnsNull()
        {
            int roomID = -1;

            int? ownerID = _dataAccess.FindRoomAndGetOwner(roomID);

            Assert.Null(ownerID);
        }
        [Fact]
        public void FindRoomAndGetOwner_ReturnsOwnerID()
        {
            int roomID = 1;
            int ownerID = 2;

            int? testOwnerID = _dataAccess.FindRoomAndGetOwner(roomID);

            Assert.Equal(ownerID, testOwnerID);
        }
        [Fact]
        public void FindOfferAndGetOwner_NoRoomWithGivenRoomNumber_ReturnsNull()
        {
            string hotelRoomNumber = "Invalid hotel room number";

            int? ownerID = _dataAccess.FindRoomAndGetOwner(hotelRoomNumber);

            Assert.Null(ownerID);
        }
        [Fact]
        public void FindOfferAndGetOwner_ValidRoomNumber_ReturnsOwnerID()
        {
            string hotelRoomNumber = "TestHotelRoomNumber1";
            int ownerID = 2;

            int? testOwnerID = _dataAccess.FindRoomAndGetOwner(hotelRoomNumber);

            Assert.Equal(ownerID, testOwnerID);
        }
        [Fact]
        public void GetOfferRooms_RoomNumberGiven_ReturnsOfferRoomViewsListWithOneElement()
        {
            int offerID = 1;
            string hotelRoomNumber = "TestHotelRoomNumber1";
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 10
            };

            List<OfferRoomView> roomViews = _dataAccess.GetOfferRooms(offerID, paging, hotelRoomNumber);

            Assert.Single(roomViews);
            Assert.Equal(hotelRoomNumber, roomViews[0].HotelRoomNumber);
        }
        [Fact]
        public void GetOfferRooms_NoRoomNumberGiven_ReturnsOfferRoomViewsList()
        {
            int offerID = 1;
            Paging paging = new Paging()
            {
                PageNumber = 1,
                PageSize = 10
            };

            List<OfferRoomView> testRoomViews = _dataAccess.GetOfferRooms(offerID, paging);
            List<OfferHotelRoomDb> rooms = _context.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID)
                                                                   .Skip((paging.PageNumber - 1) * paging.PageSize)
                                                                   .Take(paging.PageSize)
                                                                   .ToList();

            Assert.Equal(rooms.Count, testRoomViews.Count);
            for(int i = 0; i<rooms.Count;i++)
                Assert.Equal(rooms[i].RoomID, testRoomViews[i].RoomID);
        }
        [Fact]
        public void IsRoomAlreadyAddedToOffer_RoomAlreadyAdded_ReturnsTrue()
        {
            int roomID = 1;
            int offerID = 1;

            bool isRoomAlreadyAdded = _dataAccess.IsRoomAlreadyAddedToOffer(roomID, offerID);

            Assert.True(isRoomAlreadyAdded);
        }
        [Fact]
        public void IsRoomAlreadyAddedToOffer_RoomNotAlreadyAdded_ReturnsFalse()
        {
            int roomID = 2;
            int offerID = 1;

            bool isRoomAlreadyAdded = _dataAccess.IsRoomAlreadyAddedToOffer(roomID, offerID);

            Assert.False(isRoomAlreadyAdded);
        }
        [Fact]
        public void UnpinRoomFromOffer_Removes()
        {
            int roomID = 1;
            int offerID = 1;

            _dataAccess.UnpinRoomFromOffer(roomID, offerID);

            Assert.False(_context.OfferHotelRooms.Any(ohr => ohr.RoomID == roomID && ohr.OfferID == offerID));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
