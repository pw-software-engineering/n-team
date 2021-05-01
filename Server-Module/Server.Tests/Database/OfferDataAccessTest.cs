using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.Models;
using Server.Models;
using Server.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
////Optional
//[assembly: CollectionBehavior(DisableTestParallelization = true)]
////Optional
//[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
////Optional
//[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]

namespace Server.Tests.Database
{
    public class OfferDataAccessTest : IDisposable
    {
        #region TestsSetup
        public OfferDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=ServerDbTestsOffer;Trusted_Connection=True;MultipleActiveResultSets=true")
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

            _dataAccess = new OfferDataAccess(_mapper, _context);
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
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelRoom OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations ON");
                _context.ClientReservations.AddRange(
                    new ClientReservationDb { ReservationID = 1, OfferID = 2, ClientID = null, HotelID = 2, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 1, 2) },
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = null, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = null, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(2001, 3, 3), ToTime = new DateTime(2001, 3, 6) });
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
        private OfferDataAccess _dataAccess;

        [Fact]
        public void DeleteOffer_ChangesDeletetionMark()
        {
            int OfferID = 1;

            _dataAccess.DeleteOffer(OfferID);
            OfferDb offer = _context.Offers.Find(OfferID);

            Assert.True(offer.IsDeleted);
        }
        [Fact]
        public void GetOfferRooms_ReturnsListOfOfferRoomNames()
        {
            int offerID = 3;

            List<string> offerRoomsTest = _dataAccess.GetOfferRooms(offerID);
            List<OfferHotelRoomDb> offerRooms = _context.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID)
                                                                        .Include(ohr => ohr.Room)
                                                                        .ToList();

            Assert.Equal(offerRooms.Count, offerRoomsTest.Count);
            for (int i = 0; i < offerRooms.Count; i++)
                Assert.Equal(offerRooms[i].Room.HotelRoomNumber, offerRoomsTest[i]);
        }
        [Fact]
        public void GetOfferPictures_ReturnsListOfOfferPictures()
        {
            int offerID = 3;

            List<string> offerPicturesTest = _dataAccess.GetOfferPictures(offerID);
            List<OfferPictureDb> offerPictures = _context.OfferPictures.Where(op => op.OfferID == offerID).ToList();

            Assert.Equal(offerPictures.Count, offerPicturesTest.Count);
            for (int i = 0; i < offerPictures.Count; i++)
                Assert.Equal(offerPictures[i].Picture, offerPicturesTest[i]);
        }
        [Fact]
        public void GetOffer_ReturnsValidOfferObject()
        {
            int OfferID = 1;

            Offer offerTest = _dataAccess.GetOffer(OfferID);
            OfferDb offer = _context.Offers.Find(OfferID);

            Assert.Equal(offer.HotelID, offerTest.HotelID);
            Assert.Equal(offer.IsActive, offerTest.IsActive);
            Assert.Equal(offer.IsDeleted, offerTest.IsDeleted);
            Assert.Equal(offer.MaxGuests, offerTest.MaxGuests);
            Assert.Equal(offer.OfferID, offerTest.OfferID);
            Assert.Equal(offer.OfferPreviewPicture, offerTest.OfferPreviewPicture);
            Assert.Equal(offer.OfferTitle, offerTest.OfferTitle);
            Assert.Equal(offer.Description, offerTest.Description);
            Assert.Equal(offer.CostPerChild, offerTest.CostPerChild);
            Assert.Equal(offer.CostPerAdult, offerTest.CostPerAdult);
        }

        [Fact]
        public void GetOffer_InvalidOfferID_ReturnsNull()
        {
            int offerID = -1;

            Offer offer = _dataAccess.GetOffer(offerID);

            Assert.Null(offer);
        }

        [Fact]
        public void GetHotelOffers_IsActiveSetToTrue_ReturnsListOfActiveOffersPreviewsObjects()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            bool? isActive = true;

            List<OfferPreview> offersPreviewsTest = _dataAccess.GetHotelOffers(paging, hotelID, isActive);
            List<OfferDb> offersPreviews = _context.Offers.Where(o => o.HotelID == hotelID && o.IsActive == isActive).ToList();

            Assert.Equal(offersPreviews.Count, offersPreviewsTest.Count);

            for (int i = 0; i < offersPreviews.Count; i++)
            {
                OfferDb offer = offersPreviews[i];
                OfferPreview offerTest = offersPreviewsTest[i];

                Assert.Equal(offer.IsActive, offerTest.IsActive);
                Assert.Equal(offer.MaxGuests, offerTest.MaxGuests);
                Assert.Equal(offer.OfferPreviewPicture, offerTest.OfferPreviewPicture);
                Assert.Equal(offer.OfferTitle, offerTest.OfferTitle);
                Assert.Equal(offer.CostPerChild, offerTest.CostPerChild);
                Assert.Equal(offer.CostPerAdult, offerTest.CostPerAdult);
            }
        }
        [Fact]
        public void GetHotelOffers_IsActiveSetToFalse_ReturnsListOfInActiveOffersPreviewsObjects()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            bool? isActive = false;

            List<OfferPreview> offersPreviewsTest = _dataAccess.GetHotelOffers(paging, hotelID, isActive);
            List<OfferDb> offersPreviews = _context.Offers.Where(o => o.HotelID == hotelID && o.IsActive == isActive).ToList();

            Assert.Equal(offersPreviews.Count, offersPreviewsTest.Count);
        }
        [Fact]
        public void GetHotelOffers_IsActiveSetToNull_ReturnsListOfAllOfferPreviewsObjects()
        {
            int hotelID = 1;
            Paging paging = new Paging();
            bool? isActive = null;

            List<OfferPreview> offersPreviewsTest = _dataAccess.GetHotelOffers(paging, hotelID, isActive);
            List<OfferDb> offersPreviews = _context.Offers.Where(o => o.HotelID == hotelID).ToList();

            Assert.Equal(offersPreviews.Count, offersPreviewsTest.Count);
        }
        [Fact]
        public void FindOfferAndGetOwner_NotFound_ReturnsNull()
        {
            int offerID = -4;

            int? owner = _dataAccess.FindOfferAndGetOwner(offerID);

            Assert.Null(owner);
        }
        [Fact]
        public void FindOfferAndGetOwner_AssertsValidOwnerIsReturned()
        {
            int offerID = 3;

            int? ownerTest = _dataAccess.FindOfferAndGetOwner(offerID);
            int? owner = _context.Offers.Find(offerID).HotelID;

            Assert.Equal(owner, ownerTest);
        }

        [Fact]
        public void AddOfferPicture_OfferPictureIsAdded()
        {
            int offerID = 1;
            OfferPictureDb picture = new OfferPictureDb 
            { 
                OfferID = offerID, 
                Picture = "TESTPICTURE" 
            };

            int offerPicturesCount = _context.OfferPictures.Where(op => op.OfferID == offerID).Count();
            _dataAccess.AddOfferPicture(picture.Picture, picture.OfferID);
            int offerPicturesUpdatedCount = _context.OfferPictures.Where(op => op.OfferID == offerID).Count();
            OfferPictureDb offerPicture = _context.OfferPictures.Where(op => op.OfferID == offerID && op.Picture == picture.Picture).First();

            Assert.NotNull(offerPicture);
            Assert.Equal(offerPicturesCount + 1, offerPicturesUpdatedCount);
            Assert.Equal(offerPicture.Picture, picture.Picture);
            Assert.Equal(offerPicture.OfferID, offerID);
        }
        [Fact]
        public void AddOffer_OfferIsAdded()
        {
            Offer offer = new Offer
            { 
                HotelID = 2, 
                OfferTitle = "TestOfferTitle4", 
                OfferPreviewPicture = "TestOfferPreviewPicture4", 
                IsActive = true, 
                IsDeleted = false, 
                CostPerChild = 40, 
                CostPerAdult = 44, 
                MaxGuests = 4, 
                Description = "TestDescription4" 
            };

            int offerID = _dataAccess.AddOffer(offer);
            OfferDb offerTest = _context.Offers.Find(offerID);

            Assert.Equal(offer.HotelID, offerTest.HotelID);
            Assert.Equal(offer.IsActive, offerTest.IsActive);
            Assert.Equal(offer.IsDeleted, offerTest.IsDeleted);
            Assert.Equal(offer.MaxGuests, offerTest.MaxGuests);
            Assert.Equal(offer.OfferPreviewPicture, offerTest.OfferPreviewPicture);
            Assert.Equal(offer.OfferTitle, offerTest.OfferTitle);
            Assert.Equal(offer.Description, offerTest.Description);
            Assert.Equal(offer.CostPerChild, offerTest.CostPerChild);
            Assert.Equal(offer.CostPerAdult, offerTest.CostPerAdult);
        }
        [Fact]
        public void AddOfferPictures_OfferPicturesAreAdded()
        {
            List<string> pictures = new List<string>() 
            { 
                "Pic1", 
                "Pic2", 
                "Pic3" 
            };
            int offerID = 2;

            int offerPicturesCount = _context.OfferPictures.Where(op => op.OfferID == offerID).Count();
            _dataAccess.AddOfferPictures(pictures, offerID);
            int offerPicturesUpdatedCount = _context.OfferPictures.Where(op => op.OfferID == offerID).Count();

            Assert.Equal(offerPicturesCount + pictures.Count, offerPicturesUpdatedCount);
        }
        [Fact]
        public void UpdateOffer_OfferIsUpdated()
        {
            int offerID = 1;
            OfferUpdateInfo offerUpdate = new OfferUpdateInfo
            { 
                OfferTitle = "TestOfferTitle4", 
                OfferPreviewPicture = "TestOfferPreviewPicture4", 
                IsActive = true, 
                Description = "TestDescription4" 
            };

            _dataAccess.UpdateOffer(offerID, offerUpdate);
            OfferDb offerTest = _context.Offers.Find(offerID);

            Assert.NotNull(offerTest);
            Assert.Equal(offerUpdate.Description, offerTest.Description);
            Assert.Equal(offerUpdate.IsActive, offerTest.IsActive);
            Assert.Equal(offerUpdate.OfferPreviewPicture, offerTest.OfferPreviewPicture);
            Assert.Equal(offerUpdate.OfferTitle, offerTest.OfferTitle);
        }
        [Fact]
        public void AreThereUnfinishedReservationsForOffer_NoReservations_ReturnsFalse()
        {
            int offerID = 2;

            bool areThereUnfinishedReservations = _dataAccess.AreThereUnfinishedReservationsForOffer(offerID);

            Assert.False(areThereUnfinishedReservations);
        }
        [Fact]
        public void AreThereUnfinishedReservationsForOffer_ReservationsExist_ReturnsTrue()
        {
            int offerID = 3;

            bool areThereUnfinishedReservations = _dataAccess.AreThereUnfinishedReservationsForOffer(offerID);

            Assert.True(areThereUnfinishedReservations);
        }
        [Fact]
        public void UnpinRoomsFromOfffer_AllOfferRoomsEntitiesAreDeleted()
        {
            int offerID = 2;

            _dataAccess.UnpinRoomsFromOffer(offerID);
            int numberOfRoomsRelatedToOffer = _context.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID).Count();

            Assert.Equal(0, numberOfRoomsRelatedToOffer);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
