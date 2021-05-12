using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.RequestModels
{
    public class OfferInfoUpdate
    {
        public string OfferTitle { get; set; }
        public string OfferPreviewPicture { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public List<string> OfferPictures { get; set; }
    }
}
