using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.ViewsTagID.Search
{
    public class HotelOffersTagID
    {
        public string TopPagerNextBtnID { get; } = "top-pager-next-btn";
        public string TopPagerPreviousBtnID { get; } = "top-pager-previous-btn";
        public string TopPagerInputID { get; } = "top-pager-input";
        public string BottomPagerNextBtnID { get; } = "bottom-pager-next-btn";
        public string BottomPagerPreviousBtnID { get; } = "bottom-pager-previous-btn";
        public string BottomPagerInputID { get; } = "bottom-pager-input";
        public string HotelOffersListID { get; } = "hotel-offers-list";
        public string FilterFromTimeInputID { get; } = "filter-from-time-input";
        public string FilterToTimeInputID { get; } = "filter-to-time-input";
        public string FilterMinCostInputID { get; } = "filter-min-cost-input";
        public string FilterMaxCostInputID { get; } = "filter-max-cost-input";
        public string FilterMinGuestsInputID { get; } = "filter-min-guests-input";
        public string ApplyFilterBtn { get; } = "apply-filter-btn";
    }
}
