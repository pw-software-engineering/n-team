using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Client;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database.Client
{
    public class HotelSearchDataAccessTest : IDisposable
    {
        #region TestsSetup
        public HotelSearchDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer(configurationBuilder.GetConnectionString("HotelSearchDAClientTest"))
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

            _dataAccess = new HotelSearchDataAccess(_mapper, _context);
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

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelPictures ON");
                _context.HotelPictures.AddRange(
                    new HotelPictureDb { PictureID = 1, HotelID = 2, Picture = "TestPicture1" },
                    new HotelPictureDb { PictureID = 2, HotelID = 3, Picture = "TestPicture2" },
                    new HotelPictureDb { PictureID = 3, HotelID = 3, Picture = "TestPicture3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelPictures OFF");

                transaction.Commit();
            }
        }
        #endregion

        private ServerDbContext _context;
        private IMapper _mapper;
        private HotelSearchDataAccess _dataAccess;

        [Fact]
        public void GetHotels_NoHotelFilter_ReturnsListAllHotelPreviews()
        {
            Paging paging = new Paging(1000, 1);
            HotelFilter hotelFilter = new HotelFilter();

            List<HotelPreviewView> hotelPreviews = _dataAccess.GetHotels(hotelFilter, paging);

            Assert.Equal(_context.Hotels.Count(), hotelPreviews.Count);
        }

        [Fact]
        public void GetHotels_FilterOneHotelByPartialName_ReturnsListOfOneHotel()
        {
            Paging paging = new Paging(1000, 1);
            HotelFilter hotelFilter = new HotelFilter()
            {
                HotelName = "telName2"
            };

            List<HotelPreviewView> hotelPreviews = _dataAccess.GetHotels(hotelFilter, paging);

            Assert.Single(hotelPreviews);
            Assert.Equal(2, hotelPreviews[0].HotelID);
            Assert.Contains(hotelFilter.HotelName, hotelPreviews[0].HotelName);
        }

        [Fact]
        public void GetHotels_NoHotelsMatchingFilter_ReturnsEmptyList()
        {
            Paging paging = new Paging(1000, 1);
            HotelFilter hotelFilter = new HotelFilter()
            {
                HotelName = "ABCD1234$#@!"
            };

            List<HotelPreviewView> hotelPreviews = _dataAccess.GetHotels(hotelFilter, paging);

            Assert.Empty(hotelPreviews);
        }

        [Fact]
        public void GetHotelDetails_NonExistentID_ReturnsNull()
        {
            int hotelID = -1;

            HotelView hotel = _dataAccess.GetHotelDetails(hotelID);

            Assert.Null(hotel);
        }

        [Fact]
        public void GetHotelDetails_ValidID_ReturnsHotelInfoWithoutPictures()
        {
            int hotelID = 3;

            HotelView hotel = _dataAccess.GetHotelDetails(hotelID);

            Assert.NotNull(hotel);
            Assert.Equal("TestCity3", hotel.City);
            Assert.Equal("TestCountry3", hotel.Country);
            Assert.Equal("TestHotelName3", hotel.HotelName);
            Assert.Equal("TestHotelDesc3", hotel.HotelDescription);
        }

        [Fact]
        public void GetHotelPictures_NonExistentID_ReturnsEmptyList()
        {
            int hotelID = -1;

            List<string> hotelPictures = _dataAccess.GetHotelPictures(hotelID);

            Assert.Empty(hotelPictures);
        }

        [Fact]
        public void GetHotelPictures_ValidID_ReturnsListOfBase64Pictures()
        {
            int hotelID = 3;

            List<string> hotelPictures = _dataAccess.GetHotelPictures(hotelID);

            Assert.Equal(2, hotelPictures.Count);
            Assert.NotEqual(hotelPictures[0], hotelPictures[1]);
        }
        [Fact]
        public void GetOfferReviews_ReturnsListOfHotelReviews()
        {
            int hotelID = 1;
            Paging paging = new Paging();

            List<ReviewView> testedReviews = _dataAccess.GetHotelReviews(hotelID, paging);
            List<ClientReviewDb> reviews = _context.ClientReviews
                                                   .Where(cr => cr.HotelID == hotelID)
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
