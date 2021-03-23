using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database
{
    public class ServerDBContext : DbContext
    {
        public ServerDBContext(DbContextOptions<ServerDBContext> options) : base(options)
        {
        }
        #region Tables
        //Client Tables
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientReservation> ClientReservations { get; set; }
        public DbSet<ClientReview> ClientReviews { get; set; }
        //Hotel Tables
        public DbSet<HotelInfo> HotelInfos { get; set; }
        public DbSet<HotelPicture> HotelPictures { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        //Offer Tables
        public DbSet<Offer> Offers { get; set; }
        public DbSet<AvalaibleTimeInterval> AvalaibleTimeIntervals { get; set; }
        public DbSet<OfferHotelRoom> OfferHotelRooms { get; set; }
        public DbSet<OfferPicture> OfferPictures { get; set; }
        #endregion
        //FluentAPI    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region PrimaryKeys
            //Client PrimaryKeys
            modelBuilder.Entity<Client>()
                .HasKey(c => c.ClientID);
            modelBuilder.Entity<ClientReservation>()
               .HasKey(cr => cr.ReservationID);
            modelBuilder.Entity<ClientReview>()
               .HasKey(cr => cr.ReviewID);

            //Hotel PrimaryKeys
            modelBuilder.Entity<HotelInfo>()
               .HasKey(hi => hi.HotelID);
            modelBuilder.Entity<HotelPicture>()
               .HasKey(hp => hp.PictureID);
            modelBuilder.Entity<HotelRoom>()
               .HasKey(hr => hr.RoomID);

            //Offer PrimaryKeys
            modelBuilder.Entity<Offer>()
               .HasKey(o => o.OfferID);
            modelBuilder.Entity<OfferHotelRoom>()
               .HasKey(ohr => new { ohr.OfferID, ohr.RoomID });
            modelBuilder.Entity<OfferPicture>()
               .HasKey(op => op.PictureID);
            modelBuilder.Entity<AvalaibleTimeInterval>()
               .HasKey(ati => ati.TimeIntervalID);
            #endregion

            #region Relations
            //Relations for client tables
            modelBuilder.Entity<ClientReservation>()
               .HasOne(cr => cr.Room)
               .WithMany()
               .HasForeignKey(cr => cr.RoomID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservation>()
               .HasOne(cr => cr.Hotel)
               .WithMany()
               .HasForeignKey(cr => cr.HotelID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservation>()
               .HasOne(cr => cr.Offer)
               .WithMany()
               .HasForeignKey(cr => cr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservation>()
               .HasOne(cr => cr.Client)
               .WithMany(c => c.ClientReservations)
               .HasForeignKey(cr => cr.ClientID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClientReview>()
               .HasOne(cr => cr.Client)
               .WithMany(c => c.ClientReviews)
               .HasForeignKey(cr => cr.ClientID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReview>()
               .HasOne(cr => cr.Offer)
               .WithMany(o => o.ClientReviews)
               .HasForeignKey(cr => cr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);

            //Relations for offers tables
            modelBuilder.Entity<AvalaibleTimeInterval>()
               .HasOne(ati => ati.Offer)
               .WithMany(o => o.AvalaibleTimeIntervals)
               .HasForeignKey(ati => ati.OfferID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Offer>()
               .HasOne(o => o.Hotel)
               .WithMany(h => h.Offers)
               .HasForeignKey(o => o.HotelID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OfferPicture>()
               .HasOne(op => op.Offer)
               .WithMany(o => o.OfferPictures)
               .HasForeignKey(op => op.OfferID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OfferHotelRoom>()
               .HasOne(ohr => ohr.Offer)
               .WithMany(o => o.OfferHotelRooms)
               .HasForeignKey(ohr => ohr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<OfferHotelRoom>()
               .HasOne(ohr => ohr.Room)
               .WithMany(r => r.OfferHotelRooms)
               .HasForeignKey(ohr => ohr.RoomID)
               .OnDelete(DeleteBehavior.NoAction);

            //Relations for hotel tables
            modelBuilder.Entity<HotelPicture>()
               .HasOne(hp => hp.Hotel)
               .WithMany(hi => hi.HotelPictures)
               .HasForeignKey(hp => hp.HotelID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HotelRoom>()
               .HasOne(hr => hr.Hotel)
               .WithMany(h => h.HotelRooms)
               .HasForeignKey(hr => hr.HotelID)
               .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}
