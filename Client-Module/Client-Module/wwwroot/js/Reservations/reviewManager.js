function ReviewManager({
    apiConfig = {
        apiBaseUrl: null,
        apiTokenHeaderName: null,
        apiTokenCookieName: null
    },
    modalReviewPopup,
    modalConfirmPopup
} = {}) {
    this.apiConfig = apiConfig;
    this.modalReviewPopup = modalReviewPopup;
    this.modalConfirmPopup = modalConfirmPopup;

    this.currentReviewSessionID = 0;
}

ReviewManager.prototype.reservationReviewApiCall = function ({ httpVerb, reservationID, reviewInfo } = {}) {
    var headers = {};
    headers[this.apiConfig.apiTokenHeaderName] = getCookie(this.apiConfig.apiTokenCookieName);
    var ajaxRequest = {
        method: httpVerb,
        url: `${this.apiConfig.apiBaseUrl}/api/client/reservations/${reservationID}/review`,
        headers: headers
    };
    if (reviewInfo !== undefined) {
        ajaxRequest.data = reviewInfo;
    }
    return $.ajax(ajaxRequest).then(
        (data, textStatus, jqXHR) => {
            console.log(data);
            return data;
        },
        (jqXHR, textStatus, errorThrown) => {
            throw {
                response: jqXHR.responseJSON,
                textStatus: textStatus
            };
        }
    );
}

ReviewManager.prototype.getReview = function (reservationID) {
    return this.reservationReviewApiCall({
        httpVerb: "GET",
        reservationID: reservationID
    });
}

ReviewManager.prototype.updateReview = function (reservationID, reviewInfo) {
    return this.reservationReviewApiCall({
        httpVerb: "PUT",
        reservationID: reservationID,
        reviewInfo: reviewInfo
    });
}

ReviewManager.prototype.deleteReview = function (reservationID) {
    return this.reservationReviewApiCall({
        httpVerb: "DELETE",
        reservationID: reservationID
    });
}

ReviewManager.prototype.onEditReview = async function (reservationEntry) {
    var sessionID = ++this.currentReviewSessionID;
    var reservationID = reservationEntry.getReservationID();
    this.modalReviewPopup.clearDisplay();
    if (reservationEntry.getReviewID() !== null) {
        var reviewData = null;
        try {
            reviewData = await this.getReview(reservationID);
        } catch (ajaxError){
            console.log(ajaxError);
        }
        if (sessionID !== this.currentReviewSessionID) {
            return;
        }
        if (reviewData !== null) {
            this.modalReviewPopup.setRating(reviewData.rating);
            this.modalReviewPopup.setReviewContent(reviewData.content);
            this.modalReviewPopup.setAppearance("edit");
        } else {
            this.modalReviewPopup.displayError();
        }
    } else {
        this.modalReviewPopup.setAppearance("create");
    }
    this.modalReviewPopup.showModal();
    while (!this.modalReviewPopup.isClosed
        && sessionID == this.currentReviewSessionID
        && await this.modalReviewPopup.getModalInput()
    ) {
        var reviewInfo = {
            rating: this.modalReviewPopup.getRating(),
            content: this.modalReviewPopup.getReviewContent()
        }
        this.modalReviewPopup.displayProcessing();
        var reviewID;
        try {
            reviewID = await this.updateReview(reservationID, reviewInfo);
        } catch (ajaxError) {
            console.log(ajaxError);
            this.modalReviewPopup.displayError();
            continue;
        }
        if (sessionID !== this.currentReviewSessionID) {
            return;
        }
        this.modalReviewPopup.displaySuccess();
        if (reservationEntry.getReviewID() === null) {
            reservationEntry.setReviewID(reviewID);
            this.modalReviewPopup.setAppearance("edit");
        }
    }
    this.modalReviewPopup.hideModal();
}

ReviewManager.prototype.onCreateReview = async function (reservationEntry) {
    this.onEditReview(reservationEntry);
}

ReviewManager.prototype.onDeleteReview = async function (reservationEntry) {
    this.modalConfirmPopup.displayQuestion("Are you sure you want to delete the review?");
    this.modalConfirmPopup.showModal();
    if (await this.modalConfirmPopup.getModalInput()) {
        this.modalConfirmPopup.displayProcessing();
        try {
            await this.deleteReview(reservationEntry.getReviewID());
        } catch (ajaxError) {
            this.modalConfirmPopup.displayError(`Could not delete the review - ${ajaxError.textStatus}`);
            return;
        }
        this.modalConfirmPopup.displaySuccess("Review deleted successfully");
        reservationEntry.setReviewID(null);
    } else {
        this.modalConfirmPopup.hideModal();
    }
}