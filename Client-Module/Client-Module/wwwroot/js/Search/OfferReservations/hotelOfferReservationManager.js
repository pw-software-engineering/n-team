class HotelOfferReservationManager extends ServerApiManager {
    constructor({
        hotelID = null,
        offerID = null,
        apiConfig = null,
        reservationModal = null
    } = {}) {
        super(apiConfig);
        this.hotelID = hotelID;
        this.offerID = offerID;
        this.reservationModal = reservationModal;
    }

    async createReservation() {
        var sessionID = this.reservationModal.createSession();
        while (true) {
            try {
                var reservationData = await this.reservationModal.getUserInput(sessionID);
            } catch (error) {
                return;
            }
            console.log(reservationData);
            if (!this.reservationModal.displayLoading(sessionID)) {
                return;
            }
            try {
                await this.createRequest({
                    httpVerb: "POST",
                    apiURL: `/hotels/${this.hotelID}/offers/${this.offerID}/reservations`,
                    additionalAjaxConfig: {
                        contentType: "application/json",
                        data: JSON.stringify(reservationData)
                    }
                });
            } catch (error) {
                if (!(this.reservationModal.displayServerError(sessionID, error.message))) {
                    return;
                }
                continue;
            }
            if (!this.reservationModal.displaySuccess(sessionID)) {
                return;
            }
        }
    }
}