using AutoMapper;
using Server.Database.Models;
using Server.RequestModels;
using Server.RequestModels.Client;
using Server.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Database.DataAccess.Client
{
    public class OfferSearchDataAccess : IOfferSearchDataAccess
    {
        private readonly IMapper _mapper;
        private readonly ServerDbContext _dbContext;
        public OfferSearchDataAccess(IMapper mapper, ServerDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public List<OfferPreviewView> GetHotelOffers(int hotelID, OfferFilter offerFilter, Paging paging)
        {
            if (paging == null)
            {
                throw new ArgumentNullException("paging");
            }
            if (offerFilter == null)
            {
                throw new ArgumentNullException("offerFilter");
            }

            IEnumerable<OfferDb> offers = _dbContext.Offers.Where(o => !o.IsDeleted && o.IsActive && o.HotelID == hotelID);
            if (offerFilter.CostMax.HasValue)
            {
                offers = offers.Where(o => Math.Max(o.CostPerAdult, o.CostPerChild) <= offerFilter.CostMax);
            }
            if (offerFilter.CostMin.HasValue)
            {
                offers = offers.Where(o => Math.Min(o.CostPerAdult, o.CostPerChild) >= offerFilter.CostMin);
            }
            if (offerFilter.MinGuests.HasValue)
            {
                offers = offers.Where(o => o.MaxGuests >= offerFilter.MinGuests);
            }

            offers = offers.AsEnumerable();
            offers = offers.Where(o => CheckHotelOfferAvailability(o.OfferID, offerFilter.FromTime.Value, offerFilter.ToTime.Value))
                           .Skip((paging.PageNumber - 1) * paging.PageSize)
                           .Take(paging.PageSize);

            return _mapper.Map<List<OfferPreviewView>>(offers.ToList());
        }

        public bool CheckHotelOfferAvailability(int offerID, DateTime from, DateTime to)
        {
            foreach (OfferHotelRoomDb offerRoom in _dbContext.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID))
            {
                if (_dbContext.ClientReservations.Where(cr => cr.RoomID == offerRoom.RoomID && !(cr.ToTime < from || cr.FromTime > to)).Any())
                {
                    continue;
                }
                return true;
            }
            return false;
        }
        public OfferView GetHotelOfferDetails(int offerID)
        {
            return _mapper.Map<OfferView>(_dbContext.Offers.Find(offerID));
        }
        public List<string> GetHotelOfferPictures(int offerID)
        {
            return _dbContext.OfferPictures.Where(odb => odb.OfferID == offerID).Select(odb => odb.Picture).ToList();
        }

        public bool CheckHotelOfferExistence(int hotelID, int offerID)
        {
            return _dbContext.Offers.Any(o => o.HotelID == hotelID && o.OfferID == offerID);
        }

        public bool CheckHotelExistence(int hotelID)
        {
            return _dbContext.Hotels.Any(h => h.HotelID == hotelID);
        }

        public List<AvailabilityTimeInterval> GetHotelOfferAvailability(int hotelID, int offerID, DateTime fromTime, DateTime toTime)
        {
            if (!CheckHotelOfferExistence(hotelID, offerID))
            {
                return null;
            }
            List<AvailabilityTimeInterval> availabilityTimeIntervals = new List<AvailabilityTimeInterval>();
            foreach (OfferHotelRoomDb offerRoom in _dbContext.OfferHotelRooms.Where(ohr => ohr.OfferID == offerID))
            {
                List<AvailabilityTimeInterval> roomAvailability = GetRoomAvailabilityTimeIntervals(offerRoom.RoomID, fromTime, toTime);
                availabilityTimeIntervals = MergeAvailabilityTimeIntervals(availabilityTimeIntervals, roomAvailability);
            }
            return availabilityTimeIntervals;
        }

        List<AvailabilityTimeInterval> GetRoomAvailabilityTimeIntervals(int roomID, DateTime fromTime, DateTime toTime)
        {
            List<AvailabilityTimeInterval> roomUnavailability =
                _dbContext.ClientReservations
                .Where(cr => cr.RoomID == roomID && !(cr.ToTime < fromTime || cr.FromTime > toTime))
                .OrderBy(cdb => cdb.FromTime)
                .Select(cdb => new AvailabilityTimeInterval(cdb.FromTime, cdb.ToTime))
                .ToList();

            List<AvailabilityTimeInterval> roomAvailability = new List<AvailabilityTimeInterval>();
            if (roomUnavailability.Count == 0)
            {
                roomAvailability.Add(new AvailabilityTimeInterval(fromTime, toTime));
                return roomAvailability;
            }

            if (roomUnavailability[0].StartDate > fromTime)
            {
                roomAvailability.Add(new AvailabilityTimeInterval(fromTime, roomUnavailability[0].StartDate.AddDays(-1)));
            }

            for (int i = 1; i < roomUnavailability.Count; i++)
            {
                AvailabilityTimeInterval timeInterval = new AvailabilityTimeInterval(
                    roomUnavailability[i - 1].EndDate.AddDays(1),
                    roomUnavailability[i].StartDate.AddDays(-1));
                if (timeInterval.EndDate < timeInterval.StartDate)
                {
                    continue;
                }
                roomAvailability.Add(timeInterval);
            }

            if (roomUnavailability[roomUnavailability.Count - 1].EndDate < toTime)
            {
                roomAvailability.Add(new AvailabilityTimeInterval(roomUnavailability[roomUnavailability.Count - 1].EndDate.AddDays(1), toTime));
            }

            return roomAvailability;
        }

        List<AvailabilityTimeInterval> MergeAvailabilityTimeIntervals(
            List<AvailabilityTimeInterval> availabilityTimeIntervals1,
            List<AvailabilityTimeInterval> availabilityTimeIntervals2)
        {
            List<AvailabilityTimeInterval> resultTimeIntervals = new List<AvailabilityTimeInterval>();
            int it1 = 0, it2 = 0;
            while (it1 < availabilityTimeIntervals1.Count || it2 < availabilityTimeIntervals2.Count)
            {
                if (it1 == availabilityTimeIntervals1.Count)
                {
                    resultTimeIntervals.Add(availabilityTimeIntervals2[it2++]);
                    continue;
                }
                else if (it2 == availabilityTimeIntervals2.Count)
                {
                    resultTimeIntervals.Add(availabilityTimeIntervals1[it1++]);
                    continue;
                }
                AvailabilityTimeInterval interval1 = availabilityTimeIntervals1[it1];
                AvailabilityTimeInterval interval2 = availabilityTimeIntervals2[it2];
                if (interval1.StartDate >= interval2.StartDate)
                {
                    if (interval1.EndDate <= interval2.EndDate)
                    {
                        it1++;
                        continue;
                    }
                    resultTimeIntervals.Add(interval2);
                    it2++;
                }
                else
                {
                    resultTimeIntervals.Add(interval1);
                    it1++;
                }
            }
            return resultTimeIntervals;
        }

        public List<ReviewInfo> GetOfferReviews(int hotelID, int offerID)
        {
            if (_dbContext.Hotels.FirstOrDefault(x => x.HotelID == hotelID) == null)
                throw new Exception("cannot find hotel");
            if (_dbContext.Offers.FirstOrDefault(x => x.OfferID == offerID) == null)
                throw new Exception("cannot find offer");

            List<ReviewInfo> reviewInfos = new List<ReviewInfo>();
            var resultDB = _dbContext.ClientReviews.Where(x => x.OfferID == offerID).ToList();
            foreach (var review in resultDB)
            {
                if (review.HotelID != hotelID)
                    throw new Exception("bad request");
                var client = _dbContext.Clients.Find(review.ClientID);
                reviewInfos.Add(new ReviewInfo
                {
                    reviewID = review.ReviewID,
                    content = review.Content,
                    rating = (int)review.Rating,
                    revewerUsername = client.Name,
                    creationDate = review.ReviewDate
                });
            }
            return reviewInfos;
        }

        public List<ReviewInfo> GetHotelReviews(int hotelID, int from, int take)
        {
            if (_dbContext.Hotels.FirstOrDefault(x => x.HotelID == hotelID) == null)
                throw new Exception("cannot find hotel");

            List<ReviewInfo> reviewInfos = new List<ReviewInfo>();
            var resultDB = _dbContext.ClientReviews.Where(x => x.HotelID == hotelID).Skip(from).Take(take).ToList();
            foreach (var review in resultDB)
            {
                var client = _dbContext.Clients.Find(review.ClientID);
                reviewInfos.Add(new ReviewInfo
                {
                    reviewID = review.ReviewID,
                    content = review.Content,
                    rating = (int)review.Rating,
                    revewerUsername = client.Name,
                    creationDate = review.ReviewDate
                });
            }
            return reviewInfos;
        }
    }
}
