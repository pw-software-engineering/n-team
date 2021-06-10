
namespace Server.Database.Models
{
    public class OfferHotelRoomDb
    {
        //Properties
        public int OfferID { get; set; }
        public int RoomID { get; set; }
        //Navigational Properties
        public OfferDb Offer { get; set; }
        public HotelRoomDb Room { get; set; }
    }
}
