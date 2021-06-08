using System;
using System.Collections.Generic;

namespace Server.ViewModels.Client
{
    public class OfferView
    {
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferDescription { get; set; }
        public List<string> OfferPictures { get; set; }
        public int MaxGuests { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<AvailabilityTimeInterval> AvailabilityTimeIntervals { get; set; }
    }

    public class AvailabilityTimeInterval
    {
        public AvailabilityTimeInterval(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public override bool Equals(object obj)
        {
            if(!(obj is AvailabilityTimeInterval))
            {
                return false;
            }
            AvailabilityTimeInterval other = obj as AvailabilityTimeInterval;
            return StartDate == other.StartDate && EndDate == other.EndDate;
        }

        public override int GetHashCode()
        {
            return StartDate.GetHashCode() ^ EndDate.GetHashCode();
        }
    }
}
