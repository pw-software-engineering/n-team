function ReservationManager({
    apiConfig = {
        apiBaseUrl: null,
        apiTokenHeaderName: null,
        apiTokenCookieName: null
    },
    modalConfirmPopup,
    reviewManager,
    reservationPresenter,
    pager
} = {}) {
    this.apiConfig = apiConfig;
    this.modalConfirmPopup = modalConfirmPopup;
    this.reviewManager = reviewManager;
    this.reservationPresenter = reservationPresenter;
    this.pager = pager;

    this.reservationEntries = {};
}

ReservationManager.prototype.onCancelReservation = async function (reservationEntry) {
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

ReservationManager.prototype.getReservations = function (pageNumber, pageSize) {
    var headers = {};
    headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
    return $.ajax({
        method: "GET",
        url: `${this.apiConfig.apiBaseUrl}/client/reservations?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        headers: headers
    }).then(
        (data, textStatus, jqXHR) => {
            console.log(data);
            return data;
        },
        (jqXHR, textStatus, errorThrown) => {
            throw Error(`GetReservation failed - ${textStatus}`);
        }
    );
}

ReservationManager.prototype.deleteReservation = function (reservationID) {
    var headers = {};
    headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
    return $.ajax({
        method: "DELETE",
        url: `${this.apiConfig.apiBaseUrl}/client/reservations/${reservationID}`,
        headers: headers
    }).then(
        (data, textStatus, jqXHR) => {
            console.log(data);
            return data;
        },
        (jqXHR, textStatus, errorThrown) => {
            throw new Error(`DeleteReservation failed - ${textStatus}`);
        }
    );
}

ReservationManager.prototype.loadReservationEntries = async function (pageNumber, pageSize) {
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
}