class HotelOfferDetailsManager extends ServerApiManager {
    constructor({
        hotelID = null,
        offerID = null,
        apiConfig = null
    } = {}) {
        super(apiConfig);
        this.hotelID = hotelID;
        this.offerID = offerID;
    }

    getHotelOfferDetails() {
        return this.createRequest({
            httpVerb: "GET",
            apiURL: `/hotels/${this.hotelID}/offers/${this.offerID}`
        });
    }
}