using AutoMapper;
using Server.Database.Models;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DataAccess.Client.Review
{
    public class ReviewDataAccess:IReviewDataAccess
    {
        private readonly ServerDbContext _dbContext;
        public ReviewDataAccess(ServerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public bool IsReviewExist(int reservationID)
        {
            var wynik = _dbContext.ClientReviews.FirstOrDefault(x =>  x.ReservationID == reservationID);
            return wynik != null;
        }
        
        public int UpdateReview(int reservationID,ReviewUpdater reviewUpdater)
        {
            if (reviewUpdater == null)
                throw new Exception("info is a null");
            var wynik = _dbContext.ClientReviews.FirstOrDefault(x => x.ReservationID == reservationID);
            if (wynik == null)
                throw new Exception("cannot find review");

            wynik.Content = reviewUpdater.content;
            wynik.Rating = (uint)reviewUpdater.rating;
            _dbContext.SaveChanges();
            return wynik.ReviewID;
        }
        public int AddNewReview(int reservationID, ReviewUpdater reviewUpdater)
        {
            if (reviewUpdater == null)
                throw new Exception("info is a null");
            var reservation = _dbContext.ClientReservations.FirstOrDefault(x => x.ReservationID == reservationID);
            if (reservation == null)
                throw new Exception("cannot find reservation");
            var offer = _dbContext.Offers.FirstOrDefault(x => x.OfferID == reservation.OfferID);
            if (offer == null)
                throw new Exception("cannot find offer");

            ClientReviewDb newReviewInfo = new ClientReviewDb { 
                Rating = (uint)reviewUpdater.rating,
                Content = reviewUpdater.content,
                ReviewDate = DateTime.UtcNow,
                ClientID = reservation.ClientID.Value,
                ReservationID = reservationID,
                OfferID =offer.OfferID,
                HotelID = offer.HotelID };

            
            // tutaj muszę 2 razy zapisywać by nadać id do review a potem przepisać je do reservation
            _dbContext.ClientReviews.Add(newReviewInfo);
            _dbContext.SaveChanges();
            reservation.ReviewID = newReviewInfo.ReviewID;
            _dbContext.SaveChanges();
            return newReviewInfo.ReviewID;
        }

        public ReviewInfo GetReview(int reservationID)
        {
            var resultDB = _dbContext.ClientReviews.FirstOrDefault(x => x.ReservationID == reservationID);
            if (resultDB == null)
                throw new Exception("cannot find review");
            var client = _dbContext.Clients.FirstOrDefault(x => x.ClientID == resultDB.ClientID);
            if (client == null)
                throw new Exception("cannot find client");

            return new ReviewInfo {reviewID = resultDB.ReviewID,
                content=resultDB.Content,
                rating = (int)resultDB.Rating,
                creationDate = resultDB.ReviewDate
                ,revewerUsername = client.Name };
        }

        public void DeleteReview(int reservationID)
        {
            var reservation = _dbContext.ClientReservations.FirstOrDefault(x => x.ReservationID == reservationID);
            if (reservation == null)
                throw new Exception("cannot find reservation");
            var review = _dbContext.ClientReviews.FirstOrDefault(x => x.ReservationID == reservationID);
            if (review == null)
                throw new Exception("cannot find review");

            _dbContext.ClientReviews.Remove(review);
            reservation.ReviewID = null;
            _dbContext.SaveChanges();

        }

        public bool IsClientTheOwnerOfReservation(int reservationID,int clientID)
        {
            var reservation = _dbContext.ClientReservations.FirstOrDefault(x => x.ReservationID == reservationID);
            if (reservation == null)
                return false;
            return reservation.ClientID == clientID;
        }


    }
}
