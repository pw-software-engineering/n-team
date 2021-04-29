using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database
{
    public class HotelTokeanDataAccessTest : IDisposable
    {
        private ServerDbContext _context;
        private IMapper _mapper;
        private HotelTokenDataAccess _dataAccess;

        #region TestsSetup
        public HotelTokeanDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=ServerDbTests;Trusted_Connection=True;MultipleActiveResultSets=true")
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

            _dataAccess = new HotelTokenDataAccess(_mapper, _context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelInfos ON");
                _context.HotelInfos.AddRange(
                    new HotelInfoDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDesc = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" },
                    new HotelInfoDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDesc = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2" },
                    new HotelInfoDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDesc = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3" });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelInfos OFF;");

                transaction.Commit();
            }
        }
        #endregion

        [Fact]
        public void GetHotelIdFromTokenGoodTokenTest()
        {
            int? id = _dataAccess.GetHotelIdFromToken("TestAccessToken1");

            Assert.True(id.HasValue);
            Assert.True(id.Value == 1);
        }
        [Fact]
        public void GetHotelIdFromTokenBadTokenTest()
        {
            int? id = _dataAccess.GetHotelIdFromToken("NotTestAccessToken1");

            Assert.True(!id.HasValue);
        }
    }
}
