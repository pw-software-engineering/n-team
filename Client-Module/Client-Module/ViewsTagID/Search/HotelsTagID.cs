using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.ViewsTagID.Search
{
    public class HotelsTagID
    {
        public string TopPagerNextBtnID { get; } = "top-pager-next-btn";
        public string TopPagerPreviousBtnID { get; } = "top-pager-previous-btn";
        public string TopPagerInputID { get; } = "top-pager-input";
        public string BottomPagerNextBtnID { get; } = "bottom-pager-next-btn";
        public string BottomPagerPreviousBtnID { get; } = "bottom-pager-previous-btn";
        public string BottomPagerInputID { get; } = "bottom-pager-input";
        public string HotelDisplayListID { get; } = "hotel-display-list";
        public string HotelNameFilterInputID { get; } = "hotel-name-input-filter";
        public string HotelNameValidationBoxID { get; } = "hotel-name-validation-box";
        public string CountryFilterInputID { get; } = "country-input-filter";
        public string CountryValidationBoxID { get; } = "country-validation-box";
        public string CityFilterInputID { get; } = "city-input-filter";
        public string CityValidationBoxID { get; } = "city-validation-box";
        public string ApplyFilterBtn { get; } = "apply-filter-btn";
    }
}
