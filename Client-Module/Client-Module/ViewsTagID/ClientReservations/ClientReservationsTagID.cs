using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Module.ViewsTagID.ClientReservations
{
    public class ClientReservationsTagID
    {
        public string TopPagerNextBtnID { get; } = "top-pager-next-btn";
        public string TopPagerPreviousBtnID { get; } = "top-pager-previous-btn";
        public string TopPagerInputID { get; } = "top-pager-input";
        public string BottomPagerNextBtnID { get; } = "bottom-pager-next-btn";
        public string BottomPagerPreviousBtnID { get; } = "bottom-pager-previous-btn";
        public string BottomPagerInputID { get; } = "bottom-pager-input";
        public string ModalCancelReservationID { get; } = "cancel-reservation-modal";
        public string ModalReservationReviewID { get; } = "reservation-review-modal";
        public string ReservationsListID { get; } = "reservations-list";
    }
}
