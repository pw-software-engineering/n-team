using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
        {
        }
        #region Tables
        //Client Tables
        public DbSet<ClientDb> Clients { get; set; }
        public DbSet<ClientReservationDb> ClientReservations { get; set; }
        public DbSet<ClientReviewDb> ClientReviews { get; set; }
        //Hotel Tables
        public DbSet<HotelInfoDb> HotelInfos { get; set; }
        public DbSet<HotelPictureDb> HotelPictures { get; set; }
        public DbSet<HotelRoomDb> HotelRooms { get; set; }
        //Offer Tables
        public DbSet<OfferDb> Offers { get; set; }
        public DbSet<AvalaibleTimeIntervalDb> AvalaibleTimeIntervals { get; set; }
        public DbSet<OfferHotelRoomDb> OfferHotelRooms { get; set; }
        public DbSet<OfferPictureDb> OfferPictures { get; set; }
        #endregion
        //FluentAPI    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region PrimaryKeys
            //Client PrimaryKeys
            modelBuilder.Entity<ClientDb>()
                .HasKey(c => c.ClientID);
            modelBuilder.Entity<ClientReservationDb>()
               .HasKey(cr => cr.ReservationID);
            modelBuilder.Entity<ClientReviewDb>()
               .HasKey(cr => cr.ReviewID);

            //Hotel PrimaryKeys
            modelBuilder.Entity<HotelInfoDb>()
               .HasKey(hi => hi.HotelID);
            modelBuilder.Entity<HotelPictureDb>()
               .HasKey(hp => hp.PictureID);
            modelBuilder.Entity<HotelRoomDb>()
               .HasKey(hr => hr.RoomID);

            //Offer PrimaryKeys
            modelBuilder.Entity<OfferDb>()
               .HasKey(o => o.OfferID);
            modelBuilder.Entity<OfferHotelRoomDb>()
               .HasKey(ohr => new { ohr.OfferID, ohr.RoomID });
            modelBuilder.Entity<OfferPictureDb>()
               .HasKey(op => op.PictureID);
            modelBuilder.Entity<AvalaibleTimeIntervalDb>()
               .HasKey(ati => ati.TimeIntervalID);
            #endregion

            #region Relations
            //Relations for client tables
            modelBuilder.Entity<ClientReservationDb>()
               .HasOne(cr => cr.Room)
               .WithMany()
               .HasForeignKey(cr => cr.RoomID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservationDb>()
               .HasOne(cr => cr.Hotel)
               .WithMany()
               .HasForeignKey(cr => cr.HotelID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservationDb>()
               .HasOne(cr => cr.Offer)
               .WithMany()
               .HasForeignKey(cr => cr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservationDb>()
               .HasOne(cr => cr.Client)
               .WithMany(c => c.ClientReservations)
               .HasForeignKey(cr => cr.ClientID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClientReviewDb>()
               .HasOne(cr => cr.Client)
               .WithMany(c => c.ClientReviews)
               .HasForeignKey(cr => cr.ClientID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReviewDb>()
               .HasOne(cr => cr.Offer)
               .WithMany(o => o.ClientReviews)
               .HasForeignKey(cr => cr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReviewDb>()
               .HasOne(cr => cr.Hotel)
               .WithMany(h => h.Reviews)
               .HasForeignKey(cr => cr.HotelID);
            modelBuilder.Entity<ClientReviewDb>()
               .HasOne(cr => cr.Reservation)
               .WithOne(r => r.Review)
               .HasForeignKey<ClientReviewDb>(cr => cr.ReviewID);

            //Relations for offers tables
            modelBuilder.Entity<AvalaibleTimeIntervalDb>()
               .HasOne(ati => ati.Offer)
               .WithMany(o => o.AvalaibleTimeIntervals)
               .HasForeignKey(ati => ati.OfferID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OfferDb>()
               .HasOne(o => o.Hotel)
               .WithMany(h => h.Offers)
               .HasForeignKey(o => o.HotelID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OfferPictureDb>()
               .HasOne(op => op.Offer)
               .WithMany(o => o.OfferPictures)
               .HasForeignKey(op => op.OfferID);

            modelBuilder.Entity<OfferHotelRoomDb>()
               .HasOne(ohr => ohr.Offer)
               .WithMany(o => o.OfferHotelRooms)
               .HasForeignKey(ohr => ohr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<OfferHotelRoomDb>()
               .HasOne(ohr => ohr.Room)
               .WithMany(r => r.OfferHotelRooms)
               .HasForeignKey(ohr => ohr.RoomID)
               .OnDelete(DeleteBehavior.NoAction);

            //Relations for hotel tables
            modelBuilder.Entity<HotelPictureDb>()
               .HasOne(hp => hp.Hotel)
               .WithMany(hi => hi.HotelPictures)
               .HasForeignKey(hp => hp.HotelID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HotelRoomDb>()
               .HasOne(hr => hr.Hotel)
               .WithMany(h => h.HotelRooms)
               .HasForeignKey(hr => hr.HotelID)
               .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Seeding
            modelBuilder.Entity<ClientDb>().HasData(
                new ClientDb { ClientID = 1, Username = "TestUsername1", Email = "TestEmail1", Name = "TestName1", Surname = "TestSurname1", Password = "TestPassword1" },
                new ClientDb { ClientID = 2, Username = "TestUsername2", Email = "TestEmail2", Name = "TestName2", Surname = "TestSurname2", Password = "TestPassword2" },
                new ClientDb { ClientID = 3, Username = "TestUsername3", Email = "TestEmail3", Name = "TestName3", Surname = "TestSurname3", Password = "TestPassword3" });

            modelBuilder.Entity<HotelInfoDb>().HasData(
                new HotelInfoDb { HotelID = 1, City = "TestCity1", Country = "TestCountry1", HotelDesc = "TestHotelDesc1", AccessToken = "TestAccessToken1", HotelName = "TestHotelName1", HotelPreviewPicture = "TestHotelPreviewPicture1" },
                new HotelInfoDb { HotelID = 2, City = "TestCity2", Country = "TestCountry2", HotelDesc = "TestHotelDesc2", AccessToken = "TestAccessToken2", HotelName = "TestHotelName2", HotelPreviewPicture = "TestHotelPreviewPicture2" },
                new HotelInfoDb { HotelID = 3, City = "TestCity3", Country = "TestCountry3", HotelDesc = "TestHotelDesc3", AccessToken = "TestAccessToken3", HotelName = "TestHotelName3", HotelPreviewPicture = "TestHotelPreviewPicture3" });

            modelBuilder.Entity<HotelPictureDb>().HasData(
                new HotelPictureDb { PictureID = 1, HotelID = 2, Picture = "TestPicture1" },
                new HotelPictureDb { PictureID = 2, HotelID = 3, Picture = "TestPicture2" },
                new HotelPictureDb { PictureID = 3, HotelID = 3, Picture = "TestPicture3" });

            modelBuilder.Entity<HotelRoomDb>().HasData(
                new HotelRoomDb { RoomID = 1, HotelID = 2, HotelRoomNumber = "TestHotelRoomNumber1" },
                new HotelRoomDb { RoomID = 2, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber2" },
                new HotelRoomDb { RoomID = 3, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber3" },
                new HotelRoomDb { RoomID = 4, HotelID = 3, HotelRoomNumber = "TestHotelRoomNumber4" });

            modelBuilder.Entity<OfferDb>().HasData(
                new OfferDb { OfferID = 1, HotelID = 2, OfferTitle = "TestOfferTitle1", OfferPreviewPicture = "TestOfferPreviewPicture1", IsActive = true, IsDeleted = false, CostPerChild = 10, CostPerAdult = 11, MaxGuests = 1, Description = "TestDescription1" },
                new OfferDb { OfferID = 2, HotelID = 3, OfferTitle = "TestOfferTitle2", OfferPreviewPicture = "TestOfferPreviewPicture2", IsActive = true, IsDeleted = false, CostPerChild = 20, CostPerAdult = 22, MaxGuests = 2, Description = "TestDescription2" },
                new OfferDb { OfferID = 3, HotelID = 3, OfferTitle = "TestOfferTitle3", OfferPreviewPicture = "TestOfferPreviewPicture3", IsActive = false, IsDeleted = true, CostPerChild = 30, CostPerAdult = 33, MaxGuests = 3, Description = "TestDescription3" });

            modelBuilder.Entity<OfferPictureDb>().HasData(
                new OfferPictureDb { PictureID = 1, OfferID = 2, Picture = "TestPicture1" },
                new OfferPictureDb { PictureID = 2, OfferID = 3, Picture = "TestPicture2" },
                new OfferPictureDb { PictureID = 3, OfferID = 3, Picture = "TestPicture3" });

            modelBuilder.Entity<OfferHotelRoomDb>().HasData(
                new OfferHotelRoomDb { OfferID = 1, RoomID = 1 },
                new OfferHotelRoomDb { OfferID = 2, RoomID = 2 },
                new OfferHotelRoomDb { OfferID = 3, RoomID = 2 },
                new OfferHotelRoomDb { OfferID = 3, RoomID = 3 });

            modelBuilder.Entity<ClientReviewDb>().HasData(
               new ClientReviewDb { ReviewID = 1, OfferID = 2, ClientID = 2, HotelID = 2, Content = "TestContent1", Rating = 1, ReviewDate = new DateTime(2001, 1, 1) },
               new ClientReviewDb { ReviewID = 2, OfferID = 3, ClientID = 3, HotelID = 3, Content = "TestContent2", Rating = 2, ReviewDate = new DateTime(2001, 2, 2) },
               new ClientReviewDb { ReviewID = 3, OfferID = 3, ClientID = 3, HotelID = 3, Content = "TestContent3", Rating = 3, ReviewDate = new DateTime(2001, 3, 3) });

            modelBuilder.Entity<ClientReservationDb>().HasData(
                new ClientReservationDb { ReservationID = 1, OfferID = 2, ClientID = 2, HotelID = 2, RoomID = 2, ReviewID = 1, NumberOfAdults = 1, NumberOfChildren = 0, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 1, 2) },
                new ClientReservationDb { ReservationID = 2, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 2, ReviewID = 2, NumberOfAdults = 1, NumberOfChildren = 1, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(3001, 2, 4) },
                new ClientReservationDb { ReservationID = 3, OfferID = 3, ClientID = 3, HotelID = 3, RoomID = 3, ReviewID = 3, NumberOfAdults = 1, NumberOfChildren = 2, FromTime = new DateTime(2001, 3, 3), ToTime = new DateTime(2001, 3, 6) });

            modelBuilder.Entity<AvalaibleTimeIntervalDb>().HasData(
                new AvalaibleTimeIntervalDb { TimeIntervalID = 1, OfferID = 2, FromTime = new DateTime(2001, 1, 1), ToTime = new DateTime(2001, 2, 2) },
                new AvalaibleTimeIntervalDb { TimeIntervalID = 2, OfferID = 3, FromTime = new DateTime(2001, 2, 2), ToTime = new DateTime(2001, 3, 3) },
                new AvalaibleTimeIntervalDb { TimeIntervalID = 3, OfferID = 3, FromTime = new DateTime(2001, 3, 3), ToTime = new DateTime(2001, 4, 4) });

            #endregion
        }
    }
}
