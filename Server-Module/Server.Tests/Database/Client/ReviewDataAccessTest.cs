using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess.Client.Review;
using Server.Database.Models;
using Server.ViewModels.Client;
using System;
using System.IO;
using Xunit;

namespace Server.Tests.Database.Client
{
    public class ReviewDataAccessTest : IDisposable
    {

        #region TestsSetup
        public ReviewDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer(configurationBuilder.GetConnectionString("ReviewDAClientTest"))
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

            _dataAccess = new ReviewDataAccess(_context);
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
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels OFF");

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
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime =  DateTime.Now },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(3001, 3, 3), ToTime = new DateTime(3001, 3, 6) });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations OFF");

                _context.OfferHotelRooms.AddRange(
                    new OfferHotelRoomDb { OfferID = 1, RoomID = 1 },
                    new OfferHotelRoomDb { OfferID = 2, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 3 });
                _context.SaveChanges();

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReviews ON");
                _context.ClientReviews.AddRange(
                    new ClientReviewDb { ReviewID=1,ClientID = 1, HotelID = 2, Content = "content1", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 1, OfferID = 2 },
                    new ClientReviewDb { ReviewID = 2,ClientID = 3, HotelID = 3, Content = "content2", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 2, OfferID = 3 },
                    new ClientReviewDb { ReviewID = 3, ClientID = 3, HotelID = 3, Content = "content2", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 3, OfferID = 3 }

                    );
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReviews OFF");

                transaction.Commit();
            }
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
        #endregion
        private ServerDbContext _context;
        private IMapper _mapper;
        private ReviewDataAccess _dataAccess;

        #region GetReview
        [Fact]
        public void GetReview_BadID()
        {
            int reservationID = 4;
            Assert.Throws<Exception>(()=>_dataAccess.GetReview(reservationID));
        }
        [Fact]
        public void GetReview_GoodID()
        {
            int reservationID = 1;
            var good_result = _context.ClientReviews.Find(reservationID);
            var user = _context.Clients.Find(good_result.ClientID);
            var result = _dataAccess.GetReview(reservationID);
            Assert.True(result.creationDate == good_result.ReviewDate && result.content == good_result.Content&& result.revewerUsername==user.Name);
        }
        #endregion
        #region DeleteReview
        [Fact]
        public void DeleteReview_BadID()
        {
            int reservationID = 4;
            Assert.Throws<Exception>(() => _dataAccess.DeleteReview(reservationID));
        }
        [Fact]
        public void DeleteReview_GoodID()
        {
            int reservationID = 2;
            _dataAccess.DeleteReview(reservationID);
            var reservation = _context.ClientReservations.Find(reservationID);
            Assert.True(reservation.ReviewID == null && _context.ClientReviews.Find(reservationID) == null);
        }
        #endregion
        #region IsReviewExist
        [Fact]
        public void IsReviewExist_Test()
        {
            var result = _dataAccess.IsReviewExist(4);
            var result2 = _dataAccess.IsReviewExist(1);
            Assert.True(!result && !result);
        }
        #endregion
        #region IsClientTheOwnerOfReservation
        [Fact]
        public void IsClientTheOwnerOfReservation_Test()
        {
            bool t1=_dataAccess.IsClientTheOwnerOfReservation(1, 2);
            bool t2=_dataAccess.IsClientTheOwnerOfReservation(1, 1);
            bool t3=_dataAccess.IsClientTheOwnerOfReservation(4, 1);
            Assert.True(t1 && !t2 && !t3);
        }
        #endregion
        #region AddNewReview
        [Fact]
        public void AddNewReview_BadID()
        {
            var paramertr = new ReviewUpdater { content = "this is updated content", rating = 10 };
            Assert.Throws<Exception>(()=> _dataAccess.AddNewReview(4, paramertr));
        }
        [Fact]
        public void AddNewReview_NullParameter()
        {
            Assert.Throws<Exception>(() => _dataAccess.AddNewReview(3, null));
        }
        [Fact]
        public void AddNewReview_GoodTest()
        {
            var paramertr = new ReviewUpdater { content = "this is updated content", rating = 10 };
            int reviewID = _dataAccess.AddNewReview(3, paramertr);
            var result = _context.ClientReviews.Find(reviewID);
            Assert.True(result.Rating == paramertr.rating && result.Content == paramertr.content);
        }
        #endregion
        #region UpdateReview
        [Fact]
        public void UpdateReview_NullUpadater()
        {
            Assert.Throws<Exception>(() => _dataAccess.UpdateReview(3, null));
        }
        [Fact]
        public void UpdateReview_BadId()
        {
            var paramertr = new ReviewUpdater { content = "this is updated content", rating = 10 };
            Assert.Throws<Exception>(() => _dataAccess.UpdateReview(55, paramertr));
        }
        [Fact]
        public void UpdateReview_GoodTest()
        {
            var paramertr = new ReviewUpdater { content = "this is updated content", rating = 10 };
            int reviewId = _dataAccess.UpdateReview(3, paramertr);
            var result = _context.ClientReviews.Find(reviewId);
            Assert.True(result.Content == paramertr.content);

        }
        #endregion
        #region IsDataValid
        [Fact]
        public void IsDataValid_InvalidDataToLow()
        {
            Assert.True(!_dataAccess.IsDataValid(new ReviewUpdater { content = "a", rating = 0 }));
        }
        [Fact]
        public void IsDataValid_InvalidDataToHight()
        {
            Assert.True(!_dataAccess.IsDataValid(new ReviewUpdater { content = "a", rating = 11 }));
        }
        [Fact]
        public void IsDataValid_GoodValues()
        {
            // _dataAccess.IsDataValid
            for(int i=1;i<11;i++)
                Assert.True(_dataAccess.IsDataValid(new ReviewUpdater { content = "a", rating = i }));
        }
        #endregion
        #region IsAddingReviewToReservationEnabled
        [Fact]
        public void IsAddingReviewToReservationEnabled_ToLate()
        {
            Assert.True(!_dataAccess.IsAddingReviewToReservationEnabled(1));
        }
        [Fact]
        public void IsAddingReviewToReservationEnabled_ToEarly()
        {
            Assert.True(!_dataAccess.IsAddingReviewToReservationEnabled(3));
        }
        [Fact]
        public void IsAddingReviewToReservationEnabled_Goodtest()
        {
            Assert.True(!_dataAccess.IsAddingReviewToReservationEnabled(2));
        }
        [Fact]
        public void IsAddingReviewToReservationEnabled_BadID()
        {
            Assert.Throws<Exception>(() => _dataAccess.IsAddingReviewToReservationEnabled(100));
        }
        #endregion
    }
}
