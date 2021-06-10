using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess.Hotel;
using Server.Database.Models;
using System;
using System.IO;
using Xunit;

namespace Server.Tests.Database.Hotel
{
    public class HotelTokenDataAccessTest : IDisposable
    {
        private ServerDbContext _context;
        private HotelTokenDataAccess _dataAccess;

        #region TestsSetup
        public HotelTokenDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer(configurationBuilder.GetConnectionString("HotelTokenDAHotelTest"))
                    .UseInternalServiceProvider(serviceProvider);

            _context = new ServerDbContext(builder.Options, false);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            Seed();

            _dataAccess = new HotelTokenDataAccess(_context);
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

                transaction.Commit();
            }
        }
        #endregion

        #region GetHotelIdFromToken
        [Fact]
        public void GetHotelIdFromToken_GoodToken_ReturnsId()
        {
            int? id = _dataAccess.GetHotelIdFromToken("TestAccessToken1");

            Assert.True(id.HasValue);
            Assert.True(id.Value == 1);
        }
        [Fact]
        public void GetHotelIdFromToken_BadToken_ReturnsNull()
        {
            int? id = _dataAccess.GetHotelIdFromToken("NotTestAccessToken1");

            Assert.True(!id.HasValue);
        }
        #endregion
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
