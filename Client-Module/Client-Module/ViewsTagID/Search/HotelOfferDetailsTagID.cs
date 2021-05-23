using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.ViewsTagID.Search
{
    public class HotelOfferDetailsTagID
    {
        public string LoadingDataDivID { get; } = "loading-data-div";
        public string MainContentDivID { get; } = "main-content-div";
        public string PreviousCarouselPictureBtnID { get; } = "previous-picture-carousel-btn";
        public string NextCarouselPictureBtnID { get; } = "next-picture-carousel-btn";
        public string CarouselPictureImgID { get; } = "carousel-picture-img";
        public string OfferTitleTextID { get; } = "offer-title-text";
        public string OfferDescriptionTextID { get; } = "offer-description-text";
        public string MaxGuestsTextID { get; } = "max-guests-text";
        public string CostPerAdultTextID { get; } = "cost-per-adult-text";
        public string CostPerChildTextID { get; } = "cost-per-child-text";
        public string OfferStatusBoxID { get; } = "offer-status-box";
        public string CreateReservationBtnID { get; } = "create-reservation-btn";

        public string ReviewTopPagerNextBtnID { get; } = "review-top-pager-next-btn";
        public string ReviewTopPagerPreviousBtnID { get; } = "review-top-pager-previous-btn";
        public string ReviewTopPagerInputID { get; } = "review-top-pager-input";
        public string ReviewBottomPagerNextBtnID { get; } = "review-bottom-pager-next-btn";
        public string ReviewBottomPagerPreviousBtnID { get; } = "review-bottom-pager-previous-btn";
        public string ReviewBottomPagerInputID { get; } = "review-bottom-pager-input";
        public string OfferReviewsListID { get; } = "offer-reviews-list";

        public string ReservationModalID { get; } = "reservation-modal";
        public string ReservationFromTimeInputID { get; } = "reservation-from-time-input";
        public string ReservationToTimeInputID { get; } = "reservation-to-time-input";
        public string ReservationNumberOfAdultsInputID { get; } = "reservation-number-of-adults-input";
        public string ReservationNumberOfChildrenInputID { get; } = "reservation-number-of-children-input";
        public string ReservationModalCreateButtonID { get; } = "reservation-create-button";
        public string ReservationModalGuestInputsErrorBoxID { get; } = "reservation-guest-inputs-error-box";
        public string ReservationModalServerErrorBoxID { get; } = "reservation-server-error-box";
        public string ReservationModalOfferAvailabilityListID { get; } = "reservation-availability-list";
    }
}
