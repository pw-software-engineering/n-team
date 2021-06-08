class ReservationManager extends ServerApiManager {
    constructor({
        apiConfig = null,
        modalConfirmPopup = null,
        reviewManager = null,
        reservationPresenter = null,
        pager = null
    } = {}) {
        super(apiConfig);

        this.modalConfirmPopup = modalConfirmPopup;
        this.reviewManager = reviewManager;
        this.reservationPresenter = reservationPresenter;
        this.pager = pager;

        this.reservationEntries = {};
    }

    async onCancelReservation(reservationEntry) {
        this.modalConfirmPopup.showModal();
        this.modalConfirmPopup.displayQuestion("Are you sure you want to cancel your reservation?");
        if (await this.modalConfirmPopup.getModalInput()) {
            this.modalConfirmPopup.displayProcessing();
            try {
                await this.deleteReservation(reservationEntry.getReservationID());
            } catch (ajaxError) {
                this.modalConfirmPopup.displayError(ajaxError.message);
                return;
            }
            this.modalConfirmPopup.displaySuccess("Reservation cancelled successfully");
            this.loadReservationEntries(this.pager.getPageNumber(), this.pager.getPageSize());
        } else {
            this.modalConfirmPopup.hideModal();
        }
    }

    async loadReservationEntries(pageNumber, pageSize) {
        this.reservationPresenter.displayLoading();
        try {
            var reservationsAjaxData = await this.getReservations(pageNumber, pageSize);
            this.reservationEntries = {};
            for (var i = 0; i < reservationsAjaxData.length; i++) {
                var entry = new ReservationEntry({
                    reservationData: reservationsAjaxData[i],
                    reservationManager: this,
                    reviewManager: this.reviewManager
                });
                this.reservationEntries[entry.getReservationID()] = entry;
            }
            this.reservationPresenter.displayReservations(this.reservationEntries);
        } catch (error) {
            this.reservationPresenter.displayError(error?.message);
        }
    }

    getReservations(pageNumber, pageSize) {
        return this.createRequest({
            httpVerb: "GET",
            apiURL: `/client/reservations?pageNumber=${pageNumber}&pageSize=${pageSize}`
        });
    }

    deleteReservation(reservationID) {
        return this.createRequest({
            httpVerb: "DELETE",
            apiURL: `/client/reservations/${reservationID}`,
        });
    }
}