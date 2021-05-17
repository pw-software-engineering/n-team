using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels.Client
{
    public class ReservationData
    {
        public HotelInfoPreview HotelInfoPreview { get; set; }
        public ReservationInfoView ReservationInfo { get; set; }
        public ReservationOfferInfoPreview OfferInfoPreview { get; set; }

        public ReservationData Clone()
        {
            return new ReservationData()
            {
                HotelInfoPreview = HotelInfoPreview.Clone(),
                ReservationInfo = ReservationInfo.Clone(),
                OfferInfoPreview = OfferInfoPreview.Clone()
            };
        }
    }

    public class HotelInfoPreview
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public HotelInfoPreview Clone()
        {
            return new HotelInfoPreview()
            {
                HotelID = HotelID,
                HotelName = HotelName,
                Country = Country,
                City = City
            };
        }
    }

    public class ReservationInfoView
    {
        public int ReservationID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfAdults { get; set; }
        public int? ReviewID { get; set; }

        public ReservationInfoView Clone()
        {
            return new ReservationInfoView()
            {
                ReservationID = ReservationID,
                From = From,
                To = To,
                NumberOfChildren = NumberOfChildren,
                NumberOfAdults = NumberOfAdults,
                ReviewID = ReviewID
            };
        }
    }

    public class ReservationOfferInfoPreview
    {
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferPreviewPicture { get; set; }

        public ReservationOfferInfoPreview Clone()
        {
            return new ReservationOfferInfoPreview()
            {
                OfferID = OfferID,
                OfferTitle = OfferTitle,
                OfferPreviewPicture = OfferPreviewPicture
            };
        }
    }

    public class ReviewInfoView
    {
        public int ReviewID { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreationDate { get; set; }
        public string ReviewerUsername { get; set; }
    }
}
