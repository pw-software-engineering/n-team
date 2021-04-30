using Hotel.Models;

namespace Hotel.ViewModels
{
    public class OfferEditViewModel
    {
        public Offer Offer { get; set; }
        public bool ChangePreviewPicture { get; set; }
        public bool ChangeOfferPictures { get; set; }

        public OfferEditViewModel(Offer offer)
        {
            Offer = offer;
            ChangePreviewPicture = false;
            ChangeOfferPictures = false;
        }
    }
}
