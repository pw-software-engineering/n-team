using System.Collections.Generic;

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
        public int MaxGuests { get; set; }
        public string Description { get; set; }
        //Navigational Properties
        public HotelDb Hotel { get; set; }
        public List<OfferPictureDb> OfferPictures { get; set; }
        public List<ClientReviewDb> ClientReviews { get; set; }
        public List<OfferHotelRoomDb> OfferHotelRooms { get; set; }
    }
}
