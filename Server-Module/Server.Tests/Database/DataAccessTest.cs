using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.AutoMapper;
using Server.Database;
using Server.Database.DataAccess;
using Server.Database.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Server.Tests.Database
{
    public class DataAccessTest
    {
        public DataAccessTest()
        {
            ContextOptions = new DbContextOptionsBuilder<ServerDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ServerDB;ConnectRetryCount=0")
                .Options;
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());
            });
            Mapper = config.CreateMapper();
        }
        protected DbContextOptions<ServerDbContext> ContextOptions { get; }
        protected IMapper Mapper { get; }
        [Fact]
        public void Can_DeleteOffer()
        {   
            using (var context = new ServerDbContext(ContextOptions))
            {
                DataAccess dataAccess = new DataAccess(Mapper, context);
                int OfferID = -1;

                dataAccess.DeleteOffer(OfferID);
                OfferDb offer = context.Offers.Find(OfferID);

                Assert.True(offer.IsDeleted);
            }
        }
        [Fact]
        public void Can_GetOffer()
        {
            using (var context = new ServerDbContext(ContextOptions))
            {
                DataAccess dataAccess = new DataAccess(Mapper, context);
                int OfferID = -1;

                Offer offerTest = dataAccess.GetOffer(OfferID);
                OfferDb offer = context.Offers.Find(OfferID);

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
        }
        [Fact]
        public void Can_GetHotelOffers()
        {
            using (var context = new ServerDbContext(ContextOptions))
            {
                DataAccess dataAccess = new DataAccess(Mapper, context);
                int hotelID = -1;

                List<OfferPreview> offersPreviewsTest = dataAccess.GetHotelOffers(hotelID);
                List<OfferDb> offersPreviews = context.Offers.Where(o => o.HotelID == hotelID).ToList();

                Assert.Equal(offersPreviews.Count, offersPreviewsTest.Count);

                for(int i=0;i<offersPreviews.Count;i++)
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
        }
        [Fact]
        public void Can_FindOfferAndGetOwner_No_Offer()
        {
            using (var context = new ServerDbContext(ContextOptions))
            {
                DataAccess dataAccess = new DataAccess(Mapper, context);
                int offerID = -4;

                int? owner = dataAccess.FindOfferAndGetOwner(offerID);
                Assert.Null(owner);
            }
        }
        [Fact]
        public void Can_FindOfferAndGetOwner()
        {
            using (var context = new ServerDbContext(ContextOptions))
            {
                DataAccess dataAccess = new DataAccess(Mapper, context);
                int offerID = -3;

                int? ownerTest = dataAccess.FindOfferAndGetOwner(offerID);
                int? owner = context.Offers.Find(offerID).HotelID;

                Assert.Equal(owner, ownerTest);
            }
        }

        [Fact]
        public void Can_AddOfferPicture()
        {
            using (var context = new ServerDbContext(ContextOptions))
            {
                DataAccess dataAccess = new DataAccess(Mapper, context);
                int offerID = -1;
                OfferPictureDb picture = new OfferPictureDb { OfferID = offerID, Picture = "TESTPICTURE"};

                List<OfferPictureDb> offerPictures = context.OfferPictures.Where(op => op.OfferID == offerID).ToList();
                dataAccess.AddOfferPicture(picture.Picture, picture.OfferID);
                List<OfferPictureDb> offerPicturesUpdated = context.OfferPictures.Where(op => op.OfferID == offerID).ToList();
                OfferPictureDb offerPicture = offerPicturesUpdated.Last();

                Assert.NotNull(offerPicture);
                Assert.Equal(offerPictures.Count + 1, offerPicturesUpdated.Count);
                Assert.Equal(offerPicture.Picture, picture.Picture);
                Assert.Equal(offerPicture.OfferID, offerID);

            }
        }

    }
}
