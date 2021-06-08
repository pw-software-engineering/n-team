class ReviewManager extends ServerApiManager {
    constructor({
        apiConfig = null,
        modalReviewPopup = null,
        modalConfirmPopup = null
    } = {}) {
        super(apiConfig);

        this.modalReviewPopup = modalReviewPopup;
        this.modalConfirmPopup = modalConfirmPopup;

        this.currentReviewSessionID = 0;
    }

    getReview(reservationID) {
        return this.createRequest({
            httpVerb: "GET",
            apiURL: `/client/reservations/${reservationID}/review`
        });
    }

    updateReview(reservationID, reviewInfo) {
        return this.createRequest({
            httpVerb: "PUT",
            apiURL: `/client/reservations/${reservationID}/review`,
            additionalAjaxConfig: {
                contentType: "application/json",
                data: JSON.stringify(reviewInfo)
            }
        });
    }

    deleteReview(reservationID) {
        return this.createRequest({
            httpVerb: "DELETE",
            apiURL: `/client/reservations/${reservationID}/review`
        });
    }

    async onEditReview(reservationEntry) {
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
                this.modalReviewPopup.displayError(ajaxError.message);
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

    async onCreateReview (reservationEntry) {
        this.onEditReview(reservationEntry);
    }

    async onDeleteReview(reservationEntry) {
        this.modalConfirmPopup.displayQuestion("Are you sure you want to delete the review?");
        this.modalConfirmPopup.showModal();
        if (await this.modalConfirmPopup.getModalInput()) {
            this.modalConfirmPopup.displayProcessing();
            try {
                await this.deleteReview(reservationEntry.getReservationID());
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
}