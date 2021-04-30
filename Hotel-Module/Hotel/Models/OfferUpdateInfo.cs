using System.Collections.Generic;

namespace Hotel.Models
{
    public class OfferUpdateInfo
    {
        public bool IsActive { get; set; }
        public string OfferTitle { get; set; }
        public string Description { get; set; }
        public string OfferPreviewPicture { get; set; }
        public IEnumerable<string> OfferPictures { get; set; }
    }
}
