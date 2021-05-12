using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModels
{
    public class OfferIDView
    {
        public int OfferID { get; }
        public OfferIDView(int offerID)
        {
            OfferID = offerID;
        }
    }
}
