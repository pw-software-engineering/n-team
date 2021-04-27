using Hotel.Models;
using System.Collections.Generic;

namespace Hotel.ViewModels
{
    public class OffersIndex
    {
        public IEnumerable<OfferPreview> OfferPreviews { get; set; }
        public Paging Paging { get; set; }
        public bool? IsActive { get; set; }
        public OffersIndex(IEnumerable<OfferPreview> offers, Paging paging, bool? isActive = null)
        {
            OfferPreviews = offers;
            Paging = paging;
            IsActive = isActive;
        }
    }
}
