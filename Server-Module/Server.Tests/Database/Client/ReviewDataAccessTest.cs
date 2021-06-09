using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess.Client.Review;
using Server.Database.Models;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System;
using System.IO;
using System.Linq;
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

            _dataAccess = new ReviewDataAccess(_context, _mapper);
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

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReviews ON");
                _context.ClientReviews.AddRange(
                    new ClientReviewDb { ReviewID = 1, ClientID = 1, HotelID = 2, Content = "content1", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 1, OfferID = 2 },
                    new ClientReviewDb { ReviewID = 2, ClientID = 3, HotelID = 3, Content = "content2", Rating = 4, ReviewDate = DateTime.UtcNow, ReservationID = 1, OfferID = 3 });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReviews OFF");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations ON");
                _context.ClientReservations.AddRange(
                    new ClientReservationDb { ReservationID = 1, OfferID = 2, ClientID = 2, HotelID = 2, RoomID = 2, ReviewID = 1, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 1, 2) },
                    new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 2, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = DateTime.UtcNow },
                    new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 3, ReviewID = null, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(2001, 3, 3), ToTime = DateTime.Now.AddDays(-1) });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ClientReservations OFF");

                _context.OfferHotelRooms.AddRange(
                    new OfferHotelRoomDb { OfferID = 1, RoomID = 1 },
                    new OfferHotelRoomDb { OfferID = 2, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 2 },
                    new OfferHotelRoomDb { OfferID = 3, RoomID = 3 });
                _context.SaveChanges();

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

        [Fact]
        public void DoesReviewExist_ReviewExists_ReturnsTrue()
        {
            int reservationID = 1;

            bool doesExist = _dataAccess.DoesReviewExist(reservationID);

            Assert.True(doesExist);
        }
        [Fact]
        public void DoesReviewExist_ReviewDoesNotExist_ReturnsFalse()
        {
            int reservationID = 2;

            bool doesExist = _dataAccess.DoesReviewExist(reservationID);

            Assert.False(doesExist);
        }
        [Fact]
        public void EditReview_ReviewUpdateIsNull_ThrowsArgumentNullException()
        {
            int reservationID = 1;
            ReviewUpdate reviewUpdate = null;

            Action action = () => _dataAccess.EditReview(reservationID, reviewUpdate);

            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void EditReview_ReturnsModifiedReviewID()
        {
            int reservationID = 1;
            ReviewUpdate reviewUpdate = new ReviewUpdate()
            {
                Content = "TestContent",
                Rating = 1
            };

            int reviewID = _dataAccess.EditReview(reservationID, reviewUpdate);
            ClientReviewDb review = _context.ClientReviews.Find(reviewID);

            Assert.NotNull(review);
            Assert.Equal(reviewUpdate.Content, review.Content);
            Assert.Equal(reviewUpdate.Rating, (int)review.Rating);
        }
        [Fact]
        public void AddReview_ReviewUpdateIsNull_ThrowsArgumentNullException()
        {
            int reservationID = 1;
            ReviewUpdate reviewUpdate = null;

            Action action = () => _dataAccess.AddReview(reservationID, reviewUpdate);

            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void AddReview_ReturnsAddedReviewID()
        {
            int reservationID = 2;
            ReviewUpdate reviewUpdate = new ReviewUpdate()
            {
                Content = "TestContent",
                Rating = 1
            };

            int reviewID = _dataAccess.AddReview(reservationID, reviewUpdate);
            ClientReviewDb review = _context.ClientReviews.Find(reviewID);
            ClientReservationDb reservation = _context.ClientReservations.Find(reservationID);

            Assert.NotNull(review);
            Assert.Equal(reviewID, reservation.ReviewID);
            Assert.Equal(reservationID, review.ReservationID);
            Assert.Equal(reservation.OfferID, review.OfferID);
            Assert.Equal(reservation.HotelID, review.HotelID);
            Assert.Equal(reservation.ClientID, review.ClientID);
            Assert.Equal(reviewUpdate.Content, review.Content);
            Assert.Equal(reviewUpdate.Rating, (int)review.Rating);
        }
        [Fact]
        public void GetReview_ReturnsReviewView()
        {
            int reservationID = 1;

            ReviewView testedReview = _dataAccess.GetReview(reservationID);
            ClientReviewDb review = _context.ClientReviews.First(cr => cr.ReservationID == reservationID);
            string clientName = _context.Clients.Find(review.ClientID).Username;

            Assert.Equal(review.ReviewID, testedReview.ReviewID);
            Assert.Equal(review.Content, testedReview.Content);
            Assert.Equal((int)review.Rating, testedReview.Rating);
            Assert.Equal(review.ReviewDate, testedReview.CreationDate);
            Assert.Equal(clientName, testedReview.ReviewerUsername);
        }
        [Fact]
        public void FindReservationOwner_NoReservation_ReturnsNull()
        {
            int reservationID = -1;

            int? ownerID = _dataAccess.FindReservationOwner(reservationID);

            Assert.Null(ownerID);
        }
        [Fact]
        public void FindReservationOwner_ReturnsOwnerID()
        {
            int reservationID = 1;
            int ownerID = 2;

            int? ownerTestID = _dataAccess.FindReservationOwner(reservationID);

            Assert.Equal(ownerID, ownerTestID);
        }
        [Fact]
        public void DeleteReview_ReviewIsDeleted()
        {
            int reservationID = 1;
            int reviewID = _context.ClientReviews.First(cr => cr.ReservationID == reservationID).ReviewID;

            _dataAccess.DeleteReview(reservationID);
            ClientReviewDb review = _context.ClientReviews.Find(reviewID);

            Assert.Null(review);
            Assert.Null(_context.ClientReservations.Find(reservationID).ReviewID);
        }
        [Fact]
        public void IsReviewChangeAllowed_ChangeAllowed_ReturnsTrue()
        {
            int reservationID = 3;

            bool changeAllowed = _dataAccess.IsReviewChangeAllowed(reservationID);

            Assert.True(changeAllowed);
        }
        [Fact]
        public void IsReviewChangeAllowed_ChangeNotAllowed_ReturnsFalse()
        {
            int reservationID = 1;

            bool changeAllowed = _dataAccess.IsReviewChangeAllowed(reservationID);

            Assert.False(changeAllowed);
        }
    }
}
