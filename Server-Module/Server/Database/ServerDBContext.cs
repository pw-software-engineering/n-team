using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database
{
    public class ServerDBContext:DbContext
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
        }
    }
}
