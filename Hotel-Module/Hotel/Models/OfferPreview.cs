namespace Hotel.Models
{
    public class OfferPreview
    {
        public int OfferID { get; set; }
        public bool IsActive { get; set; }
        public string OfferTitle { get; set; }
        public double CostPerChild { get; set; }
        public double CostPerAdult { get; set; }
        public uint MaxGuests { get; set; }
        public string OfferPreviewPicture { get; set; }
    }
}
