﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.Models;
using Server.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database
{
    public class HotelAccountDataAccessTest
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

            _dataAccess = new HotelAccountDataAccess( _context);
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
                    new HotelInfoDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDescription = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" , HotelPictures = new List<HotelPictureDb>()},
                    new HotelInfoDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDescription = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2", HotelPictures = new List<HotelPictureDb>() },
                    new HotelInfoDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDescription = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3", HotelPictures = new List<HotelPictureDb>() });
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HotelInfos OFF;");

                transaction.Commit();
            }
        }
        
        private bool Same(HotelInfoDb a, HotelInfoDb b)
        {
            if (a.City != b.City || a.Country != b.Country || a.HotelDescription != b.HotelDescription
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
        public void GetInfo_GoodId_ReturnsHotelGetInfo()
        {
            var wahtWeWant = new HotelInfoDb() {HotelID =1, City = "TestCity1", Country = "TestCountry1", HotelDescription = "TestHotelDesc1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1", HotelPictures = new List<HotelPictureDb>() };
            //var wahtWeWant = _context.HotelInfos.
            //var a = _context.HotelInfos.Find(2);
            var whatWeGet = _dataAccess.GetInfo(1);

            Assert.True(Same(whatWeGet, wahtWeWant));
        }
    
        [Fact]
        public void GetInfo_BadId_ThrowsExepcion()
        {
            Assert.Throws<InvalidOperationException>(() => _dataAccess.GetInfo(44));
        }
        #endregion

        #region UpdateInfo
        [Fact]
        public void UpdateInfo_GoodId_ReturnsVoid()
        {
            var wahtWeWant = new HotelInfoDb() { City = "TestCity3", Country = "TestCountry3", HotelDescription = "TestHotelDesc1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" , HotelPictures  = null};
            wahtWeWant.HotelID = 3;
            _dataAccess.UpdateInfo(wahtWeWant);
            var whatWeGet = _dataAccess.GetInfo(3);
            Assert.True(Same(whatWeGet, wahtWeWant));
        }

        [Fact]
        public void UpdateInfo_BadId_ThrowsExepcion()
        {
            var wahtWeWant = new HotelGetInfo() { City = "TestCity1", Country = "TestCountry1", HotelDesc = "TestHotelDesc1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" };
            var p = _mapper.Map<HotelInfoDb>(wahtWeWant);
            p.HotelID = 44;
            Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(() => _dataAccess.UpdateInfo(p));
        }

        [Fact]
        public void UpdateInfo_NullPArametr_ThrowsExepcion()
        {
            Assert.Throws<NullReferenceException>(() => _dataAccess.UpdateInfo(null));
        }
        #endregion
    }
}
