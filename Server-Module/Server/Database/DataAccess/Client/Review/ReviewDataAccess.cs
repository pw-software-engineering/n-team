using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System;
using System.Linq;

namespace Server.Database.DataAccess.Client.Review
{
    public class ReviewDataAccess : IReviewDataAccess
    {
        private readonly ServerDbContext _dbContext;
        private readonly IMapper _mapper;
        public ReviewDataAccess(ServerDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool DoesReviewExist(int reservationID)
        {
            return _dbContext.ClientReviews.Any(review => review.ReservationID == reservationID);
        }

        public int EditReview(int reservationID, ReviewUpdate reviewUpdate)
        {
            if (reviewUpdate is null)
                throw new ArgumentNullException("reviewUpdate");

            ClientReviewDb review = _dbContext.ClientReviews.First(r => r.ReservationID == reservationID);
            review.Content = reviewUpdate.Content;
            review.Rating = (uint)reviewUpdate.Rating;
            _dbContext.SaveChanges();
            return review.ReviewID;
        }
        public int AddReview(int reservationID, ReviewUpdate reviewUpdate)
        {
            if (reviewUpdate is null)
                throw new ArgumentNullException("reviewUpdate");

            ClientReservationDb reservation = _dbContext.ClientReservations.Find(reservationID);

            ClientReviewDb newReviewInfo = new ClientReviewDb
            {
                Rating = (uint)reviewUpdate.Rating,
                Content = reviewUpdate.Content,
                ReviewDate = DateTime.UtcNow,
                ClientID = reservation.ClientID,
                ReservationID = reservationID,
                OfferID = reservation.OfferID,
                HotelID = reservation.HotelID
            };

            _dbContext.ClientReviews.Add(newReviewInfo);
            _dbContext.SaveChanges();
            reservation.ReviewID = newReviewInfo.ReviewID;
            _dbContext.SaveChanges();
            return newReviewInfo.ReviewID;
        }

        public ReviewView GetReview(int reservationID)
        {
            ClientReviewDb review = _dbContext.ClientReviews.First(r => r.ReservationID == reservationID);
            ClientDb client = _dbContext.Clients.Find(review.ClientID);

            ReviewView view = _mapper.Map<ReviewView>(review);
            view.ReviewerUsername = client.Username;

            return view;
        }

        public int? FindReservationOwner(int reservationID)
        { 
            return _dbContext.ClientReservations.Find(reservationID)?.ClientID;
        }

        public void DeleteReview(int reservationID)
        {
            ClientReservationDb reservation = _dbContext.ClientReservations.Find(reservationID);
            ClientReviewDb review = _dbContext.ClientReviews.Find(reservation.ReviewID);

            reservation.ReviewID = null;
            _dbContext.ClientReviews.Remove(review);
            _dbContext.SaveChanges();
        }

        public bool IsReviewChangeAllowed(int reservationID)
        {
            DateTime reservationEndTime = _dbContext.ClientReservations.Find(reservationID).ToTime;
            return reservationEndTime <= DateTime.UtcNow && DateTime.UtcNow <= reservationEndTime.AddDays(30);
        }
    }
}
