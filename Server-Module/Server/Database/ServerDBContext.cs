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
        public DbSet<OfferHotelRoom> OfferHotelRooms { get; set; }
        public DbSet<OfferPicture> OfferPictures { get; set; }


        //FluentAPI    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            //Relations for client tables
            modelBuilder.Entity<ClientReservation>()
               .HasOne<HotelRoom>()
               .WithMany()
               .HasForeignKey(cr => cr.RoomID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservation>()
               .HasOne<HotelInfo>()
               .WithMany()
               .HasForeignKey(cr => cr.HotelID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservation>()
               .HasOne<Offer>()
               .WithMany()
               .HasForeignKey(cr => cr.RoomID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReservation>()
               .HasOne<Client>()
               .WithMany()
               .HasForeignKey(cr => cr.ClientID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClientReview>()
               .HasOne<Client>()
               .WithMany()
               .HasForeignKey(cr => cr.ClientID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ClientReview>()
               .HasOne<Offer>()
               .WithMany()
               .HasForeignKey(cr => cr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);

            //Relations for offers tables
            modelBuilder.Entity<Offer>()
               .HasOne<HotelInfo>()
               .WithMany()
               .HasForeignKey(o => o.HotelID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OfferPicture>()
               .HasOne<Offer>()
               .WithMany()
               .HasForeignKey(op => op.OfferID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OfferHotelRoom>()
               .HasOne<Offer>()
               .WithMany()
               .HasForeignKey(ohr => ohr.OfferID)
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<OfferHotelRoom>()
               .HasOne<HotelRoom>()
               .WithMany()
               .HasForeignKey(ohr => ohr.RoomID)
               .OnDelete(DeleteBehavior.NoAction);

            //Relations for hotel tables
            modelBuilder.Entity<HotelPicture>()
               .HasOne<HotelInfo>()
               .WithMany()
               .HasForeignKey(hp => hp.HotelID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HotelRoom>()
               .HasOne<HotelInfo>()
               .WithMany()
               .HasForeignKey(hr => hr.HotelID)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
