
namespace Server.ViewModels.Hotel
{
    public class OfferPreviewView
    {
        public int OfferID { get; set; }
        public bool IsActive { get; set; }
        public string OfferTitle { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public int MaxGuests { get; set; }
        public string OfferPreviewPicture { get; set; }
    }
}
