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
        //Tables
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientReservation> ClientReservations { get; set; }
        public DbSet<ClientReview> ClientReviews { get; set; }
        public DbSet<HotelInfo> HotelInfos { get; set; }
        public DbSet<HotelPicture> HotelPictures { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferHotelRoom> OfferHotelRooms { get; set; }
        public DbSet<OfferPicture> OfferPictures { get; set; }


        //FluentAPI    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
