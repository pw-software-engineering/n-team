using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.Models
{
    public class OfferDb
    {
        //Properties
        public int OfferID { get; set; }
        public int HotelID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferPreviewPicture { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }       
        //OfferInfo Properties
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public uint MaxGuests { get; set; }
        public string Description { get; set; }
        //Navigational Properties
        public HotelInfoDb Hotel { get; set; }
        public List<AvalaibleTimeIntervalDb> AvalaibleTimeIntervals { get; set; }
        public List<OfferPictureDb> OfferPictures { get; set; }
        public List<ClientReviewDb> ClientReviews { get; set; }
        public List<OfferHotelRoomDb> OfferHotelRooms { get; set; }
        public OfferDb() { }
        public OfferDb(Offer offer, int hotelID)
        {
            HotelID = hotelID;
            OfferTitle = offer.offerTitle;
            OfferPreviewPicture = offer.offerPreviewPicture;
            IsActive = offer.isActive;
            IsDeleted = false;
            CostPerChild = offer.costPerChild;
            CostPerAdult = offer.costPerAdult;
            MaxGuests = offer.maxGuests;
            Description = offer.description;
        }
    }
}
