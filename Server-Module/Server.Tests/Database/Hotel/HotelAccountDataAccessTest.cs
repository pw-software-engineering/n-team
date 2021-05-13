using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.DataAccess.Hotel;
using Server.Database.Models;
using Server.RequestModels.Hotel;
using Server.ViewModels;
using Server.ViewModels.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database.Hotel
{
    public class HotelAccountDataAccessTest : IDisposable
    {
        private ServerDbContext _context;
        private IMapper _mapper;
        private HotelAccountDataAccess _dataAccess;
        
        #region TestsSetup
        public HotelAccountDataAccessTest()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ServerDbContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=ServerDbHotelAccountTests;Trusted_Connection=True;MultipleActiveResultSets=true")
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

            _dataAccess = new HotelAccountDataAccess( _context, _mapper);
        }

        private void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels ON");
                _context.Hotels.AddRange(
                    new HotelDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDescription = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" , HotelPictures = new List<HotelPictureDb>()},
                    new HotelDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDescription = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2", HotelPictures = new List<HotelPictureDb>() },
                    new HotelDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDescription = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3", HotelPictures = new List<HotelPictureDb>() });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Hotels OFF;");

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelPictures ON");
                _context.HotelPictures.AddRange(
                    new HotelPictureDb { HotelID = 1, Picture = "a1", PictureID = 1 },
                    new HotelPictureDb { HotelID = 1, Picture = "a2", PictureID = 2 },
                    new HotelPictureDb { HotelID = 2, Picture = "a3", PictureID = 3 },
                    new HotelPictureDb { HotelID = 2, Picture = "a4", PictureID = 4 },
                    new HotelPictureDb { HotelID = 3, Picture = "a5", PictureID = 5 }
                    );
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelPictures OFF;");

                transaction.Commit();
            }
        }
        
        private bool Compare(HotelInfoView a, HotelInfoView b)
        {
            if (a.City != b.City || a.Country != b.Country || a.HotelDesc != b.HotelDesc
                || a.HotelName != b.HotelName ||  a.HotelPreviewPicture != b.HotelPreviewPicture)
                return false;


            if (a.HotelPictures != null)
            {
                if (b.HotelPictures == null)
                    return false;
                foreach (var p in a.HotelPictures)
                {
                    if (!b.HotelPictures.Contains(p))
                        return false;
                }
            }
            return true;
        }
        #endregion
        
        #region GetInfo
        [Fact]
        public void GetInfo_ValidId_ReturnsHotelInfoView()
        {
            int hotelId = 1;
            HotelInfoView hotelInfo = new HotelInfoView() 
            { 
                Country = "TestCountry1",
                City = "TestCity1",
                HotelDesc = "TestHotelDesc1", 
                HotelName = "TestHotelName1", 
                HotelPreviewPicture = "TestHotelPreviewPicture1", 
                HotelPictures = new List<string>() 
            };

            HotelInfoView hotelInfoTest = _dataAccess.GetHotelInfo(hotelId);

            Assert.True(Compare(hotelInfo, hotelInfoTest));
        }
    
        [Fact]
        public void GetHotelInfo_InvalidId_ReturnsNullObject()
        {
            int hotelID = 44;

            HotelInfoView hotelInfo = _dataAccess.GetHotelInfo(hotelID); 

            Assert.Null(hotelInfo);
        }
        #endregion

        #region UpdateInfo
        [Fact]
        public void UpdateHotelInfo_ValidId_UpdatesHotelInfo()
        {
            int hotelId = 3;
            HotelInfoUpdate hotelInfo = new HotelInfoUpdate() 
            { 
                HotelDesc = "TestHotelDesc1", 
                HotelName = "TestHotelName1", 
                HotelPreviewPicture = "TestHotelPreviewPicture1", 
                HotelPictures  = new List<string>()
            };

            _dataAccess.UpdateHotelInfo(hotelId, hotelInfo);
            HotelInfoView hotelInfoTest = _dataAccess.GetHotelInfo(3);

            Assert.Equal(hotelInfo.HotelDesc, hotelInfoTest.HotelDesc);
            Assert.Equal(hotelInfo.HotelName, hotelInfoTest.HotelName);
            Assert.Equal(hotelInfo.HotelPreviewPicture, hotelInfoTest.HotelPreviewPicture);
        }
        [Fact]
        public void UpdateInfo_NullHotelInfoUpdate_ThrowsArgumentNullException()
        {
            int hotelId = 1;

            Action action = () => _dataAccess.UpdateHotelInfo(hotelId, null);

            Assert.Throws<ArgumentNullException>(action);
        }
        #endregion
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
