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
               .HasForeignKey(op => op.OfferID)
               .OnDelete(DeleteBehavior.NoAction);

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
        }
    }
}
