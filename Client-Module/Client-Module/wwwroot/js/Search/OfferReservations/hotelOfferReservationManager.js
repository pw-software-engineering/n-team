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

    async createReservation({
        fromTime = null,
        toTime = null,
        numberOfAdults = null,
        numberOfChildren = null
    } = {}) {
        this.reservationModal.createSession();

        return this.createRequest({
            httpVerb: "POST",
            apiURL: `/hotels/${this.hotelID}/offers/${this.offerID}/reservations`,
            additionalAjaxConfig: {
                contentType: "application/json",
                data: JSON.stringify({
                    fromTime: fromTime,
                    toTime: toTime,
                    numberOfAdults: numberOfAdults,
                    numberOfChildren: numberOfChildren
                })
            }
        });
    }
}